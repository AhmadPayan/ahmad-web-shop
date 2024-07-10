#!/bin/sh
set -e
export AWS_PROFILE=localstack-ahmad
print_current_directory() {
    echo "Current directory contents:"
    ls -al
}

# Function to wait for LocalStack to be ready
wait_for_localstack() {
    echo "Waiting for LocalStack to be ready..."
    until $(curl --output /dev/null --silent --head --fail http://localhost:4566); do
        printf '.'
        sleep 5
    done
    echo "LocalStack is ready."
}

# Function to package Lambda function into a ZIP file
package_lambda_function() {
    echo "Packaging Lambda function..."

    # Navigate to the Lambda function project directory
    cd /

    cd src/

    # Move the ZIP file to the local S3 bucket of LocalStack
    awslocal s3 mb s3://local-lambda-bucket  # Create S3 bucket if it doesn't exist
    awslocal s3 cp WebShop.Lambda.zip s3://local-lambda-bucket/WebShop.Lambda.zip

    echo "Lambda function packaged and uploaded to S3."
}

# Function to deploy CloudFormation stack
deploy_cloudformation_stack() {
    echo "Creating CloudFormation stack..."
    aws --endpoint-url=http://localhost:4566 cloudformation create-stack \
        --stack-name WebShopStack \
        --template-body file:///etc/localstack/init/ready.d/aws-cloudformation-template.yml \

    # Wait for stack creation to complete
    aws --endpoint-url=http://localhost:4566 cloudformation wait stack-create-complete --stack-name WebShopStack

    echo "CloudFormation stack created successfully."
}

# Main script flow
wait_for_localstack
package_lambda_function
deploy_cloudformation_stack

echo "LocalStack setup completed."