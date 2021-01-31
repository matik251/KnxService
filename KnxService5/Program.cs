using Serilog;
using System;
using Topshelf;
using Topshelf.Logging;
using Topshelf.StartParameters;

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

                x.EnableStartParameters();


                bool throwOnStart = false;
                bool throwOnStop = false;
                bool throwUnhandled = false;
                bool XmlReader = false;
                bool TelegramDecoder = false;
                string ServerUrl = "https://192.168.1.200:1200";

#if DEBUG
                XmlReader = true;
                TelegramDecoder = true;
#endif


                x.Service(settings => new KnxService(throwOnStart, throwOnStop, throwUnhandled, XmlReader, TelegramDecoder, ServerUrl), s =>
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


                x.WithStartParameter("XmlReader", n => XmlReader = (n.Equals("1") || (n.Equals("true"))));
                x.WithStartParameter("TelegramDecoder", n => TelegramDecoder = (n.Equals("1") || (n.Equals("true"))));
                x.WithStartParameter("ServerUrl", n => ServerUrl = n);

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
