using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda;
using Amazon.Runtime;
using Amazon.SQS;
using LocalStack.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebShop.IntegrationTests
{
    public static class LocalStackConfiguration
    {
        public static ServiceProvider ConfigureLocalStack()
        {
            var services = new ServiceCollection();

            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Configure LocalStack and AWS options
            services.AddLocalStack(configuration);
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Credentials = new BasicAWSCredentials("test", "test"),
                Region = RegionEndpoint.USEast2
            });
            var sqsConfig = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.USEast2, 
                ServiceURL = configuration["LocalStack:LocalStackHost"], // LocalStack endpoint URL
                EndpointDiscoveryEnabled = true // Ensure this is set appropriately
            };
            var sqsClient = new AmazonSQSClient(sqsConfig);

            // Register the SQS client in the DI container
            services.AddSingleton<IAmazonSQS>(sqsClient);
            services.AddAWSService<IAmazonLambda>();

            return services.BuildServiceProvider();
        }

        public static IAmazonSQS CreateSQSClient(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IAmazonSQS>();
        }

        public static IAmazonLambda CreateLambdaClient(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IAmazonLambda>();
        }
    }
}