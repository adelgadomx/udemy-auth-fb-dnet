using Microsoft.AspNetCore.Mvc;
using NetFirebase.Api.Dtos.UsuarioRegister;
using NetFirebase.Api.Services.Authentication;

namespace NetFirebase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
 
public class UsuarioController : ControllerBase 
{
    private readonly IAuthenticationService _authenticationService;

    public UsuarioController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register (
        [FromBody] UsuarioRegisterRequestDto request
    )
    {
        return await _authenticationService.RegisterAsync(request);
    }
}
    