using KnxService5.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace KnxService5
{
    class EmiDecoder
    {
        static readonly LogWriter _log = HostLogger.Get<EmiDecoder>();

        public static void ProcessTelegram(ApiService service)
        {
            try
            {
                var telegram = service.GetKnxTelegramToDecode();

                var temp = DecodeEmi(telegram, service);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
            }
        }

        private static DecodedTelegram DecodeEmi(KnxTelegram encoded, ApiService service)
        {
            var decoded = new DecodedTelegram();

            //TOOD

            return decoded;
        }

        private static DecodedTelegram DecodeDatapoint(DecodedTelegram current)
        {
            //TODO
            return current;
        }

        enum Separator
        {
            //TODO uzupelnic z kartki
            Comma = ',',
            Tab = '\t',
            Space = ' '
        }
    }
}
