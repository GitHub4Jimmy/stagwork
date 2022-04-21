using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels
{
    public class FilterBoxViewModel : ITranslatable
    {
        [Translatable]
        public string Header { get; set; }
        public string DatabaseName { get; set; }
        [Translatable]
        public List<FilterViewModel> Items { get; set; }
    }
}