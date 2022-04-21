using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPSubmissionDataViewModel
    {
        public string DatabaseName { get; set; }
        public List<MPQuestionResponseViewModel> Responses { get; set; }
    }
}
