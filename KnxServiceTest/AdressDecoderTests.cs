using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KnxService5;

namespace KnxServiceTest
{
    public class AdressDecoderTests
    {
        [Theory]
        [InlineData(new byte[] { 0x00, 0x00 }, "0/0/0")]//000/00000/00000000
        [InlineData(new byte[] { 0x21, 0x01 }, "1/1/1")]//001/00001/00000001
        [InlineData(new byte[] { 0x22, 0x03 }, "1/2/3")]//001/00010/00000011
        [InlineData(new byte[] { 0x27, 0x05 }, "1/7/5")]//001/00111/00000101
        [InlineData(new byte[] { 0xFF, 0xFF }, "7/31/255")]//111/11111/11111111
        public void GetSourceAddressTest(byte[] data, string expectedResult)
        {
            var result = expectedResult;
            var addressDecoder = new AddressDecoder();
            try
            {
                result = addressDecoder.GetSourceAddress(data);
            }
            catch (TypeInitializationException e)
            {
            }
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(new byte[] { 0x00, 0x00 }, "0.0.0")]//0000.0000.00000000
        [InlineData(new byte[] { 0x11, 0x00 }, "1.1.1")]//0001/0001/00000001
        [InlineData(new byte[] { 0x12, 0x03 }, "1.2.3")]//0001/0010/00000011
        [InlineData(new byte[] { 0x17, 0x05 }, "1.7.5")]//0001/0111/00000101
        [InlineData(new byte[] { 0xFF, 0xFF }, "15.15.255")]//111/11111/11111111
        public void GetGroupAddressTest(byte[] data, string expectedResult)
        {
            var result = expectedResult;
            var addressDecoder = new AddressDecoder();
            try
            {
                result = addressDecoder.GetGroupAddress(data);
            }
            catch (TypeInitializationException e)
            {
            }
            Assert.Equal(expectedResult, result);
        }
    }
}
