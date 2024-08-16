using MediatR;
using Microsoft.AspNetCore.Identity;
using IoChatServer.Domain.Models.Authorization;
using IoChatServer.Services.User;

using SignInResult = IoChatServer.Services.User.SignInResult;

namespace IoChatServer.Application.Commands.Authenticate.AuthenticateCommand;

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, SignInResult>
{
    private UserManager<Domain.Entities.User> _userManager;
    private IUserService _userService;
    
    public AuthenticateCommandHandler(UserManager<Domain.Entities.User> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }
    
    public async Task<SignInResult> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var response = await _userService.Authenticate(new AuthenticateRequest(command.UserName, command.Password))!;
        if (response.Succeeded)
        {
            var user = await _userManager.FindByIdAsync(response.User.Id.ToString());
            return SignInResult.Success(response.AccessToken, user);
        }
        
        return SignInResult.Failure;
    }
}