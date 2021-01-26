using Clinia.Extensions.Logging.Audit;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static AuditLoggerBuilder WithAudit(this ILogger logger)
        {
            return new AuditLoggerBuilder(logger).WithAudit();
        }
    }
}