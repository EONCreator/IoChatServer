using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;

public class FindMessagesResponse
{
    public List<MessageClientModel> Messages { get; }

    public FindMessagesResponse(List<MessageClientModel> messages)
    {
        Messages = messages;
    }
}