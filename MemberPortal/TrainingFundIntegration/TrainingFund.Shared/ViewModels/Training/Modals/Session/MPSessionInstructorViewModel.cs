using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionInstructorViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public List<MPSessionInstructorItemViewModel> Items { get; set; }
    }
}