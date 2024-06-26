using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WebShop.Application.Services;
using WebShop.Infrastructure.Publishers;
using WebShop.Infrastructure.Repositories;

namespace WebShop.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Credentials = new BasicAWSCredentials("test", "test"),
                Region = RegionEndpoint.USEast2 // Adjust region as needed
            });
            var sqsConfig = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.USEast2, // Dummy region, adjust as needed
                ServiceURL = Configuration["AWS_ENDPOINT"], // LocalStack endpoint URL
                EndpointDiscoveryEnabled = false // Ensure this is set appropriately
            };
            var sqsClient = new AmazonSQSClient(sqsConfig);

            // Register the SQS client in the DI container
            services.AddSingleton<IAmazonSQS>(sqsClient);

            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddOptions();
            services.Configure<LocalStackOptions>(Configuration.GetSection("LocalStack"));
            services.AddScoped<IEventPublisher>(sp =>
            {
                var sqsClient = sp.GetRequiredService<IAmazonSQS>();
                var localStackOptions = sp.GetRequiredService<IOptions<LocalStackOptions>>();

                // Get queueUrl from options
                var queueUrl = localStackOptions.Value.QueueUrl;

                // Validate queueUrl
                if (string.IsNullOrEmpty(queueUrl))
                {
                    throw new InvalidOperationException("QueueUrl is not configured or invalid.");
                }

                return new EventPublisher(sqsClient, queueUrl);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebShop.Api", Version = "v1" });
            });

            return services;
        }
    }
}
