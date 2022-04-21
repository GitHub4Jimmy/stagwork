using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingFund.Shared.ViewModels
{
    public class SearchViewModel
    {
        public int PersonId { get; set; }
        public string SearchTerm { get; set; }
        public List<FilterBoxViewModel> Filters { get; set; }
    }
}
