using Habitualize.Model;
using Habitualize.ViewModel;
using Habitualize.Services;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Habitualize.View;

public partial class AppMessenger : ContentPage
{
    private bool _isSubscribed = false;
    private readonly SaveAndLoad _saveAndLoad = new SaveAndLoad();
    private Timer _timer;
    private string _userId1; // Текущий пользователь
    private string _userId2; // Собеседник
    public Friend ChatFriend { get; set; }
    public string FriendName { get; set; }
    public string FriendAvatar { get; set; }

    public AppMessenger(Friend friend)
	{
        InitializeComponent();
        ChatFriend = friend;
        FriendName = friend?.Name;
        FriendAvatar = friend?.Avatar;
        NavigationPage.SetHasNavigationBar(this, false);
        var saveAndLoadService = new SaveAndLoad();
        _userId1 = saveAndLoadService.UserId; // текущий пользователь
        _userId2 = friend?.Id;
        var currentUserId = saveAndLoadService.UserId;
        BindingContext = new AppMessengerViewModel(friend, currentUserId);
        StartTimer();
    }

    private void StartTimer()
    {
        _timer = new Timer(3000);
        _timer.Elapsed += async (s, e) => await LoadMessagesAsync();
        _timer.AutoReset = true;
        _timer.Start();
    }

    private async Task LoadMessagesAsync()
    {
        var messages = await _saveAndLoad.LoadMessagesFromFirebase(_userId1, _userId2);
        if (messages == null) return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (BindingContext is AppMessengerViewModel vm)
            {
                vm.UpdateMessages(messages);
                await Task.Delay(100); // Дать UI обновиться
                ScrollToLastMessage();
            }
        });
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is AppMessengerViewModel vm)
            vm.StopChatHistoryRefresh();

        await Navigation.PopAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ScrollToLastMessage();

        if (!_isSubscribed && BindingContext is AppMessengerViewModel vm)
        {
            vm.Messages.CollectionChanged += Messages_CollectionChanged;
            _isSubscribed = true;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
        _timer?.Dispose();
        if (_isSubscribed && BindingContext is AppMessengerViewModel vm)
        {
            vm.Messages.CollectionChanged -= Messages_CollectionChanged;
            _isSubscribed = false;
        }
    }

    private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // Скроллим только если добавлено новое сообщение
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            ScrollToLastMessage();
    }

    private void ScrollToLastMessage()
    {
        if (MessagesCollectionView.ItemsSource is IList<object> items && items.Count > 0)
        {
            var lastMessageItem = items.LastOrDefault(i => i is MessageItem);
            if (lastMessageItem != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagesCollectionView.ScrollTo(lastMessageItem, position: ScrollToPosition.End, animate: true);
                });
            }
        }
    }

}