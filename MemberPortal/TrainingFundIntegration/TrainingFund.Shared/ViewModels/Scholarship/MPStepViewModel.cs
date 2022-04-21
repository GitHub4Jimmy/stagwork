using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPStepViewModel
    {
        public MPGenericLinkButtonViewModel SaveAndContinueButton { get; set; }
        public MPGenericLinkButtonViewModel SaveAndExitButton { get; set; }
        public MPGenericLinkButtonViewModel BackButton { get; set; }
        public MPGenericLinkButtonViewModel CancelButton { get; set; }
        public List<MPStepBlockViewModel> Blocks { get; set; }
        public List<string> Bullets { get; set; }
        public string SubHeader { get; set; }
        public string Header { get; set; }
        public MPStepModalViewModel Modal { get; set; }
        public MPStepPostViewModel Popup { get; set; }
        public MPStepPostViewModel InitialPopup { get; set; }
        public List<MPRequiredDependentViewModel> RequiredDependents { get; set; } = new List<MPRequiredDependentViewModel>();
    }
}
