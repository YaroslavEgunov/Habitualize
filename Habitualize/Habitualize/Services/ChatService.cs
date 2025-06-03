using Microsoft.AspNetCore.SignalR.Client;
using Habitualize.Model;

namespace Habitualize.Services;

public class ChatService
{
    private HubConnection _connection;
    private string _friendId;
    private string _currentUserId;
    private readonly SaveAndLoad _saveAndLoad = new();
    public event Action<Message> OnMessageReceived;

    public async Task ConnectAsync(string userId, string friendId)
    {
        _currentUserId = userId;
        _friendId = friendId;
        _connection = new HubConnectionBuilder()
            .WithUrl("https://habit/chatHub?userId=" + userId)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<Message>("ReceiveMessage", async (message) =>
        {
            await _saveAndLoad.SaveMessageToFirebase(message, _currentUserId, _friendId);
            OnMessageReceived?.Invoke(message);
        });

        await _connection.StartAsync();
    }

    public async Task SendMessageAsync(Message message)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            await _connection.InvokeAsync("SendMessage", message);
            await _saveAndLoad.SaveMessageToFirebase(message, _currentUserId, _friendId);
        }
    }

    public async Task<List<Message>> LoadHistoryAsync(string userId, string friendId)
    {
        var since = DateTime.UtcNow.AddMonths(-1);
        return await _saveAndLoad.LoadMessagesFromFirebase(userId, friendId, since);
    }
}