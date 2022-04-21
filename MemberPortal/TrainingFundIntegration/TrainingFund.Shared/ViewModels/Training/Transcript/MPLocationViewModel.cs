using TrainingFund.Shared.Attributes;
using TrainingFund.Shared.Interfaces;

namespace TrainingFund.Shared.ViewModels.Training.Transcript
{
    public class MPLocationViewModel : ITranslatable
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double DistanceToHomeOrWork { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public MPLocationTypeViewModel Type { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel SearchLink { get; set; }
        [Translatable]
        public MPGenericLinkButtonViewModel DirectionsLink { get; set; }
    }
}