using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;

public class GetMessagesResponse
{
    public List<MessageModel> Messages { get; }

    public GetMessagesResponse(List<MessageModel> messages)
    {
        Messages = messages;
    }
}