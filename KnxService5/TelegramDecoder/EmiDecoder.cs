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
            var idle = false;
            while (true)
            {
                KnxTelegram knxTelegram = null;
                DecodedTelegram decoded = null;
                try
                {
                    if(idle == true)
                    {
                        _log.Error("Back to decoding");
                    }
                    knxTelegram = service.GetKnxTelegramToDecode();

                    decoded = DecodeEmi(knxTelegram, service);
                }
                catch (Exception e)
                {
                    _log.Error(e.Message);
                }
                if (knxTelegram != null)
                {
                    idle = false;
                    try
                    {
                        knxTelegram.Processed = 1;
                        service.PutKnaTelegram(knxTelegram);
                        decoded.Tid = (long)knxTelegram.Tid;

                        service.PostDecodedTelegram(decoded);
                    }
                    catch (Exception e)
                    {
                        _log.Error(e.Message);
                    }
                }
                else
                {
                    _log.Error("NoLogsToDecode");
                    idle = true;
                    Thread.Sleep(10000);
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
