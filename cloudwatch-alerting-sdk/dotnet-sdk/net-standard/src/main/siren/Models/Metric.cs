using System;
using System.Collections.Generic;

namespace StagwellTech.SirenSDK.Models
{
    public class Metric
    {
        public Metric() { }
        public Metric(string name, double value)
        {
            this.name = name;
            this.value = value;
        }

        //public string category;
        public string name;
        //public Dictionary<string, string> tags = new Dictionary<string, string>();
        public double value;
        //public DateTime ts = DateTime.Now;

    }
}
