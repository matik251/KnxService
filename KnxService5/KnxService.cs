﻿using System;
using System.Threading;
using Topshelf;
using Topshelf.Logging;

namespace KnxService5
{
    internal class KnxService: ServiceControl
    {
        readonly bool _throwOnStart;
        readonly bool _throwOnStop;
        readonly bool _throwUnhandled;
        static readonly LogWriter _log = HostLogger.Get<KnxService>();

        ApiService apiService = new ApiService();

        public KnxService(bool throwOnStart, bool throwOnStop, bool throwUnhandled)
        {
            _throwOnStart = throwOnStart;
            _throwOnStop = throwOnStop;
            _throwUnhandled = throwUnhandled;
        }

        public bool Start(HostControl hostControl)
        {
            _log.Info("KnxService Starting...");

            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));

            Thread.Sleep(1000);

            if (_throwOnStart)
            {
                _log.Info("Throwing as requested");
                throw new InvalidOperationException("Throw on Start Requested");
            }
            

            ThreadPool.QueueUserWorkItem(x =>
            {
                XmlHandler.ProcessXml(apiService);
            });

            //ThreadPool.QueueUserWorkItem(x =>
            //{
            //    EmiDecoder.ProcessTelegrams(apiService);
            //});

            _log.Info("KnxService Started");

            Thread.Sleep(10000);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _log.Info("KnxService Stopped");

            if (_throwOnStop)
                throw new InvalidOperationException("Throw on Stop Requested!");

            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            _log.Info("KnxService Paused");

            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            _log.Info("KnxService Continued");

            return true;
        }
    }
}
