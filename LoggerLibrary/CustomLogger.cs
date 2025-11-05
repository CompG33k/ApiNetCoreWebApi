using Microsoft.Extensions.Logging;

namespace LoggerLibrary
{
    public class CustomLogger
    {
        private readonly ILogger _logger;

        public CustomLogger(ILogger logger)
        {
            _logger = logger;
        }

        // Define an Action delegate for logging an informational message
        public Action<string> LogInfoAction => (message) => _logger.LogInformation(message);

        // Define an Action delegate for logging a warning message with an event ID
        public Action<EventId, string> LogWarningWithEventIdAction => (eventId, message) => _logger.LogWarning(eventId, message);

        // Define a method that takes an Action delegate as a parameter for flexible logging
        public void ExecuteAndLog(Action actionToExecute, Action<string> logSuccess, Action<Exception, string> logError)
        {
            try
            {
                actionToExecute();
                logSuccess("Operation completed successfully.");
            }
            catch (Exception ex)
            {
                logError(ex, "An error occurred during the operation.");
            }
        }

        // Example of a specific logging method within the class
        public void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }
}
