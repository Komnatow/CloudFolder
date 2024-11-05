using CloudFolder.Models;
using Microsoft.Extensions.Options;

namespace CloudFolder.Procedures;

public class RoleMiddleware
{
    private readonly IOptions<GlobalVariablesOptions> _global;
    private readonly RequestDelegate _next;

    public RoleMiddleware(RequestDelegate next, IOptions<GlobalVariablesOptions> global)
    {
        _next = next;
        _global = global;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var global = _global.Value;
        if(global.role.Length > 0 || global.role != null)
        {
            context.Request.Headers.Add("role", global.role);
        }
        else
        {
            context.Request.Headers.Add("role", "");
        }
        await _next(context);
    }
}
