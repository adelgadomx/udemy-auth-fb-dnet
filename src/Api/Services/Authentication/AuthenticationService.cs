
using FirebaseAdmin.Auth;
using NetFirebase.Api.Dtos.UsuarioRegister;

namespace NetFirebase.Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    public async Task<string> RegisterAsync(UsuarioRegisterRequestDto request)
    {
        var userArgs = new UserRecordArgs {
            Email = request.Email,
            Password = request.Password
        };

        var usuario = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

        return usuario.Uid;
    }
}
