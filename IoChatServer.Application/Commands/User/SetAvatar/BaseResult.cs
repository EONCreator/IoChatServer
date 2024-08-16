namespace IoChatServer.Application.Commands.User.CreateUserCommand.User;

public class BaseResult
{
    public bool Success { get; private set; }

    public static BaseResult Succeeded => new(true);
    public static BaseResult Failure => new(false);

    public BaseResult(bool success)
    {
        Success = success;
    }
}

public class SetAvatarResponse
{
    public string Avatar { get; }

    public SetAvatarResponse(string avatar)
    {
        Avatar = avatar;
    }
}