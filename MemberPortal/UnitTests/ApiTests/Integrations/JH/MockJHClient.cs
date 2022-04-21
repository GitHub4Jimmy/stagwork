using Newtonsoft.Json;
using StagwellTech.SEIU.CommonEntities.JohnHancock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static StagwellTech.SEIU.CommonEntities.JohnHancock.JHApiClient;

namespace ApiTests.Services.JH
{
    class MockJHClient : IJHApiClient
    {
        public JHAccessToken GetAccessToken(int? timeout = null)
        {
            return new JHAccessToken();
        }

        public JHParticipantPlansResponse GetParticipantPlans(string ssn)
        {
            return new JHParticipantPlansResponse();
        }

        public JHParticipantPlansResponse GetParticipantPlanSummary(string ssn)
        {
            var response = new JHParticipantPlansResponse();
            response.id = "1";
            response.asOfDate = DateTime.Now;
            response.participantPlans = new List<ParticipantPlan> {
                new ParticipantPlan()
                {
                    accountId = "1",
                    employeeNumber = "1",
                    plan = new Plan() { }
                }
            };
            return response;
        }
    }
}
