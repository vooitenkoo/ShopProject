using System.Reflection;
using System.Text;
using Application.Behaviors;
using Application.IManagers;
using Application.IService;
using Application.MappingProfiles;
using Application.Products.Commands;
using Application.Products.Queries;
using Application.Products.Validators;
using Application.Tags.Commands;
using Application.Tags.Queries;
using Application.Tags.Validators;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.DbContexts;
using Infrastructure.Managers;
using Infrastructure.Service;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Middleware;

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

builder.Services.AddDbContext<ProductManagmentServiceDbContext>(options =>
    options.UseNpgsql($"Host={builder.Configuration["Database:Host"]};" +
                     $"Port={builder.Configuration["Database:Port"]};" +
                     $"Database={builder.Configuration["Database:Name"]};" +
                     $"Username={builder.Configuration["Database:Username"]};" +
                     $"Password={builder.Configuration["Database:Password"]}")    
);

// builder.Services.AddDbContext<ProductManagmentServiceDbContext>(options =>
//     options.UseSqlite("Data Source=myDataBase.db"));
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Client"));
});

// Add AutoMapper
builder.Services.AddAutoMapper(
    typeof(ProductProfile),
    typeof(TagProfile)
);

// Add MediatR with Validation Behavior
builder.Services.AddMediatR(cfg => {
    // Register all commands and queries from the Application assembly
    var applicationAssembly = typeof(CreateProductCommand).Assembly;
    cfg.RegisterServicesFromAssembly(applicationAssembly);

    // Add validation behavior
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateTagCommandValidator).Assembly);

// Add Services
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ValidationExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
