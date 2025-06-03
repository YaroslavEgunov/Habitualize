namespace Habitualize.Model;

public class Message
{
    public string Text { get; set; }
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string SenderAvatar { get; set; }
    public bool IsFriend { get; set; }
    public bool IsRead { get; set; }
    public string MessageBackground => IsFriend ? "#97d7f7" : "#D1C4E9";
    public LayoutOptions MessageAlignment => IsFriend ? LayoutOptions.Start : LayoutOptions.End;
    public int MessagePosition => IsFriend ? 0 : 1;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

}