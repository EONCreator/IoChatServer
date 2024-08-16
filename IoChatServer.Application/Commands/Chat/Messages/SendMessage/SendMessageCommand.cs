using IoChatServer.Domain.Entities;
using IoChatServer.Services.ChatBub;
using MediatR;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class SendMessageCommand : IRequest<SendMessageResponse>
{
    public Message Message { get; set; }

    public SendMessageCommand(Message message)
    {
        Message = message;
    }
}