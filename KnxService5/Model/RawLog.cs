using System;
using System.Collections.Generic;

#nullable disable

namespace KnxService5.Model
{
    public partial class RawLog
    {
        public int LogId { get; set; }
        public DateTime? LogTimestamp { get; set; }
        public string Service { get; set; }
        public string FrameFormat { get; set; }
        public string RawData { get; set; }
    }
}
