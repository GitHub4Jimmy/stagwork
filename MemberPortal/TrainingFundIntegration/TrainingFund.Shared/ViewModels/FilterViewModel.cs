using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels
{
    public class FilterViewModel : ITranslatable
    {
        public string Value { get; set; }
        [Translatable]
        public string Text { get; set; }
        public string DatabaseName { get; set; }
        public bool isSelected { get; set; }

        [Translatable]
        public List<FilterViewModel> NestedFilters { get; set; }
    }
}