using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExtremeServerless.Functions
{
    public static class NotifyClientsWithMessage
    {
        private static AzureSignalR signalR = new AzureSignalR(
            Environment.GetEnvironmentVariable("SignalR"));
        
        [FunctionName("NotifyClientsWithMessage")]
        public static async Task Run(
            [CosmosDBTrigger("chatsystem", "messages", 
                ConnectionStringSetting = "CosmosDB")]
            IReadOnlyList<Document> documents, 
            TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                var messagesToBroadcast = documents.Select((doc) => new
                {
                    message = doc.GetPropertyValue<string>("message"),
                    user = doc.GetPropertyValue<User>("user")
                });

                var ser = new JsonSerializerSettings();
                ser.ContractResolver = new CamelCasePropertyNamesContractResolver();
                
                await signalR.SendAsync("chatServerlessHub", "NewMessages", 
                    JsonConvert.SerializeObject(messagesToBroadcast, ser));
            }
        }
    }
}
