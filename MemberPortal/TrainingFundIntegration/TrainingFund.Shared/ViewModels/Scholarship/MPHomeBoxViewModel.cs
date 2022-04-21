using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPHomeBoxViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        public bool isVisibleContent { get; set; }
        [Translatable]
        public string SubHeader { get; set; }
        [Translatable]
        public string Content { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel ContentLink { get; set; }
        [Translatable]
        public List<MPHomeBoxItemViewModel> Items { get; set; }
        public bool isVisibleFooterContent { get; set; }
        [Translatable]
        public string FooterContent { get; set; }
    }
}