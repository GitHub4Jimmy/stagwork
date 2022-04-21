using System.Collections.Generic;
using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionNextClassViewModel : ITranslatable
    {
        public bool isVisible { get; set; }
        [Translatable]
        public string Header { get; set; }
        [Translatable]
        public string DateHeader { get; set; }
        [Translatable]
        public string DateFooter { get; set; }
        [Translatable]
        public string DayAndTime { get; set; }
        [Translatable]
        public string LocationName { get; set; }
        [Translatable]
        public string LocationAddress { get; set; }
        [Translatable]
        public string RoomName { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel LocationLink { get; set; }
        [Translatable]
        public List<MPGenericLinkButtonViewModel> FooterLinks { get; set; }
        [Translatable]
        public string Footer { get; set; }
    }
}