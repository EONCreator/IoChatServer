using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using IoChatServer.Domain.Models.Authorization;
using IoChatServer.Services.Authorization;

namespace IoChatServer.Services.User;

using Domain.Entities;

public interface IUserService
{
    Task<SignInResult>? Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    Task<User>? GetById(string id);
    Task<string>? GetCurrentUserId();
    Task<User>? GetCurrentUser();
}

public class SignInResult
{
    public bool Succeeded { get; }
    public string? AccessToken { get; }
    public User? User { get; }
    
    public static SignInResult Failure => new(false, null, null);
    public static SignInResult Success(string accessToken, User user) => new(true, accessToken, user);
    
    public SignInResult(bool succeeded, string? accessToken, User user)
    {
        Succeeded = succeeded;
        AccessToken = accessToken;
        User = user;
    }
}

public class UserService : IUserService
{
    private readonly IJwtUtils _jwtUtils;
    private readonly UserManager<User> _userManager;
    private IUserService _userServiceImplementation;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserManager<User> userManager, 
        IJwtUtils jwtUtils,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _jwtUtils = jwtUtils;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SignInResult>? Authenticate(AuthenticateRequest model)
    {
        var user = _userManager.Users.SingleOrDefault(x => x.UserName == model.UserName);
        bool passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordCorrect)
            return SignInResult.Failure;
        
        // return null if user not found
        if (user == null)
            return SignInResult.Failure;

        // authentication successful so generate jwt token
        var token = _jwtUtils.GenerateJwtToken(user);

        return SignInResult.Success(token, user);
    }

    public IEnumerable<User> GetAll()
    {
        return _userManager.Users.ToList();
    }

    public async Task<User>? GetById(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<string>? GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            throw new Exception("GetCurrentUserId(): HttpContext is null");

        var user = httpContext.User;
        if (user == null)
            throw new Exception("GetCurrentUserId(): user is null");
        
        var id = user.Claims.ToList().Find(r => r.Type == "id").Value;

        return await Task.FromResult(id);
    }

    public async Task<User>? GetCurrentUser()
    {
        var userId = await GetCurrentUserId();
        var user = await GetById(userId);

        return user;
    }
}