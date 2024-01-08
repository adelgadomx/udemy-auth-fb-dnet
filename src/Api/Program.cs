using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using NetFirebase.Api.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NetFirebase.Api.Data;
using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Services.Productos;
using NetFirebase.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Postgres DbContext
var connectionString = builder.Configuration.GetConnectionString("PgDatabase");
builder.Services.AddDbContext<DatabaseContext>(options => {
    options.UseNpgsql(connectionString);
});
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

// Databse sqlite - replaced by pg databse
/* builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.LogTo(Console.WriteLine, new[] {
            DbLoggerCategory.Database.Command.Name
        },
        LogLevel.Information
    ).EnableSensitiveDataLogging();
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqlDatabase"));
});
 */
 
 // Add models to Injection repository
 builder.Services.AddScoped<IProductoService, ProductoService>();

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

app.AddDataProductos();

app.Run();
