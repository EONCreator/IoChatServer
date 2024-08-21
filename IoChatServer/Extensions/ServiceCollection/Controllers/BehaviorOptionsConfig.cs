using Microsoft.AspNetCore.Mvc;

namespace IoChatServer.Extensions.Controllers;

public static class ApiBehaviorOptionsConfig
{
    public static readonly Action<ApiBehaviorOptions> Options = options =>
    {
        options.SuppressConsumesConstraintForFormFileParameters = true;
        options.SuppressInferBindingSourcesForParameters = true;
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
        options.ClientErrorMapping[StatusCodes.Status404NotFound].Link =
            "https://httpstatuses.com/404";
    };
}