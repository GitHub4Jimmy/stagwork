using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionLocationItemViewModel : ITranslatable
    {
        public int LocationId { get; set; }
        public bool isInPersonIcon { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string Name { get; set; }
        [Translatable]
        public string Line1 { get; set; }
        [Translatable]
        public string Line2 { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel Link { get; set; }
    }
}