using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebShop.Domain.Events;

namespace WebShop.Infrastructure.Publishers
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public EventPublisher(IAmazonSQS sqsClient, string queueUrl)
        {
            _sqsClient = sqsClient;
            _queueUrl = queueUrl;
        }

        public async Task PublishOrderPlacedEventAsync(OrderPlacedEvent orderPlacedEvent)
        {
            var messageBody = JsonSerializer.Serialize(orderPlacedEvent);
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = messageBody,
                MessageAttributes = new Dictionary<string, MessageAttributeValue> {
                    {"APP_NAME",
                        new MessageAttributeValue { StringValue = "AhmadTest", DataType= "String" }
                    }
                }
            };

            await _sqsClient.SendMessageAsync(sendMessageRequest);
        }
    }
}
