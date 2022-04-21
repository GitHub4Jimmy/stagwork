using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPSelectionViewModel
    {
        public string DatabaseName { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string Empty { get; set; }
        public bool isSelected { get; set; }
        public string Value { get; set; }
        public Dictionary<string, string> ObjectValues { get; set; }
        public List<MPQuestionViewModel> SubQuestions { get; set; }
    }
}