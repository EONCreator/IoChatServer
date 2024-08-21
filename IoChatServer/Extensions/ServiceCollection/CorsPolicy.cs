namespace IoChatServer.Extensions;

public static class CorsPolicy
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "MyPolicy",
                policy  =>
                {
                    policy.WithOrigins("http://217.171.146.249", "http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        
        return services;
    }
}