// System namespaces
using System.Text;
using System.Reflection;

// Microsoft namespaces
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MediatR;
using FluentValidation;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Authentication.Commands.Login;
using Application.Features.Authentication.Commands.Register;
using Application.IServices;
using Application.IRepositories;
using Application.Validators.Commands;
using Application.Validators.DTOs;
using Application.Behaviors;
using Application.MappingProfles;
using Infrastrucure.DbContexts;
using Infrastrucure.Services;
using Infrastrucure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var envConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.env.json", optional: false, reloadOnChange: true)
    .Build();
builder.Configuration.AddConfiguration(envConfig);

builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", 
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, Array.Empty<string>()}
    });
});
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ,
        ValidAudience = builder.Configuration["Jwt:Audience"] ,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Client"));
});

builder.Services.AddControllers();


builder.Services.AddDbContext<UserManagmentDbContext>(options =>
    options.UseNpgsql($"Host={builder.Configuration["Database:Host"]};" +
                     $"Port={builder.Configuration["Database:Port"]};" +
                     $"Database={builder.Configuration["Database:Name"]};" +
                     $"Username={builder.Configuration["Database:Username"]};" +
                     $"Password={builder.Configuration["Database:Password"]}")    
);


// builder.Services.AddDbContext<UserManagmentDbContext>(options =>
//     options.UseSqlite("Data Source=myDataBase.db"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddMediatR(cfg => {
    // Register WebApi assembly
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    
    // Register Application layer assemblies
    cfg.RegisterServicesFromAssembly(typeof(DeleteUserCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllUsersQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
    
    // Register Infrastructure layer assembly
    cfg.RegisterServicesFromAssembly(typeof(UserService).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(AuthenticationService).Assembly);
});

// Add validation behavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Register all validators
builder.Services.AddValidatorsFromAssemblyContaining<LoginUserDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteUserCommandValidator>();

builder.Services.AddAutoMapper(typeof(UserProfile));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
