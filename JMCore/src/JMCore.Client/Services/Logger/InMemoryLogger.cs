using Microsoft.Extensions.Logging;

namespace JMCore.Client.Services.Logger;

public class InMemoryLogger(string name) : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}] {name} - {formatter(state, exception)}");
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

}