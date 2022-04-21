using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionScheduleItemViewModel : ITranslatable
    {
        [Translatable]
        public string Column1Header { get; set; }
        [Translatable]
        public string Column1TextLine1 { get; set; }
        [Translatable]
        public string Column1TextLine2 { get; set; }
        [Translatable]
        public string Column1Subscript { get; set; }
        [Translatable]
        public string Column2Header { get; set; }
        [Translatable]
        public string Column2TextLine1 { get; set; }
        [Translatable]
        public string Column2TextLine2 { get; set; }
        [Translatable]
        public string Column2Subscript { get; set; }
        [Translatable]
        public string Column3Header { get; set; }
        [Translatable]
        public string Column3TextLine1 { get; set; }
        [Translatable]
        public string Column3TextLine2 { get; set; }
        [Translatable]
        public string Column3Subscript { get; set; }
    }
}