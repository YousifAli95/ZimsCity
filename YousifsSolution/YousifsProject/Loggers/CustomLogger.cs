using System.Text;


namespace YousifsProject.Loggers
{
    public class CustomLogger
    {
        private readonly ILogger<CustomLogger> _logger;
        private readonly TimeZoneInfo _swedenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm");

        public CustomLogger(ILogger<CustomLogger> logger)
        {
            _logger = logger;
        }

        public void LogArguments(LogLevel logLevel, string methodName, object arguments)
        {
            var formattedArguments = FormatArguments(arguments);
            var timestamp = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, _swedenTimeZone).DateTime;

            _logger.Log(logLevel, "\n{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} Method: {MethodName} arguments:\n" +
                                  "--------------------------------------\n" +
                                  "{FormattedArguments}" +
                                  "--------------------------------------",
                                  timestamp, methodName, formattedArguments);
        }
        private string FormatArguments(object arguments)
        {
            var formattedArguments = new StringBuilder();

            foreach (var property in arguments.GetType().GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(arguments);
                formattedArguments.Append($"{propertyName}: {propertyValue}\n");
            }

            return formattedArguments.ToString();
        }

    }


}
