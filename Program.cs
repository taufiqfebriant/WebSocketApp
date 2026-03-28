using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();

var clients = new List<WebSocket>();

app.MapGet("/", () => "WebSocket Server Running");

app.MapGet("/ws", async (HttpContext context) =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
    clients.Add(webSocket);
    
    var clientId = clients.Count;
    app.Logger.LogInformation("Client {ClientId} connected. Total clients: {Total}", clientId, clients.Count);

    var welcomeMessage = Encoding.UTF8.GetBytes($"Welcome! You are client #{clientId}. Total connected: {clients.Count}");
    await webSocket.SendAsync(welcomeMessage, WebSocketMessageType.Text, true, CancellationToken.None);

    var buffer = new byte[1024];
    WebSocketReceiveResult result;

    try
    {
        while (webSocket.State == WebSocketState.Open)
        {
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                break;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            app.Logger.LogInformation("Client {ClientId} sent: {Message}", clientId, message);

            var echoMessage = Encoding.UTF8.GetBytes($"Echo: {message}");
            await webSocket.SendAsync(echoMessage, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error handling client {ClientId}", clientId);
    }
    finally
    {
        clients.Remove(webSocket);
        app.Logger.LogInformation("Client {ClientId} disconnected. Total clients: {Total}", clientId, clients.Count);
        webSocket.Dispose();
    }
});

app.Run();
