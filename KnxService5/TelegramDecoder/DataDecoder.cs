using KnxService5.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KNXLib;
using Knx.Bus.Common;

namespace KnxService5
{
    public static class DataDecoder
    {

        public static DecodedTelegram GetDatapoint(ApiService service, KnxTelegram knxTelegram)
        {
            var decodedTelegram = new DecodedTelegram();

            var localByteArr = GetBytes(knxTelegram.RawData);
            if (localByteArr.Length >= 20)
            {

                try
                {
                    var addressDecoder = new AddressDecoder();

                    var source = addressDecoder.GetSourceAddress(GetBytes(13, 2, localByteArr));

                    var group = addressDecoder.GetGroupAddress(GetBytes(15, 2, localByteArr));

                    var datapoint = service.GetGroupAddressInfo(group);

                    var data = DataPoint.GetData(GetData(localByteArr), datapoint, service, knxTelegram);

                    decodedTelegram.Timestamp = knxTelegram.Timestamp;
                    decodedTelegram.TimestampS = knxTelegram.TimestampS;

                    decodedTelegram.Service = knxTelegram.Service;
                    decodedTelegram.FrameFormat = knxTelegram.FrameFormat;

                    decodedTelegram.SourceAddress = source;
                    decodedTelegram.GroupAddress = group;

                    decodedTelegram.Data = data.Data;
                    decodedTelegram.DataFloat = data.DataFloat;
                    decodedTelegram.SerializedData = data.SerializedData;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


            return decodedTelegram;
        }

        public static byte[] GetData(byte[] data)
        {
            var ret = GetBytes(1, 1, data);
            if (data.Length == 20)
            {
                ret = GetBytes(17, 2, data);
            }else if(data.Length == 21)
            {
                ret = GetBytes(18, 2, data);
            }
            else if (data.Length == 22)
            {
                ret = GetBytes(19, 2, data);
            }
            return ret;
        }

        public static byte[] GetBytes(string data)
        {
            var returnBytes = new byte[] { };
            try
            {
                var byteString = SplitByTwoBytes(data);
                returnBytes = ByteArrayExtensions.ParseHexBytes(byteString);
            }
            catch (Exception e)
            {
            }
            return returnBytes;
        }


        public static byte[] GetBytes(int? pos, int? len, string data)
        {
            var returnBytes = new byte[] { };
            try
            {
                var byteString = SplitByTwoBytes(data);
                returnBytes = ByteArrayExtensions.ParseHexBytes(byteString);
                if (pos != null && len != null)
                {
                    returnBytes = returnBytes.Skip((int)pos).Take((int)(len)).ToArray();
                }

            }
            catch (Exception e)
            {
            }
            return returnBytes;
        }


        public static byte[] GetBytes(int? pos, int? len, byte[] data)
        {
            try
            {
                if (pos != null && len != null)
                {
                    data = data.Skip((int)pos).Take((int)(len)).ToArray();
                }

            }
            catch (Exception e)
            {
            }
            return data;
        }

        public static string SplitByTwoBytes(string data)
        {
            var byteString = "";
            if (data.Length % 2 == 0)
            {
                var iterator = 0;
                foreach (var ch in data)
                {
                    byteString += ch;
                    iterator++;
                    if (iterator >= 2)
                    {
                        byteString += " ";
                        iterator = 0;
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
            return byteString;
        }
    }
}