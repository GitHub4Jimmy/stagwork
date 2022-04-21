using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPStepBlockViewModel
    {
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public List<MPQuestionViewModel> Questions { get; set; }
    }
}
