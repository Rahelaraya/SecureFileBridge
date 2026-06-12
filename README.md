# SecureFileBridge

## Overview

SecureFileBridge is a backend application designed to monitor file flow activity, system health, and heartbeat status. The application provides operational visibility through monitoring services, Health Checks, and OpenTelemetry.

### Features

- File flow monitoring
- Health status monitoring
- Heartbeat tracking
- Health Checks for system availability
- OpenTelemetry for logging, metrics, and tracing
- Dependency Injection for maintainable architecture

## Project Structure

- **Controllers** – API endpoints for monitoring and status information
- **Interfaces** – Service contracts and abstractions
- **Services** – Business logic for monitoring and health checks
- **Models** – Data models used throughout the application
- **Extensions** – Health Check and OpenTelemetry configuration
- **Configuration** – Application settings and configuration classes

## Endpoints

### GET `/api/Files/Fileflow-status`

Returns information about the current file flow status.

### GET `/api/Health/status`

Returns the overall health status of the application.

### GET `/api/Health/heartbeat`

Returns heartbeat monitoring information.

## Purpose

The purpose of SecureFileBridge is to provide a reliable way to monitor file flows, application health, and heartbeat activity. The application helps identify issues early and provides operational visibility for system monitoring and maintenance.
