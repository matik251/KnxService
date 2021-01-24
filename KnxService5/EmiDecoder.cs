using KnxService5.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace KnxService5
{
    class EmiDecoder
    {
        static readonly LogWriter _log = HostLogger.Get<EmiDecoder>();

        public static void ProcessTelegrams(ApiService service)
        {
            while (true)
            {
                KnxTelegram knxTelegram = null;
                DecodedTelegram decoded = null;
                try
                {
                    knxTelegram = service.GetKnxTelegramToDecode();

                    decoded = DecodeEmi(knxTelegram, service);
                }
                catch (Exception e)
                {
                    _log.Error(e.Message);
                }
                if (knxTelegram != null)
                {
                    knxTelegram.Processed = 1;
                    service.PostKnxTelegram(knxTelegram);
                    service.PostDecodedTelegram(decoded);
                }
                else
                {
                    Thread.Sleep(300000);
                    _log.Error("NoLogsToDecode");
                }
            }
        }

        private static DecodedTelegram DecodeEmi(KnxTelegram encoded, ApiService service)
        {
            var decoded = new DecodedTelegram();

            decoded = DataDecoder.GetDatapoint(service, encoded);

            return decoded;
        }

    }


}
