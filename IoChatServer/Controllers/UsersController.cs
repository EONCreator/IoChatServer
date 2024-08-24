using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Primitives;
using IoChatServer.Application.Commands.Authenticate.AuthenticateCommand;
using IoChatServer.Application.Commands.User.CreateUserCommand;
using IoChatServer.Application.Queries.User.GetCurrentUser;

namespace IoChatServer.Controllers;

using Microsoft.AspNetCore.Mvc;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]/[action]")]
public class UsersController : BaseController<User>
{
    private readonly IMediator _mediator;

    public UsersController(
        IRepository repository, 
        IMediator mediator) 
        : base(repository)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creating/registration of user
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.Succeeded)
            return Unauthorized("Пользователь с таким именем уже существует");
        
        return Ok(result);
    }

    /// <summary>
    /// Set avatar for user
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SetAvatar(
        [FromBody] SetAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Authenticate user
    /// </summary>
    /// <param name="command">Data for authentication</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate(
        [FromBody] AuthenticateCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.Succeeded)
            return Unauthorized("Неверное имя пользователя или пароль");
        
        return Ok(result);
    }


    /// <summary>
    /// Getting user by token
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetCurrentUserQuery(), cancellationToken));
}