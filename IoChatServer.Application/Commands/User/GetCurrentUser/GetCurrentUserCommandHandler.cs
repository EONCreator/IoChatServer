using IoChatServer.Domain.Models.Authorization;
using IoChatServer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IoChatServer.Application.Commands.User.GetCurrentUser;

public class GetCurrentUserCommandHandler : IRequestHandler<GetCurrentUserCommand, GetCurrentUserResponse>
{
    private UserManager<Domain.Entities.User> _userManager;
    private IUserService _userService;
    
    public GetCurrentUserCommandHandler(UserManager<Domain.Entities.User> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }
    
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);
        
        return new GetCurrentUserResponse(user);
    }
}