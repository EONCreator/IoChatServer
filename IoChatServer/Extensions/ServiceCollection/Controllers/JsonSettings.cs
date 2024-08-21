using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace IoChatServer.Extensions.Controllers;

public static class JsonConfig
{
    public static readonly Action<JsonOptions> Options = options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
    };
}