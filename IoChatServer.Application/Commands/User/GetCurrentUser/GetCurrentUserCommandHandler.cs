using IoChatServer.Helpers.Errors;
using MediatR;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.User.GetCurrentUser;

public class GetCurrentUserCommandHandler : IRequestHandler<GetCurrentUserCommand, GetCurrentUserOutput>
{
    private IUserService _userService;
    
    public GetCurrentUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<GetCurrentUserOutput> Handle(GetCurrentUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.GetCurrentUser();
        if (user == null)
            return GetCurrentUserOutput.Failure(UserErrors.NotFound);
        
        return GetCurrentUserOutput.Succeeded(user);
    }
}