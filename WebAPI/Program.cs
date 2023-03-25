using BusinessObject.Models;
using WebAPI.AutoMapper.Profiles;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Support both gRPC services and controllers
builder.Services.AddControllers();

// Database context
builder.Services.AddScoped<AssignmentPRNContext>();

// Server services DI config
builder.Services.AddScoped<IUserService, UserService>();

// Auto mapper config
builder.Services.AddAutoMapper(typeof(ResourceProfile));
builder.Services.AddAutoMapper(typeof(AuthProfile));

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

