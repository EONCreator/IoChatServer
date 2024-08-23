using IoChatServer.Domain.Models;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class WritingMessageOutput : SucceededResult
{
    public static WritingMessageOutput Success() => new(true);
    public static WritingMessageOutput Failure(string error) => new(false,  error);

    public WritingMessageOutput(bool succeeded, string? error = null)
        : base(succeeded, error)
    {
        
    }
}