var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// register application services
builder.Services.AddSingleton<WirtualnyPortfelAPI.Interfaces.IUserService, WirtualnyPortfelAPI.Services.UserService>();
builder.Services.AddSingleton<WirtualnyPortfelAPI.Interfaces.ISubscriptionService, WirtualnyPortfelAPI.Services.SubscriptionService>();
builder.Services.AddSingleton<WirtualnyPortfelAPI.Interfaces.INotificationService, WirtualnyPortfelAPI.Services.NotificationService>();
// Note: controllers added automatically; services already registered

// Allow MAUI mobile app to call API during prototyping (adjust for production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyPrototype", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Allow Kestrel to bind to all interfaces so phone can reach laptop via hotspot
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAnyPrototype");

app.MapControllers();

app.Run();
