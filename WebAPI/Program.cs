using BusinessObject.Models;
using WebAPI.Auth.Services;
using WebAPI.AutoMapper.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Support both gRPC services and controllers
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AssignmentPRNContext>();
builder.Services.AddTransient<IUserContextService, UserContextService>();
builder.Services.AddAutoMapper(typeof(ResourceProfile));

// Swagger stuffs
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

