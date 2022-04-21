using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Modals.Session
{
    public class MPSessionInstructorItemViewModel : ITranslatable
    {
        public int InstructorId { get; set; }
        public string Name { get; set; }
        [Translatable]
        public string Bio { get; set; }
        public string HeadshotJPG { get; set; }
    }
}