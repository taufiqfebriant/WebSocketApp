# Simple C# WebSocket Server

A simple WebSocket echo server built with ASP.NET Core 8.

## Live Demo

**Server:** http://34.143.132.27:6000  
**WebSocket:** ws://34.143.132.27:6000/ws

Connect using wscat: `wscat -c ws://34.143.132.27:6000/ws`

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

## Features

- Client connection tracking
- Welcome message on connect
- Echo responses
- Connection/disconnection logging

## Deployment Guide (Ubuntu / Linux)

This guide explains how to deploy the WebSocketApp on an Ubuntu-based Linux server (e.g., Google Cloud VM).

---

### Prerequisites

- Ubuntu 22.04 / 24.04 LTS
- .NET 8 SDK installed
- Git installed

Install dependencies:

```bash
sudo apt update
sudo apt install -y git dotnet-sdk-8.0
```

---

## Clone Repository

```bash
git clone https://github.com/taufiqfebriant/WebSocketApp.git
cd WebSocketApp
```

---

### Publish Application

Create deployment directory:

```bash
sudo mkdir -p /opt/websocket
sudo chown -R $USER:$USER /opt/websocket
```

Publish the app:

```bash
dotnet publish WebSocketApp.csproj -c Release -o /opt/websocket
```

---

### Configure systemd Service

Create a new service file:

```bash
sudo nano /etc/systemd/system/websocket.service
```

Paste the following:

```ini
[Unit]
Description=WebSocket App
After=network.target

[Service]
WorkingDirectory=/opt/websocket
ExecStart=/usr/bin/dotnet /opt/websocket/WebSocketApp.dll
Restart=always
RestartSec=10
SyslogIdentifier=websocket
User=ubuntu
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://0.0.0.0:6000

[Install]
WantedBy=multi-user.target
```

Note: Replace `User=ubuntu` with your actual username if different.

---

### Start Service

```bash
sudo systemctl daemon-reload
sudo systemctl enable websocket
sudo systemctl start websocket
```

---

### Check Service Status

```bash
sudo systemctl status websocket
```

Expected result:

```
Active: active (running)
```

---

### View Logs

```bash
journalctl -u websocket -f
```

---

### Access Application

Local access:

```
ws://localhost:6000/ws
```

External access:

```
ws://34.143.132.27:6000/ws
```

**Deployed Server:** http://34.143.132.27:6000

---

### Open Firewall (Optional)

If using Ubuntu firewall:

```bash
sudo ufw allow 6000
sudo ufw reload
```

If using Google Cloud:

- Go to VPC Network → Firewall
- Allow TCP:6000

---

### Test WebSocket Connection

Using `wscat`:

```bash
npm install -g wscat
wscat -c ws://localhost:6000/ws
```

Or from external:

```bash
wscat -c ws://34.143.132.27:6000/ws
```

---

### Summary

- Published using .NET 8
- Deployed to /opt/websocket
- Managed via systemd
- Accessible via port 6000
- Supports WebSocket connections

```

---

If you want to make it even stronger, next step I can help you:
- combine both into **one main README (system architecture + nginx)**
- or add **Nginx reverse proxy section (very impressive for reviewer)**
```
