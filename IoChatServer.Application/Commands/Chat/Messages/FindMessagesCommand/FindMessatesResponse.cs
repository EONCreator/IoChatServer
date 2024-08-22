using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;

public class FindMessagesResponse
{
    public List<MessageModel> Messages { get; }

    public FindMessagesResponse(List<MessageModel> messages)
    {
        Messages = messages;
    }
}