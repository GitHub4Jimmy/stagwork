using Newtonsoft.Json;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using StagwellTech.SEIU.CommonEntities.ReadOnly.MPFundJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public class MemberTourHelper
    {
        public static string HandleRoles(String[] UserRoles,bool CanViewUnionSection, List<MPFundJob> ActiveJobs, bool showWelcomeTour)
        {
            List<string> tourData = new List<string>();
            foreach(var role in UserRoles)
            {
                if (role.Equals(EntitlementTypes.EligibilityTypes.Health))
                {
                    tourData.Add(EntitlementTypes.MemberTourData.Health);
                }
                else if (role.Equals(EntitlementTypes.EligibilityTypes.Training))
                {
                    tourData.Add(EntitlementTypes.MemberTourData.Training);
                }
                else if (role.Equals(EntitlementTypes.EligibilityTypes.Dependents))
                {
                    tourData.Add(EntitlementTypes.MemberTourData.FamilyBenefits);
                }
                else if ((role.Equals(EntitlementTypes.EligibilityTypes.Pension401K))||(role.Equals(EntitlementTypes.EligibilityTypes.Pension)))
                {
                    if(!tourData.Any(item => item== EntitlementTypes.MemberTourData.Retirement))
                    {
                        tourData.Add(EntitlementTypes.MemberTourData.Retirement);
                    }
                }
                else if (role.Equals(EntitlementTypes.EligibilityTypes.REGISTERED_USERS)) 
                {
                    tourData.Add(EntitlementTypes.MemberTourData.Calendar);
                    tourData.Add(EntitlementTypes.MemberTourData.Profile);
                    tourData.Add(EntitlementTypes.MemberTourData.Language);
                    tourData.Add(EntitlementTypes.MemberTourData.Document);
                    tourData.Add(EntitlementTypes.MemberTourData.Help);
                    tourData.Add(EntitlementTypes.MemberTourData.Menu);
                }
            }
            if (CanViewUnionSection && ActiveJobs != null)
            {
                tourData.Add(EntitlementTypes.MemberTourData.Contract);
            }

            ConvertTourData convertTourData = new ConvertTourData
            {
                welcomeTourData = tourData,
                autoShowWelcomeTour = showWelcomeTour
            };
            return JsonConvert.SerializeObject(convertTourData);
        }
    }
    public class ConvertTourData
    {
        public List<string> welcomeTourData { get; set; }
        public bool autoShowWelcomeTour { get; set; }
    }
}
