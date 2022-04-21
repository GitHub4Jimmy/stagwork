using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPStepPostViewModel
    {
        public string Header { get; set; }
        public string Text { get; set; }
        public List<string> Bullets { get; set; }
        public bool isPermitEmailReminder { get; set; }
        public string EmailReminder { get; set; }
        public string CloseButtonText { get; set; }
        public MPGenericLinkButtonViewModel CustomButton { get; set; }
        public List<MPStepBlockViewModel> Blocks { get; set; }
    }
}
