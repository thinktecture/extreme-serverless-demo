using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Serverless
{
    public class TodoTableEntity : TableEntity
    {
        public DateTime CreatedTime { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}