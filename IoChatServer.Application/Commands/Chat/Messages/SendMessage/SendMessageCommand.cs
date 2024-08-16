using MediatR;
using IoChatServer.Domain.Entities;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class SendMessageCommand : IRequest<SendMessageResponse>
{
    public Message Message { get; }

    public SendMessageCommand(Message message)
    {
        Message = message;
    }
}