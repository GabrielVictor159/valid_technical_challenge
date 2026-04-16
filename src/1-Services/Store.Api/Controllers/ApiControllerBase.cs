using Microsoft.AspNetCore.Mvc;
using Store.Api.Extensions;
using Store.Application.Abstractions.DTOs;

namespace Store.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected UserSession? CurrentUser => User.Identity?.IsAuthenticated == true
    ? User.GetUserSession()
    : null;
}
