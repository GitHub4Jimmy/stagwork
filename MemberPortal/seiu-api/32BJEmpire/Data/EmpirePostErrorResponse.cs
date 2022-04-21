using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIU32BJEmpire.Data
{
    public class EmpirePostErrorResponse : IEmpirePostResponse
    {
        public string ackid { get; set; }
        public string type { get; set; }
        public List<EmpireException> exceptionList { get; set; } = new List<EmpireException>();
    }
}
