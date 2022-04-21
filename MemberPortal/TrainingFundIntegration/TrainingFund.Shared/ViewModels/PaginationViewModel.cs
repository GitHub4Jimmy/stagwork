using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels
{
    public class PaginationViewModel
    {
        public MPGenericLinkButtonViewModel Back { get; set; }
        public MPGenericLinkButtonViewModel Next { get; set; }
        public List<PaginationLinkViewModel> Links { get; set; } = new List<PaginationLinkViewModel>();
    }
}
