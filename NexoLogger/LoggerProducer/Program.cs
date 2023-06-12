using NexoLogger.Loggers.Builder.Hosted;
using NexoLogger.Loggers.ConsoleLogger;
using NexoLogger.Loggers.FileLogger;
using NexoLogger.Loggers.StreamLogger;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .UseNexoLogger((nexobuilder) => {
        nexobuilder
    .Add(new ConsoleLoggerConfig(){MinLogLevel = NexoLogger.Models.LogLevels.Info})
    .Add(new FileLoggerConfig() { MinLogLevel = NexoLogger.Models.LogLevels.None})
    .Add(new StreamLoggerConfig() { MinLogLevel = NexoLogger.Models.LogLevels.None})
    ;
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
