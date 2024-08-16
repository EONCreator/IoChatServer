using IoChatServer.Services.User;
using MediatR;

namespace IoChatServer.Application.Commands.Authenticate.AuthenticateCommand;

public class AuthenticateCommand : IRequest<SignInResult>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}