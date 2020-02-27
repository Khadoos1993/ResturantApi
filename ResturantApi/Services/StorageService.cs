using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResturantApi.Services
{
    public class StorageService
    {
        public abstract class IStorageService
        {
            public CloudStorageAccount storageAccount;

            public IStorageService(IOptions<AppSettings> options)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(options.Value.StorageConnectionString);
            }
            public abstract void Add(string data);
            public abstract string ProcessMessage();

        }

        public class QueueStorageService: IStorageService
        {
            CloudQueueClient queueClient;
            CloudQueue queue;
            CloudQueueMessage message;
            public QueueStorageService(IOptions<AppSettings> options):base(options)
            {
                queueClient = storageAccount.CreateCloudQueueClient();
                // Retrieve storage account from connection string.

                // Retrieve a reference to a container.
                queue = queueClient.GetQueueReference("myqueue");
            }
            public override void Add(string data)
            {
                // Create the queue if it doesn't already exist
                queue.CreateIfNotExists();
                message = new CloudQueueMessage(data);
                queue.AddMessage(message);
            }

            public override string ProcessMessage()
            {
                message = queue.GetMessage();
                queue.DeleteMessage(message);
                return message.AsString;
            }
        }
    }
}
