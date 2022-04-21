using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPCycleGridViewModel
    {
        public bool isVisible { get; set; }
        public bool isVisibleGrid { get; set; }
        public string Header { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public List<MPCycleGridItemViewModel> Applications { get; set; }
        public MPGenericLinkButtonViewModel ApplicationStartButton { get; set; }
        public MPStepPostViewModel Popup { get; set; }
    }
}
