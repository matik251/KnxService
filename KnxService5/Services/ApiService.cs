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

        HttpClient httpClient;

        public ApiService()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            httpClient = new HttpClient(handler);
        }

        public KnxProcess knxProcess;

        const string apiUrl = @"https://192.168.1.200:1200";

        const string XmlApiEnding = @"/api/XmlFiles";
        const string TelegramApiEnding = @"/api/KnxTelegrams";
        const string ProcessApiEnding = @"/api/KnxProcesses";
        const string DecodedTelegramApiEnding = @"/api/DecodedTelegrams";

        #region Methods XML Handler

        public string getFileName()
        {
            return knxProcess.ProcessedFile;
        }

        public Xmlfile GetXmlfile()
        {
            return Task.Run(async() => await GetXmlFileApi()).Result;
        }

        public void UpdateXmlFile(Xmlfile xmlfile)
        {
            Task.Run(async () => await PutXMlFile(xmlfile)).Wait();
        }

        #endregion

        #region Mehods Process
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

        #region Methods Telegrams

        public KnxTelegram PostKnxTelegram(KnxTelegram newTelegram)
        {
            return Task.Run(async () => await PostEncodedTelegram(newTelegram)).Result;
        }

        public KnxTelegram GetKnxTelegramToDecode()
        {
            return Task.Run(async () => await GetEncodedTelegram()).Result;
        }

        public void UpdateKnxTelegramState(KnxTelegram updated)
        {
            Task.Run(async () => await UpdateProcessedStateTelegram(updated)).Wait();
        }

        public DecodedTelegram PostDecodedTelegram(DecodedTelegram decodedTelegram)
        {
            return Task.Run(async () => await PostDecodedTelegramApi(decodedTelegram)).Result;
        }

        public void PutKnaTelegram(KnxTelegram knxTelegram)
        {
            Task.Run(async () => await PutEncodedTelegram(knxTelegram)).Wait();
        }

        public KnxGroupAddress GetGroupAddressInfo(string groupAddress)
        {
            return Task.Run(async () => await GetGroupAddressInfoApi(groupAddress)).Result;
        }


        #endregion


        #region API servcie handling

        //XML

        private async Task<Xmlfile> GetXmlFileApi()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl + XmlApiEnding + "/GetNotProcessedXmlFiles");

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

        private async Task PutXMlFile(Xmlfile xmlfile)
        {
            var payload = JsonConvert.SerializeObject(xmlfile);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl + XmlApiEnding + "/PutXmlFiles/" + xmlfile.Fid);

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
            }
        }

        //Zakodowane telegramy

        private async Task<KnxTelegram> PostEncodedTelegram(KnxTelegram telegram)
        {
            var result = new KnxTelegram();
            var payload = JsonConvert.SerializeObject(telegram);
            payload = payload.Replace("\"Tid\":null,", "");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl + TelegramApiEnding + "/PostKnxTelegrams");

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
            }
            return result;
        }

        private async Task PutEncodedTelegram(KnxTelegram telegram)
        {
            var payload = JsonConvert.SerializeObject(telegram);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl + TelegramApiEnding + "/PutKnxTelegrams/" + telegram.Tid);

            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var resultString = await responseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle error result
                throw new Exception();
            }
        }


        private async Task<KnxTelegram> GetEncodedTelegram()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl + TelegramApiEnding + "/GetNotProcessed");

            requestMessage.Content = new StringContent("application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<KnxTelegram>(result);
            }
            else
            {
                // Handle error result
                throw new Exception();
                return null;
            }
        }


        private async Task UpdateProcessedStateTelegram(KnxTelegram processed)
        {
            var payload = JsonConvert.SerializeObject(processed);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl + ProcessApiEnding +"/" +processed.Tid);

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

        //Zdekodowane telgramy

        private async Task<DecodedTelegram> PostDecodedTelegramApi(DecodedTelegram telegram)
        {
            var result = new DecodedTelegram();
            var payload = JsonConvert.SerializeObject(telegram);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl + DecodedTelegramApiEnding);

            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var resultString = await responseMessage.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<DecodedTelegram>(resultString);
            }   
            else
            {
                // Handle error result
                throw new Exception();
            }
            return result;
        }

        //Adres Grupowy

        private async Task<KnxGroupAddress> GetGroupAddressInfoApi(string groupAddress )
        {
            var result = new KnxGroupAddress();
            groupAddress = groupAddress.Replace("/", "%2F");
            var parameters = new Dictionary<string, string> { { "groupAddress", groupAddress } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            var requestMessage = new HttpRequestMessage( HttpMethod.Get, apiUrl + TelegramApiEnding + "/?" + encodedContent);

            requestMessage.Content = new StringContent( "application/json");

            var responseMessage = await httpClient.SendAsync(requestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var resultString = await responseMessage.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<KnxGroupAddress>(resultString);
            }
            else
            {
                // Handle error result
                throw new Exception();
            }
            return result;
        }


        //Procesy

        private async Task<KnxProcess> Postrocess(KnxProcess process)
        {
            var result = new KnxProcess();

            var payload = JsonConvert.SerializeObject(process);
            payload = payload.Replace("\"Pid\":null,", "");

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

        private async Task PutProcessUpadte(KnxProcess process) 
        {
            var payload = JsonConvert.SerializeObject(process);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, apiUrl + ProcessApiEnding +"/"+ process.Pid);

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
