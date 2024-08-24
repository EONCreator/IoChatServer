using MediatR;
using IoChatServer.Helpers.Errors;
using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Queries.Chat.Messages.GetMessages;

public class GetMessagesCommandHandler : IRequestHandler<GetMessagesQuery, GetMessagesOutput>
{
    private IChatService _chatService;
    
    public GetMessagesCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<GetMessagesOutput> Handle(GetMessagesQuery command, CancellationToken cancellationToken)
    {
        var userInChatRoom = await _chatService.UserInChatRoom(command.ChatRoomId);
        if (!userInChatRoom)
            return GetMessagesOutput.Failure(ChatErrors.UserNotInChatRoom);
        
        var messages = await _chatService.GetMessagesOfChatRoom(command.ChatRoomId);
        
        var messageList = new List<MessageClientModel>();

        foreach (var message in messages)
            messageList.Add(new MessageClientModel(
                message.Id, 
                message.Text, 
                message.Date, 
                message.SenderId, 
                $"{message.Sender.FirstName} {message.Sender.LastName}", 
                message.Sender.Avatar));
        
        return GetMessagesOutput.Success(messageList);
    }
}