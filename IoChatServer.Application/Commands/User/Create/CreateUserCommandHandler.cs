using IoChatServer.Application.Commands.User.CreateUserCommand.User;
using IoChatServer.Domain.Models.Authorization;
using IoChatServer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SignInResult = IoChatServer.Services.User.SignInResult;

namespace IoChatServer.Application.Commands.User.CreateUserCommand;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, SignInResult>
{
    private UserManager<Domain.Entities.User> _userManager;
    private IUserService _userService;
    
    public CreateUserCommandHandler(UserManager<Domain.Entities.User> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }
    
    public async Task<SignInResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        Domain.Entities.User user = new Domain.Entities.User();
        user.UserName = command.UserName;
        user.SetFirstName(command.FirstName);
        user.SetLastName(command.LastName);
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            var response = await _userService.Authenticate(new AuthenticateRequest(command.UserName, command.Password));
            if (response.Succeeded)
            {
                var responseUser = await _userManager.FindByIdAsync(response.User.Id.ToString());
                return SignInResult.Success(response.AccessToken, responseUser);
            }
        }
        
        return SignInResult.Failure;
    }
}