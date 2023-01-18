using Serilog.Events;
using Serilog.Formatting;

namespace CMS.Service.Logging;

public class CustomTextFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        if(logEvent.Level.ToString() != "Information")
        {
            output.WriteLine("\n---#####---");
            output.WriteLine($"Timestamp = {logEvent.Timestamp} | Level - {logEvent.Level}");
            output.WriteLine("---#####---");

            foreach (var item in logEvent.Properties)
                output.WriteLine(item.Key + ": " + item.Value);

            if(logEvent.Exception != null)
            {
                output.WriteLine("---## Exception Details ##---");
                output.WriteLine("Exception - {0}", logEvent.Exception);
                output.WriteLine("StackTrace - {0}", logEvent.Exception.StackTrace);
                output.WriteLine("Message - {0}", logEvent.Exception.Message);
                output.WriteLine("Source - {0}", logEvent.Exception.Source);
                output.WriteLine("InnerException - {0}", logEvent.Exception.InnerException);
            }
            output.WriteLine("---#####---");
        }
    }
}
