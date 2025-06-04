using Habitualize.Model;
using Habitualize.ViewModel;
using Habitualize.Services;

namespace Habitualize.View;

public partial class AppMessenger : ContentPage
{
    public Friend ChatFriend { get; set; }
    public string FriendName { get; set; }
    public string FriendAvatar { get; set; }
    private bool _isSubscribed = false;

    public AppMessenger(Friend friend)
	{
        InitializeComponent();
        ChatFriend = friend;
        FriendName = friend?.Name;
        FriendAvatar = friend?.Avatar;
        NavigationPage.SetHasNavigationBar(this, false);
        var saveAndLoadService = new SaveAndLoad();
        var currentUserId = saveAndLoadService.UserId;
        BindingContext = new AppMessengerViewModel(friend, currentUserId);
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
        if (MessagesCollectionView.ItemsSource is IList<Message> messages && messages.Count > 0)
        {
            // ScrollTo может не сработать сразу после добавления, используйте Device.BeginInvokeOnMainThread
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagesCollectionView.ScrollTo(messages[^1], position: ScrollToPosition.End, animate: true);
            });
        }
    }
}