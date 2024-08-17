namespace IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;

public class MessageModel
{
    public int Id { get; }
    public string Text { get;  }
    public DateTime Date { get; }
    public string SenderId { get; }
    public string SenderName { get; }
    public string SenderAvatar { get; }

    public MessageModel(
        int id, 
        string text, 
        DateTime date, 
        string senderId,
        string senderName,
        string senderAvatar)
    {
        Id = id;
        Text = text;
        Date = date;
        SenderId = senderId;
        SenderName = senderName;
        SenderAvatar = senderAvatar;
    }
}

public class GetMessagesResponse
{
    public List<MessageModel> Messages { get; }

    public GetMessagesResponse(List<MessageModel> messages)
    {
        Messages = messages;
    }
}