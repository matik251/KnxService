using System;
using System.Collections.Generic;

#nullable disable

namespace KnxService5.Model
{
    public partial class KnxTelegram
    {
        public long Tid { get; set; }
        public string TimestampS { get; set; }
        public DateTime Timestamp { get; set; }
        public string Service { get; set; }
        public string FrameFormat { get; set; }
        public string RawData { get; set; }
        public int? RawDataLength { get; set; }
        public string FileName { get; set; }
    }
}
