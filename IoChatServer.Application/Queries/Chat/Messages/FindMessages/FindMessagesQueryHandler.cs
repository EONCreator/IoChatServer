using MediatR;
using IoChatServer.Helpers.Errors;
using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Queries.Chat.Messages.FindMessages;

public class FindMessagesQueryHandler : IRequestHandler<FindMessagesQuery, FindMessagesOutput>
{
    private IChatService _chatService;
    
    public FindMessagesQueryHandler(
        IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<FindMessagesOutput> Handle(FindMessagesQuery command, CancellationToken cancellationToken)
    {
        var userInChatRoom = await _chatService.UserInChatRoom(command.ChatRoomId);
        if (!userInChatRoom)
            return FindMessagesOutput.Failure(ChatErrors.UserNotInChatRoom);
        
        var messages = await _chatService.GetMessagesOfChatRoom(
            command.ChatRoomId, 
            m => m.Text.ToLower().Contains(command.Text.ToLower()));
        
        var messageList = new List<MessageClientModel>();

        foreach (var message in messages)
            messageList.Add(new MessageClientModel(
                message.Id, 
                message.Text, 
                message.Date, 
                message.SenderId, 
                $"{message.Sender.FirstName} {message.Sender.LastName}", 
                message.Sender.Avatar));

        return FindMessagesOutput.Success(messageList);
    }
}