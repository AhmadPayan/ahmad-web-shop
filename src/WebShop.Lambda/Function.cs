using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WebShop.Lambda;

public class Function
{
    public void FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        // TODO: Publishes the mesasges into other QUEUES
        foreach (var message in evnt.Records)
        {
            context.Logger.LogInformation($"Processed message {message.Body}");
        }
    }
}
