using KnxService5.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Topshelf.Logging;

namespace KnxService5
{
    public class XmlHandler
    {
        static readonly LogWriter _log = HostLogger.Get<XmlHandler>();
        public static ConcurrentBag<int> TelegramsList = new ConcurrentBag<int>();

        public static void ProcessXml(ApiService service)
        {
            try
            {
                var xmlfile = service.GetXmlfile();
                if (xmlfile.TelegramsCount.HasValue)
                {
                    service.UpdateProcessState(xmlfile.FileName, 0, (int)xmlfile.TelegramsCount);
                }
                var temp = ReadFileTelegrams(xmlfile, service);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
            }
        }

        private static ConcurrentBag<KnxTelegram> ReadFileTelegrams(Xmlfile xmlfile, ApiService service)
        {
            var telegramsList = new ConcurrentBag<KnxTelegram>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlfile.Xmldata);

            XmlNodeList tList = xmlDoc.GetElementsByTagName("Telegram");
            var tListGenercic = tList.Cast<XmlElement>();
            Parallel.ForEach(tListGenercic, (t) =>
            {
                try
                {
                    var telegram = new KnxTelegram();
                    XmlElement temp = (XmlElement)t;

                    telegram.TimestampS = temp.GetAttributeNode("Timestamp").Value;
                    var tempDate = DateTime.Parse(telegram.TimestampS);
                    telegram.Timestamp = tempDate.AddHours(2);

                    telegram.Service = temp.GetAttributeNode("Service").Value;
                    telegram.FrameFormat = temp.GetAttributeNode("FrameFormat").Value;
                    telegram.RawData = temp.GetAttributeNode("RawData").Value;
                    telegram.RawDataLength = telegram.RawData.Length;
                    telegram.FileName = xmlfile.FileName;

                    telegramsList.Add(service.PostKnxTelegram(telegram));

                    service.UpdateProcessState(telegramsList.Count());

                }
                catch (Exception e)
                {
                    _log.Error(e.Message);
                }

            });

            return telegramsList;
        }

    }
}
