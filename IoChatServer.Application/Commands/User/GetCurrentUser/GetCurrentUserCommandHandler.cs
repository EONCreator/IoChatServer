using MediatR;
using Microsoft.AspNetCore.Identity;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.User.GetCurrentUser;

public class GetCurrentUserCommandHandler : IRequestHandler<GetCurrentUserCommand, GetCurrentUserResponse>
{
    private IUserService _userService;
    
    public GetCurrentUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();
        var user = await _userService.GetById(userId);
        
        return new GetCurrentUserResponse(user);
    }
}