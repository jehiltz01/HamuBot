using HamuBot.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.ListenAnyIP(5000); // This allows outside connections
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
var app = builder.Build();

// Instantiate BotService once — reuse for all endpoints
var bot = new BotService();

app.MapGet("/", () => "Bot Control API");

app.MapGet("/status", () => bot.IsRunning ? "Running" : "Stopped");

app.MapPost("/start", async () => {
    await bot.StartAsync(false);
    Console.WriteLine("Starting...");
    return Results.Ok("Bot started.");
});

app.MapPost("/startQuietly", async () => {
    await bot.StartAsync(true);
    Console.WriteLine("StartingQuietly...");
    return Results.Ok("Bot started quietly.");
});

app.MapPost("/stop", async () => {
    await bot.StopAsync();
    Console.WriteLine("Stopping...");
    return Results.Ok("Bot stopped.");
});

app.Run("http://0.0.0.0:5000");
