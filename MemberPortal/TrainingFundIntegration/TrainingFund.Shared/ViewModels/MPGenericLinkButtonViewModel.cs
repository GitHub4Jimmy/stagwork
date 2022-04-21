using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels
{
    public class MPGenericLinkButtonViewModel : ITranslatable
    {
        public string DatabaseName { get; set; }
        public Dictionary<string, string> DatabaseIds { get; set; }
        [Translatable]
        public string Text { get; set; }
        [Translatable]
        public string SubText { get; set; }
        public string Link { get; set; }
        public bool isNewWindow { get; set; }
        public bool? isEnabled { get; set; }
    }
}