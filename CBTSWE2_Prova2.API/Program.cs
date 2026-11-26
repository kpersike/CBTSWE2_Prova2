/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using CBTSWE2_Prova2.API.Data;
using CBTSWE2_Prova2.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")
    )
);

// Hash de senhas
builder.Services.AddScoped<IPasswordHasher<Usuario>,
    PasswordHasher<Usuario>>();

// Configurações JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "Chave JWT não configurada.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// Autenticação
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var idClaim = context.Principal?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(idClaim, out int usuarioId))
                {
                    context.Fail("Usuário inválido.");
                    return;
                }

                var dbContext = context.HttpContext
                    .RequestServices
                    .GetRequiredService<AppDbContext>();

                var usuarioAtivo = await dbContext.Usuarios
                    .AnyAsync(u => u.Id == usuarioId &&
                                   u.Status);

                if (!usuarioAtivo)
                {
                    context.Fail(
                        "Usuário inexistente ou inativo.");
                }
            }
        };
    });

var idAdministrador = builder.Configuration
    .GetValue<int>("Administracao:IdAdministrador");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SomenteAdministrador", policy =>
    {
        policy.RequireAuthenticatedUser();

        policy.RequireAssertion(context =>
            context.User.FindFirstValue(
                ClaimTypes.NameIdentifier
            ) == idAdministrador.ToString()
        );
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT obtido no login."
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();