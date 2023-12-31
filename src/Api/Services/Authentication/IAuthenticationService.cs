using NetFirebase.Api.Dtos.UsuarioRegister;

namespace NetFirebase.Api.Services.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(UsuarioRegisterRequestDto request);
}