namespace TrainingFund.Shared.ViewModels
{
    public class CanViewViewModel
    {
        public bool IsTrainingMenuVisible { get; set; } = false;
        public bool IsTrainingEnrollButtonVisible { get; set; } = false;
        public bool IsScholarshipMenuVisible { get; set; } = false;
        public bool IsPrioritizeScholarshipHomeCard { get; set; }
        public bool IsPriortizeTrainingHomeCard { get; set; }
    }
}