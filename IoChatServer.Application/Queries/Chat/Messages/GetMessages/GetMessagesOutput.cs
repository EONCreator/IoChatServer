using IoChatServer.Domain.Models;
using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Queries.Chat.Messages.GetMessages;

public class GetMessagesOutput : SucceededResult
{
    public List<MessageClientModel> Messages { get; }
    public static GetMessagesOutput Success(List<MessageClientModel> messages) => new(true, messages);
    public static GetMessagesOutput Failure(string error) => new(false, null, error);

    public GetMessagesOutput(bool succeeded, List<MessageClientModel> messages, string? error = null)
        : base(succeeded, error)
    {
        Messages = messages;
    }
}