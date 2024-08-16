using MediatR;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Authenticate.AuthenticateCommand;

public class AuthenticateCommand : IRequest<SignInResult>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}