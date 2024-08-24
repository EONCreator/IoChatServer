using IoChatServer.Helpers.Errors;
using MediatR;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Queries.User.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserOutput>
{
    private IUserService _userService;
    
    public GetCurrentUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<GetCurrentUserOutput> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userService.GetCurrentUser();
        if (user == null)
            return GetCurrentUserOutput.Failure(UserErrors.NotFound);
        
        return GetCurrentUserOutput.Succeeded(user);
    }
}