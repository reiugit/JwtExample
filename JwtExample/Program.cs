using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtExample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BearerTokenGenerator>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //Authority, Audience omitted for brevity
        var secret = "SimpleExampleKey1234567890123456"u8.ToArray(); //hardcoded for brevity

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret)
        };

    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (BearerTokenGenerator bearerTokenGenerator)
    => new { Info = "Use 'Tests.http' file to generate requests for login and token-authorization." });

app.MapPost("/login", ([FromBody] LoginRequest request, BearerTokenGenerator bearerTokenGenerator)
    => new // password not checked for brevity
    {
        Token = $"Bearer {bearerTokenGenerator.GenerateToken(request.UserName, request.Email)}"
    });

app.MapGet("/check-bearer-token", () => Results.Ok("Token accepted."))
   .RequireAuthorization();

app.Run();
