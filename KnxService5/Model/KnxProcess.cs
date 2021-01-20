using System;
using System.Collections.Generic;

#nullable disable

namespace KnxService5.Model
{
    public partial class KnxProcess
    {
        public long Pid { get; set; }
        public string ProcessName { get; set; }
        public string ProcessIp { get; set; }
        public string ProcessType { get; set; }
        public string ProcessedFile { get; set; }
        public int? ActualTelegramNr { get; set; }
        public int? TotalTelegramNr { get; set; }
    }
}
