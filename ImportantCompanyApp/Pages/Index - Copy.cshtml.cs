using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportantCompanyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string WriteMessage { get; private set; } = "";
        protected string connectionString = "DefaultEndpointsProtocol=https;" 
            +"AccountName=newqueue9531;"
            +"AccountKey=wqwCvpjlKYDaJQ0NJcCrMaPn3Yt1SunwdCKcB62mGE7HG2VcTv6pv2fFw4Hk/+9w84YERU/+f5io+AStU8GTfA==;"
            +"EndpointSuffix=core.windows.net";
        protected string queueName = "ticketrequests";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnPost()
        {
            GoWriteMessage();
        }

        public void GoWriteMessage()
        {
            
            QueueClient queue = new QueueClient(connectionString, queueName);
            queue.SendMessage("Hello, Azure!");
            WriteMessage += "Message written: Hello, Azure!\n";
        }

        public void OnGetGoReadMessage()
        {
            QueueClient queue = new QueueClient(connectionString, queueName);
            QueueMessage message = queue.ReceiveMessage().Value;
            if (message is not null)
            {
                WriteMessage += message.InsertedOn.ToString() + ": " + message.Body.ToString();
                queue.DeleteMessage(message.MessageId, message.PopReceipt);
            } else
            {
                WriteMessage += "No messages found";
            }
        }

    }
}
