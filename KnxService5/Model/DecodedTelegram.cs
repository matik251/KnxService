using System;
using System.Collections.Generic;

#nullable disable

namespace KnxService5.Model
{
    public partial class DecodedTelegram
    {
        public long Tid { get; set; }
        public string TimestampS { get; set; }
        public DateTime Timestamp { get; set; }
        public string Service { get; set; }
        public string FrameFormat { get; set; }
        public string SourceAddress { get; set; }
        public string GroupAddress { get; set; }
        public string DeviceName { get; set; }
        public string Data { get; set; }
        public double? DataFloat { get; set; }
        public string SerializedData { get; set; }
    }
}
