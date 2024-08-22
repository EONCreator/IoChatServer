using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;

public class GetMessagesResponse
{
    public List<MessageClientModel> Messages { get; }

    public GetMessagesResponse(List<MessageClientModel> messages)
    {
        Messages = messages;
    }
}