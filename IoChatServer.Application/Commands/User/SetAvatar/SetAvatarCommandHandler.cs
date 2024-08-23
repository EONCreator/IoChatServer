using IoChatServer.Application.Commands.User.CreateUserCommand.User;
using IoChatServer.Domain.Models;
using IoChatServer.Domain.Models.Authorization;
using IoChatServer.Domain.Repositories;
using IoChatServer.Helpers.Errors;
using IoChatServer.Services.Images;
using IoChatServer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SignInResult = IoChatServer.Services.User.SignInResult;

namespace IoChatServer.Application.Commands.User.CreateUserCommand;

public class SetAvatarCommandHandler : IRequestHandler<SetAvatarCommand, SucceededResult>
{
    private IRepository _repository;
    private IUserService _userService;
    private IIMageService _imageService;
    
    public SetAvatarCommandHandler(
        IRepository repository, 
        IUserService userService,
        IIMageService imageService)
    {
        _repository = repository;
        _userService = userService;
        _imageService = imageService;
    }
    
    public async Task<SucceededResult> Handle(SetAvatarCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();

        var user = _repository.Entity<Domain.Entities.User>()
            .FirstOrDefault(u => u.Id.ToString() == userId);

        if (user == null)
            return SetAvatarOutput.Failure(UserErrors.NotFound);

        string imageName = await _imageService.ConvertFromBase64(command.Base64EncodedImage);
        user.SetAvatar(imageName);

        await _repository.SaveChanges();
        
        return SetAvatarOutput.Succeeded(imageName);
    }
}