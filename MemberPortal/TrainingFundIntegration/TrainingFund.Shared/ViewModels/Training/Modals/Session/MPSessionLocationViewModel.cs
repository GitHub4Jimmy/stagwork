using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionLocationViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string SubHeader { get; set; }
        [Translatable]
        public List<MPSessionLocationItemViewModel> Items { get; set; }
    }
}