using System.Text;
using DotNetEnv;
using Backend.Context;
using Backend.Model;
using Backend.Repository;
using Backend.Repository.Auth;
using Backend.Repository.CategoryRepository;
using Backend.Repository.InventoryRepository;
using Backend.Repository.UserRepository;
using Backend.Services;
using Backend.Services.Authentication;
using Backend.Services.InventoryService;
using Backend.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

//DI Repositories
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<IManageUserRepository, ManageUserRepository>();
builder.Services.AddScoped<InventoryRepository>();
builder.Services.AddScoped<CategoryRepository>();
//DI Services
builder.Services.AddScoped<ManageUserService>();
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<InventoryServices>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//database config
var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "MyDatabase";
var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "password";

var connectionString = $"Host={host};Port={port};Database={dbName};Username={user};Password={pass};Pooling=true;";

//Db connection
builder.Services.AddDbContext<MyDbContext>(options => 
    options.UseNpgsql(connectionString));

// Register Identity
builder.Services.AddIdentity<Users, IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.Run();


