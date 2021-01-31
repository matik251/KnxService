using KnxService5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnxServiceTest
{
    public class DataDecoderTests
    {
        [Theory]
        [InlineData(new byte[] { 0x2B, 0x09, 0x03, 0x01, 0x05, 0x06, 0x04, 0xCC, 0x94, 0x8D, 0x7C, 0xB0, 0x12, 0x3D, 0x69, 0x0D, 0xE3, 0x00, 0x00, 0x80, 0x03, 0x07, 0x67 }, new byte[] { 0x03, 0x07 })]
        public void GetData(byte[] data, byte[] expectedResult)
        {
            var result = DataDecoder.GetData(data);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("2B090301050604CC948D7CB0123D690DE3000080030767", new byte[] { 0x2B, 0x09, 0x03, 0x01, 0x05, 0x06, 0x04, 0xCC, 0x94, 0x8D, 0x7C, 0xB0, 0x12, 0x3D, 0x69, 0x0D, 0xE3, 0x00, 0x00, 0x80, 0x03, 0x07, 0x67 })]
        public void GetBytesTest1(string data, byte[] expectedResult)
        {
            var result = DataDecoder.GetBytes(data);
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [InlineData(2, 3, "2B090301050604CC948D7CB0123D690DE3000080000067", new byte[] { 0x03, 0x01, 0x05 })]
        public void GetBytesTest2(int? pos, int? len, string data, byte[] expectedResult)
        {
            var result = DataDecoder.GetBytes(pos, len, data);
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [InlineData(2, 3, new byte[] { 0x2B, 0x09, 0x03, 0x01, 0x05, 0x06, 0x04, 0xCC, 0x94, 0x8D, 0x7C, 0xB0, 0x12, 0x3D, 0x69, 0x0D, 0xE3, 0x00, 0x00, 0x80, 0x00, 0x00, 0x67 }, new byte[] { 0x03, 0x01, 0x05 })]
        public void GetBytesTest3(int? pos, int? len, byte[] data, byte[] expectedResult)
        {
            var result = DataDecoder.GetBytes(pos, len, data);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("2B090301050604CC948D7CB0123D690DE30080030767", "2B 09 03 01 05 06 04 CC 94 8D 7C B0 12 3D 69 0D E3 00 80 03 07 67 ")]
        public void SplitByTwoBytes(string data, string expectedResult)
        {
            var result = DataDecoder.SplitByTwoBytes(data);
            Assert.Equal(expectedResult, result);
        }
    }
}
