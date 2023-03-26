using BusinessObject.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using WebAPI.AutoMapper.Profiles;
using WebAPI.Services;

// Firebase config
var credential = GoogleCredential.FromFile("firebasekey.json");
var firebaseApp = FirebaseApp.Create(new AppOptions
{
    Credential = credential
});

var builder = WebApplication.CreateBuilder(args);

// Support both gRPC services and controllers
builder.Services.AddControllers();

// Database context
builder.Services.AddScoped<AssignmentPRNContext>();

// Server services DI config
builder.Services.AddScoped<IUserService, UserService>();

// Firebase
builder.Services.AddSingleton<StorageClient>(StorageClient.Create(credential));

// Auto mapper config
builder.Services.AddAutoMapper(typeof(ResourceProfile));
builder.Services.AddAutoMapper(typeof(AuthProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(ClassProfile));

// Swagger stuffs
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(opts =>
{
    opts.AllowAnyHeader();
    opts.AllowAnyMethod();
    opts.AllowAnyOrigin();
});

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

