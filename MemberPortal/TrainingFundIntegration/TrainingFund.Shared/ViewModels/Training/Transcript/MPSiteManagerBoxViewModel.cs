using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPSiteManagerBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        public List<MPSiteManagerViewModel> SiteManagers { get; set; }
        public int SiteManagerCount { get; set; }
        public bool isVisible { get; set; }
    }
}