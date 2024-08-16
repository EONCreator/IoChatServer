using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Primitives;
using IoChatServer.Application.Commands.Chat.CreateChatRoom;
using IoChatServer.Application.Commands.Chat.FindUser;
using IoChatServer.Application.Commands.Chat.GetChatRooms;
using IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;
using IoChatServer.Application.Commands.Messages.SendMessageCommand;

namespace IoChatServer.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]/[action]")]
public class ChatsController : BaseController<ChatRoom>
{
    private readonly IMediator _mediator;
    
    public ChatsController(IRepository repository, IMediator mediator)
        : base(repository)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Send message to user
    /// </summary>
    /// <param name="getterId"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendMessage(
        [FromBody] Message message,
        CancellationToken cancellationToken)
    {
        return Ok(
            await _mediator.Send(
                new SendMessageCommand(message), 
                cancellationToken)
            );
    }
    
    /// <summary>
    /// Notification of writing message by user
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> WritingMessage(
        [FromQuery(Name = "chatRoomId")] int chatRoomId,
        CancellationToken cancellationToken)
    {
        return Ok(
            await _mediator.Send(
                new WritingMessageCommand(chatRoomId), 
                cancellationToken)
        );
    }
    
    /// <summary>
    /// Notification for users when user online
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> NotifyForOnline(CancellationToken cancellationToken)
        => Ok(
            await _mediator.Send(
                new NotifyForOnlineCommand(), 
                cancellationToken)
        );
    

    /// <summary>
    /// Get chat rooms by current user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetChatRooms(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetChatRoomsCommand(), cancellationToken));
    
    /// <summary>
    /// Creating chat room
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateChatRoom(
        [FromBody] CreateChatRoomCommand command,
        CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    /// <summary>
    /// Find user by nickname
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> FindUser(
        [FromQuery] string userName,
        CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new FindUserCommand(userName), cancellationToken));
    
    /// <summary>
    /// Get messages of chatroom by chatRoomId
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetMessages(
        [FromQuery] int chatRoomId,
        CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetMessagesCommand(chatRoomId), cancellationToken));


}