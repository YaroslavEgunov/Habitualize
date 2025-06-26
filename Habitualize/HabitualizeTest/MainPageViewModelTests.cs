using Habitualize.Model;
using Xunit;

public class FriendTests
{
    [Fact]
    public void Friend_Properties_SetAndGet()
    {
        var friend = new Friend
        {
            Id = "1",
            Name = "Alex",
            Avatar = "avatar.png",
            Bio = "Test bio",
            HasUnreadMessages = true
        };

        Assert.Equal("1", friend.Id);
        Assert.Equal("Alex", friend.Name);
        Assert.Equal("avatar.png", friend.Avatar);
        Assert.Equal("Test bio", friend.Bio);
        Assert.True(friend.HasUnreadMessages);
    }
}