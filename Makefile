# Makefile

# Define variables
PROJECT_NAME = WebShop.Lambda
PROJECT_PATH = src/$(PROJECT_NAME)
OUTPUT_DIR = $(PROJECT_PATH)/bin/Release/net8.0/publish
ARTIFACTS_DIR = artifacts
ZIP_FILE = $(ARTIFACTS_DIR)/$(PROJECT_NAME).zip

# Default target
all: clean build publish zip set-permissions compose-up

# Clean target
clean:
	@echo "Cleaning previous builds..."
	rm -rf $(PROJECT_PATH)/bin
	rm -rf $(PROJECT_PATH)/obj
	rm -f $(ZIP_FILE)

# Build target
build:
	@echo "Building the project..."
	dotnet build $(PROJECT_PATH)/WebShop.Lambda.csproj -c Release

# Publish target
publish: build
	@echo "Publishing the project..."
	dotnet publish $(PROJECT_PATH) -c Release -o $(OUTPUT_DIR)

# Zip target
zip: publish
	@echo "Creating a zip file..."
	@if [ -d "$(ARTIFACTS_DIR)" ]; then rm -rf $(ARTIFACTS_DIR); fi
	@mkdir -p $(ARTIFACTS_DIR)
	zip -jr $(ZIP_FILE) $(OUTPUT_DIR)/*

# Set permissions target
set-permissions:
	@echo "Setting executable permissions on localstack-setup.sh..."
	@if [ "$(shell uname)" = "Darwin" ]; then chmod +x localstack-setup.sh; fi

# Docker Compose up target
compose-up: zip set-permissions
	@echo "Running docker-compose up..."
	docker-compose up --build -d

stop:
	@echo "Running docker-compose down..."
	docker-compose down

# Run all targets
.PHONY: all build publish zip set-permissions compose-up stop