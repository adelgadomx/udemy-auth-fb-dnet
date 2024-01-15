
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient? _httpClient;
    private readonly AuthenticationStateProvider? _authenticationStateProvider;
    private readonly ILocalStorageService? _localStorageService;

    public AuthService(HttpClient? httpClient, AuthenticationStateProvider? authenticationStateProvider, ILocalStorageService? localStorageService)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorageService = localStorageService;
    }

    public async Task<string> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient!.PostAsync("api/usuario/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

        if(!response.IsSuccessStatusCode)
        {
            return null!;
        }

        var loginResult = await response.Content.ReadAsStringAsync();
        await _localStorageService!.SetItemAsStringAsync("authToken", loginResult);
        ((apiAthenticationStateProvider)_authenticationStateProvider!).MarkUserAsAuthenticated(loginModel.Email!);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult);

        return loginResult;

    }

    public async Task Logout()
    {
        await _localStorageService!.RemoveItemAsync("authToken");
        ((apiAthenticationStateProvider)_authenticationStateProvider!).MarkUserAsLoggedOut();
        _httpClient!.DefaultRequestHeaders.Authorization = null;
        
    }

    public async Task<string> Register(RegisterModel registerModel)
    {
        var registerAsJson = JsonSerializer.Serialize(registerModel);
        var response = await _httpClient!.PostAsync("api/usuario/register", new StringContent(registerAsJson, Encoding.UTF8, "application/json"));
        if(!response.IsSuccessStatusCode)
        {
            return null!;
        }

        var registerResult = await response.Content.ReadAsStringAsync();
        return registerResult;
    }
}