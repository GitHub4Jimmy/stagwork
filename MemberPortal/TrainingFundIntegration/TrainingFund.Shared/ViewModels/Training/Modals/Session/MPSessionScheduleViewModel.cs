using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionScheduleViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string ClassLengthDetails { get; set; }
        [Translatable]
        public string Dates { get; set; }
        [Translatable]
        public string Times { get; set; }
        [Translatable]
        public string ExpanderTextWhenClosed { get; set; }
        [Translatable]
        public string ExpanderTextWhenOpen { get; set; }
        [Translatable]
        public string Column1 { get; set; }
        [Translatable]
        public string Column2 { get; set; }
        [Translatable]
        public string Column3 { get; set; }
        [Translatable]
        public List<MPSessionScheduleItemViewModel> Items { get; set; }
    }
}