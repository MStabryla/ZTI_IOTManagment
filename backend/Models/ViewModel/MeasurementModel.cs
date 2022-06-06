using System;

namespace SysOT.Models
{
    public class MeasurementModel
    {
        public string Value { get; set; }
        
        public DateTime Time { get; set; }

        public MeasurementType Type { get; set; }
    }
}