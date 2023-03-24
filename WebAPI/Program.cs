using BusinessObject.Models;
using WebAPI.Auth.Services;

var builder = WebApplication.CreateBuilder(args);

// Support both gRPC services and controllers
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AssignmentPRNContext>();
builder.Services.AddTransient<IUserContextService, UserContextService>();

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

