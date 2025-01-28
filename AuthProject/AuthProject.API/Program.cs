using System.Reflection;
using AuthProject.Core.Configurations;
using AuthProject.Core.Entities;
using AuthProject.Core.Repositories;
using AuthProject.Core.Services;
using AuthProject.Core.UnitOfWork;
using AuthProject.Data;
using AuthProject.Data.Contexts;
using AuthProject.Data.Repositories;
using AuthProject.Data.UnitOfWork;
using AuthProject.Service.Services;
using AuthProject.Shared.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using AuthProject.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));


#region Connection String Configuration
var connectionString = builder.Configuration.GetSection(ConnectionStringOption.ConnectionStrings).Get<ConnectionStringOption>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString!.SqlServer, sqlServerOptionsAction =>
    {
        sqlServerOptionsAction.MigrationsAssembly(typeof(DataLayerAssembly).Assembly.FullName);
    });
});
#endregion

#region Services Configuration
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


#endregion

#region Identity Configurations
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;


})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
#endregion

#region Authentication Configurations
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();


        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = tokenOptions!.Issuer,
            ValidAudience = tokenOptions.Audiences[0],
            IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });
#endregion


#region Fluent Validation Configurations
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
#endregion


#region Extension Services
builder.Services.CustomValidationResponse();
#endregion

// .NET Default messages closed
builder.Services.Configure<ApiBehaviorOptions>(options=>options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseCustomException();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
