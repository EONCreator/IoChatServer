using IoChatServer.Domain.Models;

namespace IoChatServer.Application.Commands.User.CreateUserCommand.User;

public class SetAvatarOutput : SucceededResult
{
    public string Avatar { get; set; }

    public static SetAvatarOutput Succeeded(string avatar) => new(true, avatar);
    public static SetAvatarOutput Failure => new(false, null);

    public SetAvatarOutput(bool succeeded, string avatar, string? error = null)
        : base(succeeded, error)
    {
        Avatar = avatar;
    }
}
