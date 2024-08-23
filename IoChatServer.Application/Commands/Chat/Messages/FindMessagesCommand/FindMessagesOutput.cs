using IoChatServer.Domain.Models;
using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;

public class FindMessagesOutput : SucceededResult
{
    public List<MessageClientModel> Messages { get; }
    
    public static FindMessagesOutput Success(List<MessageClientModel> messages) => new(true, messages);
    public static FindMessagesOutput Failure(string error) => new(false, null, error);

    public FindMessagesOutput(bool succeeded, List<MessageClientModel> messages, string? error = null)
        : base(succeeded, error)
    {
        Messages = messages;
    }
}