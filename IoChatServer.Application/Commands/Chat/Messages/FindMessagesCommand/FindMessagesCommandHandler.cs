using MediatR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;

public class FindMessagesCommandHandler : IRequestHandler<FindMessagesCommand, FindMessagesResponse>
{
    private IUserService _userService;
    private IRepository _repository;
    
    public FindMessagesCommandHandler(
        IUserService userService,
        IRepository repository)
    {
        _userService = userService;
        _repository = repository;
    }
    
    public async Task<FindMessagesResponse> Handle(FindMessagesCommand command, CancellationToken cancellationToken)
    {
        var messages = await _repository.Entity<Message>()
            .Select(m => new
            {
                m.Id,
                m.ChatRoomId,
                m.Text,
                m.Date,
                m.SenderId,
                Sender = _repository.Entity<Domain.Entities.User>()
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Avatar
                    })
                    .FirstOrDefault(u => u.Id.ToString() == m.SenderId)
            })
            .Where(m => m.ChatRoomId == command.ChatRoomId && m.Text.ToLower().Contains(command.Text.ToLower()))
            .ToListAsync(cancellationToken);
        
        var messageList = new List<MessageModel>();

        foreach (var message in messages)
            messageList.Add(new MessageModel(message.Id, message.Text, message.Date, message.SenderId, $"{message.Sender.FirstName} {message.Sender.LastName}", message.Sender.Avatar));

        return new FindMessagesResponse(messageList);
    }
}