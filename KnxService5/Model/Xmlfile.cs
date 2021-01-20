using System;
using System.Collections.Generic;

#nullable disable

namespace KnxService5.Model
{
    public partial class Xmlfile
    {
        public int Fid { get; set; }
        public string FileName { get; set; }
        public string Xmldata { get; set; }
        public int? IsProcessed { get; set; }
        public int? ProcessingPercentage { get; set; }
        public int? TelegramsCount { get; set; }
        public int? CancellationToken { get; set; }
    }
}
