using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingFund.Shared.ViewModels.Scholarship
{
    public class MPApplicationFormSubmitViewModel
    {
        public int V3PersonId { get; set; }
        public int ApplicationId { get; set; }
        public int StepOrderId { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorHeader { get; set; }
    }
}
