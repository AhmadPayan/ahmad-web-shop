#!/bin/sh
set -x

print_current_directory() {
    echo "Current directory contents:"
    ls -al
}

package_lambda_function() {
    echo "Packaging Lambda function..."

    cd src/WebShop.Lambda
    
    print_current_directory

    # Build the Lambda function in Release mode
    dotnet build -c Release

    # Navigate to the output directory containing the built binaries
    cd bin/Release/net8.0

    

    # Zip the contents into a function.zip file
    zip -r function.zip *

    echo "Lambda function packaged"
}

package_lambda_function