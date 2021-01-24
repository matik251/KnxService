using KNXLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxService5
{
    static class AddressDecoder
    {
        public static string GetSourceAddress(byte[] data)
        {
            var connection = new KnxConnectionRouting();
            var tempBytes = GetAddres1(data);
            var first = connection.FromDataPoint("5.010", tempBytes);
            tempBytes = GetAddres2(data);
            var second = connection.FromDataPoint("5.010", tempBytes);
            tempBytes = GetAddres3(data);
            var third = connection.FromDataPoint("5.010", tempBytes);

            var returnVal = first + "/" + second + "/" + tempBytes;
            return returnVal;
        }

        public static string GetGroupAddress(byte[] data)
        {
            var connection = new KnxConnectionRouting();
            var tempBytes = GetSource1(data);
            var first = connection.FromDataPoint("5.010", tempBytes);
            tempBytes = GetSource2(data);
            var second = connection.FromDataPoint("5.010", tempBytes);
            tempBytes = GetAddres3(data);
            var third = connection.FromDataPoint("5.010", tempBytes);

            var returnVal = first + "/" + second + "/" + tempBytes;
            return returnVal;
        }


        #region Logic
        private static byte[] GetAddres1(byte[] data)
        {
            var bits = GetBites(data[0], 0, 5);
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            var reversed = new BitArray(bits.Cast<bool>().Reverse().ToArray());
            reversed.CopyTo(bytes, 0);
            return bytes;
        }

        private static byte[] GetSource1(byte[] data)
        {
            var bits = GetBites(data[0], 0, 4);
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            var reversed = new BitArray(bits.Cast<bool>().Reverse().ToArray());
            reversed.CopyTo(bytes, 0);
            return bytes;
        }


        private static byte[] GetAddres2(byte[] data)
        {
            var bits = GetBites(data[0], 5, 3);
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            var reversed = new BitArray(bits.Cast<bool>().Reverse().ToArray());
            reversed.CopyTo(bytes, 0);
            return bytes;
        }

        private static byte[] GetSource2(byte[] data)
        {
            var bits = GetBites(data[0], 4, 4);
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            var reversed = new BitArray(bits.Cast<bool>().Reverse().ToArray());
            reversed.CopyTo(bytes, 0);
            return bytes;
        }


        private static byte[] GetAddres3(byte[] data)
        {
            byte[] bytes = new byte[1];
            bytes[0] = data[1];
            return bytes;
        }

        private static BitArray GetBites(byte data, int start, int length)
        {
            BitArray temp = new BitArray(8);
            var test = new BitArray(8);
            var testList = GetBits(data);
            for (int index = 0; index < length; index += 1)
            {
                temp.Set(8 - length + index, testList.Skip(start + index).First());
            }
            return temp;
        }

        private static IEnumerable<bool> GetBits(byte b)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return (b & 0x80) != 0;
                b *= 2;
            }
        }
        #endregion
    }
}
