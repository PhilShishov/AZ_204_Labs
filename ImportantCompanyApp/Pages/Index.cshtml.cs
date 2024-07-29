using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ImportantCompanyApp.Pages
{
    public class IndexModel : PageModel
    {
        protected string connectionString = "<connection string from Azure Portal storage queue account>";
        protected string queueName = "ticketrequests";
        private readonly ILogger<IndexModel> _logger;
        public string WriteMessage { get; private set; } = "";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnPost(string message)
        {
            GoWriteMessage(message);
        }

        public void GoWriteMessage(string message)
        {
            QueueClient queue = new QueueClient(connectionString, queueName);
            queue.SendMessage(message);
            WriteMessage += "Message written: " + message + "!\n";
        }

        public void OnGetGoReadMessage()
        {
            QueueClient queue = new QueueClient(connectionString, queueName);
            QueueMessage message = queue.ReceiveMessage().Value;
            if (message is not null)
            {
                WriteMessage += message.InsertedOn.ToString() + ": " + message.Body.ToString();
                queue.DeleteMessage(message.MessageId, message.PopReceipt);
            }
            else
            {
                WriteMessage += "No messages found.";
            }
        }
    }
}
