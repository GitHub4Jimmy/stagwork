using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPCycleBoxViewModel
    {
        public bool isVisible { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string Expando { get; set; }
        public List<string> ExpandoBullets { get; set; }
    }
}
