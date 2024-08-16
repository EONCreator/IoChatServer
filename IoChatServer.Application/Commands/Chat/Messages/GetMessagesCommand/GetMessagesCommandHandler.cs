using IoChatServer.Application.Commands.User.CreateUserCommand;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Models.Authorization;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;

public class GetMessagesCommandHandler : IRequestHandler<GetMessagesCommand, GetMessagesResponse>
{
    private UserManager<Domain.Entities.User> _userManager;
    private IUserService _userService;
    private IRepository _repository;
    
    public GetMessagesCommandHandler(
        UserManager<Domain.Entities.User> userManager, 
        IUserService userService,
        IRepository repository)
    {
        _userManager = userManager;
        _userService = userService;
        _repository = repository;
    }
    
    public async Task<GetMessagesResponse> Handle(GetMessagesCommand command, CancellationToken cancellationToken)
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
            .Where(m => m.ChatRoomId == command.ChatRoomId)
            .ToListAsync();
        
        var messageList = new List<MessageModel>();

        foreach (var message in messages)
        {
            messageList.Add(new MessageModel(message.Id, message.Text, message.Date, message.SenderId, $"{message.Sender.FirstName} {message.Sender.LastName}", message.Sender.Avatar));
        }

        return new GetMessagesResponse(messageList);
    }
}