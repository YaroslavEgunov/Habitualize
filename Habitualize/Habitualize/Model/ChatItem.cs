namespace Habitualize.Model;

public abstract class ChatItem { }

public class MessageItem : ChatItem
{
    public Message Message { get; set; }
}

public class DateSeparatorItem : ChatItem
{
    public DateTime Date { get; set; }
    public string DateString => Date.ToString("d MMMM yyyy");
}
public class ChatItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate MessageTemplate { get; set; }
    public DataTemplate DateSeparatorTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return item switch
        {
            MessageItem => MessageTemplate,
            DateSeparatorItem => DateSeparatorTemplate,
            _ => MessageTemplate
        };
    }
}
