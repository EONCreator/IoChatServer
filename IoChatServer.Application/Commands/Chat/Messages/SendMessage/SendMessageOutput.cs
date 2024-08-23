using IoChatServer.Domain.Models;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class SendMessageOutput : SucceededResult
{
    public static SendMessageOutput Success() => new(true);
    public static SendMessageOutput Failure(string error) => new(false,  error);

    public SendMessageOutput(bool succeeded, string? error = null)
        : base(succeeded, error)
    {
        
    }
}