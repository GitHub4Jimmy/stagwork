using System.Collections.Generic;

namespace TrainingFund.DNN.Integration.ViewModels
{
    public class LinkAttributesViewModel
    {
        public string Link { get; set; }
        public string Enabled { get; set; }
        public string Target { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> DataAttributes { get; set; } = new Dictionary<string, string>();
    }
}
