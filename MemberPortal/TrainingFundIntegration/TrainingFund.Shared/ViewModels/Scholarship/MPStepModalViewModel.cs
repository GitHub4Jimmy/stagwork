using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPStepModalViewModel
    {
        public string DatabaseName { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public List<MPStepBlockViewModel> Blocks { get; set; }
        public MPGenericLinkButtonViewModel Add { get; set; }
        public MPGenericLinkButtonViewModel Close { get; set; }
    }
}
