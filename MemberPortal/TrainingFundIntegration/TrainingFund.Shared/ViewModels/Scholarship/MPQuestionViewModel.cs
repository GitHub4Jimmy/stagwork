using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPQuestionViewModel
    {
        public string SubHeader { get; set; }
        public string Header { get; set; }
        public MPQuestionValidationViewModal Validation { get; set; }
        public MPQuestionTypeViewModel Type { get; set; }
        public string DatabaseName { get; set; }
        public int ColumnCount { get; set; }
        public List<MPSelectionViewModel> Selections { get; set; }
        public List<string> PermittedDocType { get; set; }
        public bool IsDocDownloadPermitted { get; set; }
        public bool IsRequired { get; set; }
    }
}
