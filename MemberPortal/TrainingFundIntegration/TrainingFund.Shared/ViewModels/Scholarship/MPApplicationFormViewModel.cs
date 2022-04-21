using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPApplicationFormViewModel
    {
        public string Header { get; set; }
        public List<MPStepHeaderViewModel> StepHeaders { get; set; }
        public MPStepViewModel Step { get; set; }
        public bool IsLocked { get; set; }
        public bool IsLastStep { get; set; }
        public bool IsFirstStep { get; set; }
    }
}
