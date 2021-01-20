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
        public string DestinationAddress { get; set; }
    }
}
