#!/bin/sh

# Define the AWS profile and the endpoint URL
AWS_PROFILE="localstack-ahmad"
ENDPOINT_URL="http://localhost:4566"
LOG_GROUP_PATTERN="/aws/lambda/WebShopStack-ProcessOrderLambda"

# Function to describe log groups and find the one that matches the pattern
get_log_group_name() {
    echo "Fetching log group name..."
    LOG_GROUP_NAME=$(awslocal logs describe-log-groups --endpoint-url=$ENDPOINT_URL --profile $AWS_PROFILE | jq -r --arg pattern "$LOG_GROUP_PATTERN" '.logGroups[] | select(.logGroupName | contains($pattern)) | .logGroupName')
    if [ -z "$LOG_GROUP_NAME" ]; then
        echo "No log group found matching pattern $LOG_GROUP_PATTERN."
        exit 1
    fi
    echo "Log group found: $LOG_GROUP_NAME"
}

# Function to describe log streams
describe_log_streams() {
    echo "Describing log streams..."
    awslocal logs describe-log-streams --log-group-name $LOG_GROUP_NAME --endpoint-url=$ENDPOINT_URL --profile $AWS_PROFILE
}

# Function to get log events from the latest log stream
get_log_events() {
    echo "Fetching log events..."
    LOG_STREAM_NAME=$(awslocal logs describe-log-streams --log-group-name $LOG_GROUP_NAME --endpoint-url=$ENDPOINT_URL --profile $AWS_PROFILE | jq -r '.logStreams[0].logStreamName')
    if [ -z "$LOG_STREAM_NAME" ]; then
        echo "No log stream found."
    else
        awslocal logs get-log-events --log-group-name $LOG_GROUP_NAME --log-stream-name $LOG_STREAM_NAME --endpoint-url=$ENDPOINT_URL --profile $AWS_PROFILE
    fi
}

# Execute the functions
get_log_group_name
describe_log_streams
get_log_events