using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace AuditLogging
{
    public class AuditLogger
    {
        public Dictionary<string, object> Properties { get; }
        public bool IsScoped { get; set; }
        public ILogger Logger { get; }

        public AuditLogger(ILogger logger)
        {
            Properties = new Dictionary<string, object>();
            IsScoped = false;
            Logger = logger;
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
        public void LogInformation(EventId eventId, Exception exception, string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, eventId, exception, message, args));
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
        public void LogInformation(EventId eventId, string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, eventId, message, args));
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
        public void LogInformation(Exception exception, string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, exception, message, args));
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
        public void LogInformation(string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, message, args));
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
        public void LogError(EventId eventId, Exception exception, string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, eventId, exception, message, args));
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="eventId">The event id associated with the log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
        public void LogError(EventId eventId, string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, eventId, message, args));
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
        public void LogError(Exception exception, string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, exception, message, args));
        }

        /// <summary>
        /// Formats and writes an audit trail log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
        public void LogError(string message, params object[] args)
        {
            Log(() => Logger.Log(LogLevel.Error, message, args));
        }

        private void Log(Action logAction)
        {
            // Create scope
            var scope = IsScoped ? Logger.BeginScope("clinia-audit-trail") : null;
            
            // Add properties to context
            ICollection<IDisposable> logContexts = new List<IDisposable>();
            foreach (var property in Properties)
            {
                var logContext = LogContext.PushProperty(property.Key, property.Value);
                logContexts.Add(logContext);
            }

            try
            {
                logAction.Invoke();
            }
            finally
            {
                // Dispose
                scope?.Dispose();
                foreach (var logContext in logContexts)
                {
                    logContext.Dispose();
                }   
            }
        }
    }
}