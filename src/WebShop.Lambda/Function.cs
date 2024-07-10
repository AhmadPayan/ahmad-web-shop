using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WebShop.Lambda;

public class Function
{
    public void FunctionHandler(SQSEvent @event, ILambdaContext context)
    {
        // TODO: Publishes the messages into other QUEUES
        context.Logger.LogInformation("Lambda function processed an SQS event !");
        
        foreach (var message in @event.Records)
        {
            context.Logger.LogInformation($"Processed message {message.Body}");
        }
    }
}
