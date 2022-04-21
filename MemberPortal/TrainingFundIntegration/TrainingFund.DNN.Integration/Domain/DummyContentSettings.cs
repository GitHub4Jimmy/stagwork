using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingFund.Shared.ViewModels.Scholarship;

namespace TrainingFund.DNN.Integration.Domain
{
    public class DummyContentSettings
    {
        public bool UseDebugContent { get; set; }
        public int DebugPersonId { get; set; } = 0;
        public string DoceboDebugUsername { get; set; }
        public Dictionary<string, string> DebugObjectPaths { get; set; } = new Dictionary<string, string>();
    }
}
