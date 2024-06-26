# AWS Lambda Empty Function Project

This starter project consists of:
* Function.cs - class file containing a class with a single function handler method
* aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS

You may also have a test project depending on the options selected.

The generated function handler is a simple method accepting a string argument that returns the uppercase equivalent of the input string. Replace the body of this method, and parameters, to suit your needs. 

## Here are some steps to follow from Visual Studio:

To deploy your function to AWS Lambda, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed function open its Function View window by double-clicking the function name shown beneath the AWS Lambda node in the AWS Explorer tree.

To perform testing against your deployed function use the Test Invoke tab in the opened Function View window.

To configure event sources for your deployed function, for example to have your function invoked when an object is created in an Amazon S3 bucket, use the Event Sources tab in the opened Function View window.

To update the runtime configuration of your deployed function use the Configuration tab in the opened Function View window.

To view execution logs of invocations of your function use the Logs tab in the opened Function View window.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Execute unit tests
```
    cd "WebShop.Lambda/test/WebShop.Lambda.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "WebShop.Lambda/src/WebShop.Lambda"
    dotnet lambda deploy-function
```


awslocal lambda list-functions --endpoint-url http://localhost:4566
awslocal logs describe-log-groups --endpoint-url=http://localhost:4566
awslocal logs describe-log-streams --log-group-name '/aws/lambda/ProcessOrderLambda' --endpoint-url=http://localhost:4566

# See the logs
awslocal logs describe-log-streams --log-group-name '/aws/lambda/WebShopStack-ProcessOrderLambda-c3481948' --endpoint-url http://localhost:4566

- Replace the value of --log-stream-name to the actual stream name from the previous command

awslocal logs get-log-events --log-group-name '/aws/lambda/WebShopStack-ProcessOrderLambda-361c5c94' --log-stream-name '2024/06/28/[$LATEST]38828e97915b42792b64647bd3e1f771' --endpoint-url http://localhost:4566


## Invoke the lambda function

awslocal lambda invoke --function-name ProcessOrderLambda --payload '{}' response.json --endpoint-url http://localhost:4566

