## System Overview

**SecureFileBridge** is a backend application designed to securely read files from a local or network folder, process them, and send file data to RabbitMQ for further handling by other systems.

### Contexts

The application works as a bridge between file storage and message-based communication. It monitors a configured source folder, reads available files, validates them, and prepares the file content to be published as messages. The system is designed to support secure configuration, retry handling, logging, monitoring, and health checks.

### Controllers

The API exposes endpoints for checking application status and interacting with file processing functionality. Controllers are responsible for receiving HTTP requests and calling the correct services. They do not contain business logic directly, but delegate work to the application services.

### Models

Models define the data structures used by the application. File-related models store information such as file name, file path, content, file size, creation date, and processing status. Configuration models such as `FilePollingSettings` and `RabbitMqSettings` store values from `appsettings.json`, including source path, polling interval, retry attempts, RabbitMQ host, queue name, username, and password.

### Services

Services contain the main business logic of the application. The file reader service reads files from the configured folder, validates them, and prepares the content. The RabbitMQ publisher service sends processed file data to RabbitMQ. Retry services handle temporary failures, such as network issues or unavailable RabbitMQ connections.

### Repositories

Repositories can be used to store and retrieve file processing information. They separate database or storage logic from the rest of the application. This makes the application easier to maintain and test.

### Configuration

The application uses `appsettings.json` to manage settings such as file paths, polling interval, RabbitMQ connection details, retry configuration, logging level, and health check settings. Sensitive values such as passwords should not be hardcoded and should be stored securely, for example using environment variables or Azure Key Vault.

### Health Checks

Health checks are used to monitor if the application is working correctly. The system can check if the API is running, if the source folder is reachable, and if RabbitMQ is available. This makes it easier to detect problems early.

### OpenTelemetry

OpenTelemetry is used for observability. It helps collect logs, metrics, and traces from the application. This makes it easier to understand how files are processed, where errors happen, and how the system behaves in production.

### Logging

The application uses structured logging to track important events, errors, retries, and successful file processing. Logs help developers and operations teams troubleshoot problems and understand the flow of the application.

### Security

SecureFileBridge is designed with security in mind. Sensitive configuration values should be protected, file access should be limited to required folders, and RabbitMQ credentials should not be exposed in source code. The application should also validate files before processing them.

## Available Endpoints

1️⃣ GET /api/Files/Fileflow-status

Returns information about the current file flow status.

What it does:
Retrieves file flow monitoring information
Returns current flow status
Helps identify issues in the file processing flow
Provides operational visibility

2️⃣ GET /api/Health/status

Returns the overall health status of the application.

What it does:
Checks application health
Returns health status and messages
Displays heartbeat information
Shows component-level status details
Helps monitor system availability

3️⃣ GET /api/Health/heartbeat

Returns heartbeat monitoring information.

What it does:
Returns the latest heartbeat timestamp
Displays heartbeat count
Indicates whether heartbeat monitoring is healthy
Provides current monitoring information

---

## Main Flow

1. The application reads files from a configured folder.
2. Each file is validated and prepared for processing.
3. File data is converted into a message.
4. The message is published to RabbitMQ.
5. Logs, traces, and health checks are used to monitor the system.
6. Other applications can consume the message from RabbitMQ.

## Purpose

The purpose of SecureFileBridge is to create a secure and reliable connection between local file storage and a message queue system. It helps automate file transfer, reduce manual work, improve monitoring, and make system integration more stable.

