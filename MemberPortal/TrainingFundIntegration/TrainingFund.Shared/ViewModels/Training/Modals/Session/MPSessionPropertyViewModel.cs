using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionPropertyViewModel : ITranslatable
    {
        [Translatable]
        public string Title { get; set; }
        [Translatable]
        public string Value { get; set; }
    }
}