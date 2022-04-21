using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.SEIU.CommonEntities.Portal.JH;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests1.Builders
{
    public class TestDataBuilder
    {
        private SeiuContext Context;
        public TestDataBuilder(SeiuContext context)
        {
            Context = context;
        }

        public TestDataBuilder BuildJHSummaries()
        {
            Context.AddRange(
                new List<JHParticipantPlanSummary>
                {
                    new JHParticipantPlanSummary { id = "0" }
                }
            );
            return this;
        }

        public TestDataBuilder BuildMPSSN(long personId, string ssn)
        {
            Context.AddRange(
                new List<MPSSN>
                {
                    new MPSSN { 
                        PersonId = personId,
                        UniqueSSN = ssn,
                        SSNId = ssn
                    }
                }
            );
            return this;
        }
        
        public void Build()
        {
            this.Context.SaveChanges();
        }
    }
}
