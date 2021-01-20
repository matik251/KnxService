using Serilog;
using System;
using Topshelf;

namespace KnxService5
{
    class Program
    {
        static int Main()
        {
            return (int)HostFactory.Run(x =>
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .CreateLogger();
                x.UseSerilog();

                x.UseAssemblyInfoForServiceInfo();

                bool throwOnStart = false;
                bool throwOnStop = false;
                bool throwUnhandled = false;

                x.Service(settings => new KnxService(throwOnStart, throwOnStop, throwUnhandled), s =>
                {
                    s.BeforeStartingService(_ => Console.WriteLine("BeforeStart"));
                    s.BeforeStoppingService(_ => Console.WriteLine("BeforeStop"));
                });

                x.SetStartTimeout(TimeSpan.FromSeconds(10));
                x.SetStopTimeout(TimeSpan.FromSeconds(10));

                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(3);
                    r.RunProgram(5, "ping 192.168.1.11");
                    r.RunProgram(5, "ping 192.168.2.1");

                    r.OnCrashOnly();
                    r.SetResetPeriod(1);
                });

                x.AddCommandLineSwitch("throwonstart", v => throwOnStart = v);
                x.AddCommandLineSwitch("throwonstop", v => throwOnStop = v);
                x.AddCommandLineSwitch("throwunhandled", v => throwUnhandled = v);

                x.OnException((exception) =>
                {
                    Console.WriteLine("Exception thrown - " + exception.Message);
                });
            });
        }
    }
}
