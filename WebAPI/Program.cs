var builder = WebApplication.CreateBuilder(args);

// Support both gRPC services and controllers
builder.Services.AddControllers();

// Swagger stuffs
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

