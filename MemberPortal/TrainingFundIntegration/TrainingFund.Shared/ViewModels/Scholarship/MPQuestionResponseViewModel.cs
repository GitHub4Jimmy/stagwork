using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPQuestionResponseViewModel
    {
        public string ResponseValue { get; set; }
        public byte[] Document { get; set; }
        public List<MPQuestionResponseViewModel> SubResponseValues { get; set; }
        public List<Dictionary<string, string>> ModalValues { get; set; }
    }
}
