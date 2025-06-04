using eShop.WebApp.Components;
using eShop.ServiceDefaults;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add rate limiting policy
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", o =>
    {
        o.Window = TimeSpan.FromSeconds(10);
        o.PermitLimit = 5; // Allow 5 requests per 10 seconds per IP
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        o.QueueLimit = 2;
    });
    options.RejectionStatusCode = 429;
});

builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddApplicationServices();

var app = builder.Build();

app.UseRateLimiter();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAntiforgery();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapForwarder("/product-images/{id}", "http://catalog-api", "/api/catalog/items/{id}/pic");

app.Run();
