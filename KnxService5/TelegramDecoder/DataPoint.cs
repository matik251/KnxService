using KNXLib;
using KnxService5.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxService5
{
    public class DataPoint
    {
        public static DecodedTelegram GetData(byte[] data, KnxGroupAddress groupAddres, ApiService service, KnxTelegram telegram)
        {
            var returnTelegram = new DecodedTelegram();

            var datpointInfo = DataPointsDictionary.Where(d => d.Key.Equals(groupAddres.Length)).FirstOrDefault();

            var connection = new KnxConnectionRouting();
            
            var decodedData = connection.FromDataPoint(datpointInfo.Value, data);
            if(decodedData == null)
            {
                connection.FromDataPoint("5.010", data);
            }

            returnTelegram.Data = decodedData.ToString();
            try
            {
                returnTelegram.DataFloat = (float)decodedData;
            }
            catch(Exception e)
            {
                returnTelegram.DataFloat = 0.0f;
            }
            returnTelegram.SerializedData = JsonConvert.SerializeObject(decodedData);

            return returnTelegram;
        } 

        static readonly Dictionary<string, int> CEMITypes = new Dictionary<string, int>
        {
            {"Unknown", 0x00},
            {"BusmonitorIndication", 0x2b},
            {"DataRequest", 0x11},
            {"DataConfirmation", 0x2e},
        };

        static readonly Dictionary<string, int> DataPointsByteLength = new Dictionary<string, int>
        {
            {"5.001", 2},
            {"5.004", 2},
            {"5.010", 2},
            {"9.001", 4},
        };

        public static readonly Dictionary<string, string> DataPointsDictionary = new Dictionary<string, string>
        {
            {"", "9.001" },//if null then 5.001
            {"blind control", "5.010" },
            {"1 bit", "5.004" },
            {"1 byte", "5.001" },
            {"1-byte", "5.001" },
            {"2 bytes", "9.001" },
            {"boolean", "5.004" },
            {"cooling/heating", "5.004" },
            {"HVAC mode", "5.001" },
            {"lux (Lux)", "9.001" },
            {"parts/million (ppm)", "9.001" },
            {"percentage (0..100%)", "5.001" },
            {"pulses", "5.010" },
            {"speed (m/s)", "9.001" },
            {"switch", "5.004" },
            {"temperature (°C)", "9.001" }
        };
    }
}

