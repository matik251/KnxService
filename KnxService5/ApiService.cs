using KnxService5.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KnxService5
{
    public class ApiService 
    {
        static HttpClient httpClient;

        private KnxProcess knxProcess;

        const string apiUrl = @"https://192.168.1.200:1200";

        const string XmlApiEnding = @"/api/XmlFiles/";
        const string TelegramApiEnding = @"/api/KnxTelegrams/";
        const string ProcessApiEnding = @"/api/KnxProcesses/";

        #region Methods
        public string getFileName()
        {
            return knxProcess.ProcessedFile;
        }

        public Xmlfile GetXmlfile()
        {
            return Task.Run(async() => await GetXmlFileApi()).Result;
        }

        public KnxTelegram PostKnxTelegram(KnxTelegram newTelegram)
        {
            Task.Run(async () => await PostEncodedTelegram(newTelegram)).Wait();
        }

        public void PostProcess(KnxProcess init)
        {
            knxProcess = Task.Run(async () => await Postrocess(init)).Result;
        }

        public void UpdateProcessState(string processsedFile, int actualTelegramNr, int TotalTelegramNr)
        {
            knxProcess.ProcessedFile = processsedFile;
            knxProcess.ActualTelegramNr = actualTelegramNr;
            knxProcess.TotalTelegramNr = TotalTelegramNr;

            Task.Run(async () => await PutProcessUpadte(knxProcess)).Wait();
        }

        public void UpdateProcessState(int actualTelegramNr)
        {
            knxProcess.ActualTelegramNr = actualTelegramNr;

            Task.Run(async () => await PutProcessUpadte(knxProcess)).Wait();
        }
        #endregion

        #region API servcie handling
        private static async Task<Xmlfile> GetXmlFileApi()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl + XmlApiEnding + "/GetNotProcessedXmlFiles");

            requestMessage.Content = new StringContent("application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Xmlfile>(result);
            }
            else
            {
                // Handle error result
                throw new Exception();
                return null;
            }
        }

        private static async Task<KnxTelegram> PostEncodedTelegram(KnxTelegram telegram)
        {
            var result = new KnxTelegram();
            var payload = JsonConvert.SerializeObject(telegram);
           
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl + TelegramApiEnding);

            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var resultString = await responseMessage.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<KnxTelegram>(resultString);

            }
            else
            {
                // Handle error result
                throw new Exception();
                return;
            }
        }

        private static async Task<KnxProcess> Postrocess(KnxProcess process)
        {
            var result = new KnxProcess();

            var payload = JsonConvert.SerializeObject(process);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl + ProcessApiEnding);

            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var resultString = await responseMessage.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<KnxProcess>(resultString);
            }
            else
            {
                // Handle error result
                throw new Exception();
            }
            return result;
        }

        private static async Task PutProcessUpadte(KnxProcess process) 
        {
            var payload = JsonConvert.SerializeObject(process);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl + ProcessApiEnding + process.Pid);

            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle error result
                throw new Exception();
                return;
            }
        }

        #endregion
    }
}
