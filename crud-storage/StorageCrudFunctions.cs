using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;

namespace Serverless
{
    public static class StorageCrudFunctions
    {
        private const string route = "todos";

        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = route)]
            TodoCreateModel todoCreateModel,
            [Table("todos", Connection = "TableStorage")]
            IAsyncCollector<TodoTableEntity> todoTable,
            ILogger log)
        {
            log.LogInformation("Creating a new TODO item.");

            var todo = new Todo() { TaskDescription = todoCreateModel.TaskDescription };
            await todoTable.AddAsync(todo.ToTableEntity());

            return new OkObjectResult(todo);
        }

        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = route)]
            HttpRequest req,
            [Table("todos", Connection = "TableStorage")]
            CloudTable todoTable,
            ILogger log)
        {
            log.LogInformation("Getting TODO list items.");

            var query = new TableQuery<TodoTableEntity>();
            var items = await todoTable.ExecuteQuerySegmentedAsync(query, null);
            var todoItems = items.Select(Mappings.ToTodo).OrderBy(i => i.CreatedTime); // OrderBy is executed locally

            return new OkObjectResult(todoItems);
        }

        [FunctionName("GetTodoById")]
        public static IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = route + "/{id}")]
            HttpRequest req,
            [Table("todos", "TODO", "{id}", Connection = "TableStorage")]
            TodoTableEntity todo,
            ILogger log, string id)
        {
            log.LogInformation("Getting TODO item by ID.");

            if (todo == null)
            {
                log.LogInformation($"TODO {id} not found");

                return new NotFoundResult();
            }

            return new OkObjectResult(todo.ToTodo());
        }

        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = route + "/{id}")]
            TodoUpdateModel updatedTodo,
            [Table("todos", Connection = "TableStorage")]
            CloudTable todoTable,
            ILogger log, string id)
        {
            log.LogInformation("Updating TODO item.");

            var findOperation = TableOperation.Retrieve<TodoTableEntity>("TODO", id);
            var findResult = await todoTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                log.LogInformation($"TODO {id} not found");

                return new NotFoundResult();
            }

            var existingRow = (TodoTableEntity)findResult.Result;

            existingRow.IsCompleted = updatedTodo.IsCompleted;

            if (!string.IsNullOrEmpty(updatedTodo.TaskDescription))
            {
                existingRow.TaskDescription = updatedTodo.TaskDescription;
            }

            var replaceOperation = TableOperation.Replace(existingRow);
            await todoTable.ExecuteAsync(replaceOperation);

            return new OkObjectResult(existingRow.ToTodo());
        }

        [FunctionName("DeleteTodo")]
        public static async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = route + "/{id}")]
            HttpRequest req,
            [Table("todos", Connection = "TableStorage")]
            CloudTable todoTable,
            ILogger log, string id)
        {
            log.LogInformation("Deleting TODO item.");

            var deleteOperation = TableOperation.Delete(
                new TableEntity() { PartitionKey = "TODO", RowKey = id, ETag = "*" });

            try
            {
                var deleteResult = await todoTable.ExecuteAsync(deleteOperation);
            }
            catch (StorageException e) when (e.RequestInformation.HttpStatusCode == 404)
            {
                log.LogInformation($"TODO {id} not found");

                return new NotFoundResult();
            }

            return new OkResult();
        }
    }
}
