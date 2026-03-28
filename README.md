# Simple C# WebSocket Server

A simple WebSocket echo server built with ASP.NET Core 8.

## Prerequisites

- .NET 8 SDK

## How to Run Locally

```bash
dotnet restore
dotnet run
```

The application will start at `http://localhost:5013`

## How to Test

Connect to the WebSocket endpoint at `ws://localhost:5013/ws` using:

- Browser DevTools console
- Postman
- wscat: `npx wscat -c ws://localhost:5013/ws`

On connection, you will receive a welcome message showing your client number and total connected clients. Send any text message to receive an echo response.

## How to Deploy on Linux

Build and run as a self-contained application:

```bash
# Build self-contained for Linux x64
dotnet publish -c Release -r linux-x64 --self-contained

# Run the application
./bin/Release/net8.0/linux-x64/WebSocketApp
```

To bind to a different port (e.g., port 80):

```bash
ASPNETCORE_URLS=http://0.0.0.0:80 ./bin/Release/net8.0/linux-x64/WebSocketApp
```

To run in background:

```bash
nohup ./bin/Release/net8.0/linux-x64/WebSocketApp &
```

## Features

- Client connection tracking
- Welcome message on connect
- Echo responses
- Connection/disconnection logging
