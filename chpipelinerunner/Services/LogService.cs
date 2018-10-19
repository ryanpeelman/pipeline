using System;

public sealed class LogService
{
    private static readonly Lazy<LogService> _lazyInstance = new Lazy<LogService>(() => new LogService());

    public static LogService Instance
    {
        get
        {
            return _lazyInstance.Value;
        }
    }

    private LogService()
    {
        // place for instance initialization code
    }

    public void Log(object data)
    {
        Console.WriteLine("/log");
        Console.WriteLine(data);
        Console.WriteLine();
    }
}