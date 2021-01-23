using System;
using System.Collections.Generic;

#nullable disable

namespace KnxService5.Model
{
    public partial class KnxGroupAddress
    {
        public int Gid { get; set; }
        public string GroupAddress { get; set; }
        public string DeviceName { get; set; }
        public string Length { get; set; }
        public string Central { get; set; }
        public string Clutch { get; set; }
    }
}
