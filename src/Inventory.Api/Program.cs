using FluentValidation;
using Inventory.Api.Middlewares;
using Inventory.Application.Commands;
using Inventory.Application.Interfaces;
using Inventory.Application.Queries;
using Inventory.Application.Validators;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

builder.Services.AddDbContext<InventoryReadDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDbConnection>(_ =>
    new SqlConnection(connectionString));

builder.Services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
builder.Services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();
builder.Services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
builder.Services.AddScoped<IInventoryMovementWriteRepository, InventoryMovementWriteRepository>();

builder.Services.AddScoped<GetProductsQueryHandler>();
builder.Services.AddScoped<GetProductByIdQueryHandler>();
builder.Services.AddScoped<CreateCategoryCommandHandler>();
builder.Services.AddScoped<UpdateCategoryCommandHandler>();
builder.Services.AddScoped<DeleteCategoryCommandHandler>();
builder.Services.AddScoped<GetCategoriesQueryHandler>();
builder.Services.AddScoped<GetCategoryByIdQueryHandler>();
builder.Services.AddScoped<CreateProductCommandHandler>();
builder.Services.AddScoped<UpdateProductCommandHandler>();
builder.Services.AddScoped<DeleteProductCommandHandler>();
builder.Services.AddScoped<RegisterInventoryMovementCommandHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteCategoryCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteProductCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterInventoryMovementCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCommandValidator>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();