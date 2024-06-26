using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net.Http.Json;
using System.Text.Json;
using WebShop.Application.Models;
using WebShop.Domain.Entities;

namespace WebShop.IntegrationTests
{
    public class OrderIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public OrderIntegrationTests()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

            var serviceProvider = LocalStackConfiguration.ConfigureLocalStack();
            _sqsClient = LocalStackConfiguration.CreateSQSClient(serviceProvider);
            _queueUrl = "http://sqs.us-east-2.localhost.localstack.cloud:4566/000000000000/ahmad-orders-queue";
        }

        [Fact]
        public async Task TestPlaceOrderAndProcess()
        {
            // Arrange
            var orderRequest = new OrderRequestDto
            {
                OrderItems = new List<OrderItemDto> { new OrderItemDto { ProductId = 1, Quantity = 2 } },
                OrderReferenceId = Guid.NewGuid().ToString(),
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/order", orderRequest);
            var content = response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            var orderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();

            // Assert
            // Check if the order was placed successfully
            Assert.NotNull(orderResponse);
            Assert.NotEqual(Guid.Empty, orderResponse.OrderId);

            // Wait a moment to ensure the message is processed
            await Task.Delay(5000);

            // Verify the message in SQS
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = 1
            };
            var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);
            var messages = receiveMessageResponse.Messages;

            // It should be already consumed by the Lambda function
            Assert.Empty(messages);
        }
    }
}
