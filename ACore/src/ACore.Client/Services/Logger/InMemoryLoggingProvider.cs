// using System.Collections.Concurrent;
// using Microsoft.Extensions.Logging;
//
// namespace ACore.Client.Services.Logger
// {
//     public class InMemoryLoggingProvider : ILoggerProvider
//     {
//         private readonly ConcurrentDictionary<string, InMemoryLogger> _loggers = new();
//
//         public ILogger CreateLogger(string categoryName)
//         => _loggers.GetOrAdd(categoryName, name => new InMemoryLogger(name));
//
//
//         public void Dispose()
//             => _loggers.Clear();
//     }
// }
