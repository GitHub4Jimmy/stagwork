using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPStatusModalViewModel
    {
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public List<MPStatusGridItemViewModel> Grid { get; set; }
        public MPGenericLinkButtonViewModel CloseButton { get; set; }

    }
}
