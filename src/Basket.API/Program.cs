var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();
builder.Services.AddAuthorization(); // Add this line

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseAuthentication(); // Add this line
app.UseAuthorization();  // Add this line

app.MapGrpcService<BasketService>();

app.Run();
