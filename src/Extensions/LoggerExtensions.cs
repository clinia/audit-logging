using Microsoft.Extensions.Logging;

namespace AuditLogging
{
    public static class LoggerExtensions
    {
        public static AuditLoggerBuilder WithAudit(this ILogger logger)
        {
            return new AuditLoggerBuilder(logger).WithAudit();
        }
    }
}