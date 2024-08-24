using JWT_AuthAndRefrest.Context;
using JWT_AuthAndRefrest.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standar Authorization header using the bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

var key = builder.Configuration.GetValue<string>("Jwt:key");
//var keyBytes = Encoding.UTF8.GetBytes(key);

//var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{ 
    config.RequireHttpsMetadata = false; //lo que significa que no se requiere una conexión HTTPS para la validación del token. Esto podría ser útil durante el desarrollo,
                                         //pero en un entorno de producción, generalmente debería ser true para garantizar la seguridad de la transmisión de tokens.
    config.SaveToken = true; //el token JWT recibido se almacenará en el HttpContext
    config.TokenValidationParameters = new TokenValidationParameters //Aquí defines los parámetros de validación del token JWT
    {
        ValidateIssuerSigningKey = true, //lo que significa que se verificará la firma del token.
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes), // Aquí proporcionas una clave simétrica 
        ValidateIssuer = false, //significa que no se validará el emisor (issuer) del token. En un entorno de producción, generalmente configurarías esto para validar el emisor.
        ValidateAudience = false, // no se validará la audiencia del token.
        ValidateLifetime = true, // se validará el tiempo de vida (expiración) del token.
        ClockSkew = TimeSpan.Zero //no se tolerará ninguna diferencia entre el tiempo en el token y el tiempo actual.
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
