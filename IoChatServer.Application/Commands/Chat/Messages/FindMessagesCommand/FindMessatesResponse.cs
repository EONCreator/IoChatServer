using IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;

namespace IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;

public class FindMessagesResponse
{
    public List<MessageModel> Messages { get; }

    public FindMessagesResponse(List<MessageModel> messages)
    {
        Messages = messages;
    }
}