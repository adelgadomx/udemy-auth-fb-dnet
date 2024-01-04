using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using NetFirebase.Api.Services.Authentication;
using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use Firebase Authentication
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("firebase.json")
});
// Register in dependency container
// builder.Services.AddSingleton<IAuthenticationService,AuthenticationService>();
builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>((sp, httpClient) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]!);
});

// Use 
builder.Services
       .AddAuthentication()
       .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, JwtOptions =>
       {
           JwtOptions.Authority = builder.Configuration["Authentication:ValidIssuer"];
           JwtOptions.Audience = builder.Configuration["Authentication:Audience"];
           JwtOptions.TokenValidationParameters.ValidIssuer = builder.Configuration["Authentication:ValidIssuer"];
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
