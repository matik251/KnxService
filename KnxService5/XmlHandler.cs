using KnxService5.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            while (true)
            {
                try
                {
                    var xmlfile = service.GetXmlfile();
                    if(xmlfile != null)
                    {
                        xmlfile.IsProcessed = (xmlfile.IsProcessed == null ? 0 : xmlfile.IsProcessed.Value + 1);
                        service.UpdateXmlFile(xmlfile);
                        if (!xmlfile.TelegramsCount.HasValue)
                        {
                            xmlfile.TelegramsCount = xmlfile.Xmldata.ToString().Where(c => c.Equals('<')).Count() - 2;
                            service.UpdateProcessState(xmlfile.FileName, 0, (int)xmlfile.TelegramsCount);
                        }
                        var temp = ReadFileTelegrams(xmlfile, service);
                    }
                    else
                    {
                        _log.Error("NoLogsToDecode");
                        Thread.Sleep(300000);
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e.Message);
                    _log.Error("NoLogsToDecode");
                    Thread.Sleep(300000);
                }
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

            try
            {
                xmlfile.IsProcessed = 1;
                service.UpdateXmlFile(xmlfile);
            }catch(Exception e)
            {
                _log.Error(e.Message);
            }

            return telegramsList;
        }

    }
}
