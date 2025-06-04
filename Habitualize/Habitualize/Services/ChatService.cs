using Microsoft.AspNetCore.SignalR.Client;
using Habitualize.Model;
using System.Threading;


namespace Habitualize.Services;

public class ChatService
{
    private HubConnection _connection;
    private string _friendId;
    private string _currentUserId;
    private readonly SaveAndLoad _saveAndLoad = new();
    public event Action<Message> OnMessageReceived;
    public event Action<List<Message>> OnHistoryUpdated;


    private Timer _refreshTimer;


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

        StartHistoryRefresh();
    }

    private void StartHistoryRefresh()
    {
        _refreshTimer?.Dispose();
        _refreshTimer = new Timer(async _ =>
        {
            var history = await LoadHistoryAsync(_currentUserId, _friendId);
            OnHistoryUpdated?.Invoke(history);
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    public void StopHistoryRefresh()
    {
        _refreshTimer?.Dispose();
        _refreshTimer = null;
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