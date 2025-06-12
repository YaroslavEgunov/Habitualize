using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Habitualize.Model;
using Habitualize.Services;

namespace Habitualize.ViewModel;
public class AppMessengerViewModel : ObservableObject
{
    private readonly ChatService _chatService = new();
    private readonly SaveAndLoad _saveAndLoad = new();
    private readonly string _currentUserId;
    private readonly string _friendId;
    private readonly string _friendAvatar;
    private bool _isFirstLoad = true;

    public ObservableCollection<Message> Messages { get; } = new();
    public ObservableCollection<ChatItem> ChatItems { get; } = new();
    public string NewMessage { get; set; }
    public ICommand SendMessageCommand { get; }
    public string FriendName { get; }
    public string FriendAvatar => _friendAvatar;

    public AppMessengerViewModel(Friend friend, string currentUserId)
    {
        _currentUserId = currentUserId;
        _friendId = friend.Id;
        _friendAvatar = friend.Avatar;
        FriendName = friend.Name;

        SendMessageCommand = new Command(async () => await SendMessage());

        LoadHistory();

        _chatService.OnMessageReceived += async (message) => await OnMessageReceived(message);
        _chatService.ConnectAsync(_currentUserId, _friendId);
    }

    public void UpdateMessages(IEnumerable<Message> messages)
    {
        if (_isFirstLoad)
        {

            BuildChatItems(messages);
            _isFirstLoad = false;
        }
        else
        {
            AddNewMessages(messages);
        }
    }

    private async void LoadHistory()
    {
        var history = await _chatService.LoadHistoryAsync(_currentUserId, _friendId);
        foreach (var msg in history)
        {
            msg.IsFriend = msg.SenderId != _currentUserId;
            Messages.Add(msg);

            if (msg.IsFriend && !msg.IsRead)
            {
                msg.IsRead = true;
                await _saveAndLoad.MarkMessageAsReadAsync(msg, _currentUserId, _friendId);
            }
        }
        BuildChatItems(Messages);
    }

    public void AddNewMessages(IEnumerable<Message> newMessages)
    {
        var existingIds = new HashSet<string>(
            ChatItems.OfType<MessageItem>().Select(mi => mi.Message.Id)
        );

        DateTime? lastDate = ChatItems.LastOrDefault(i => i is DateSeparatorItem) is DateSeparatorItem lastSep
            ? lastSep.Date
            : null;

        foreach (var msg in newMessages.OrderBy(m => m.Timestamp))
        {
            if (existingIds.Contains(msg.Id))
                continue;

            msg.IsFriend = msg.SenderId != _currentUserId;

            var msgDate = msg.Timestamp.Date;
            if (lastDate == null || lastDate.Value != msgDate)
            {
                ChatItems.Add(new DateSeparatorItem { Date = msgDate });
                lastDate = msgDate;
            }
            ChatItems.Add(new MessageItem { Message = msg });
        }
    }


    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(NewMessage))
            return;

        var message = new Message
        {
            Text = NewMessage,
            SenderId = _currentUserId,
            Timestamp = DateTime.UtcNow
        };

        await _chatService.SendMessageAsync(message);
        await _saveAndLoad.SaveMessageToFirebase(message, _currentUserId, _friendId);

        Messages.Add(message);
        NewMessage = string.Empty;
        BuildChatItems(Messages);
        OnPropertyChanged(nameof(NewMessage));
    }

    private async Task OnMessageReceived(Message message)
    {
        message.IsFriend = message.SenderId != _currentUserId;
        Messages.Add(message);
        BuildChatItems(Messages);
    }

    public void StopChatHistoryRefresh()
    {
        _chatService?.StopHistoryRefresh();
    }

    private void BuildChatItems(IEnumerable<Message> messages)
    {
        ChatItems.Clear();
        DateTime? lastDate = null;
        foreach (var msg in messages.OrderBy(m => m.Timestamp))
        {
            // ВАЖНО: выставляем IsFriend
            msg.IsFriend = msg.SenderId != _currentUserId;

            var msgDate = msg.Timestamp.Date;
            if (lastDate == null || lastDate.Value != msgDate)
            {
                ChatItems.Add(new DateSeparatorItem { Date = msgDate });
                lastDate = msgDate;
            }
            ChatItems.Add(new MessageItem { Message = msg });
        }
    }
}
