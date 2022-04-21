using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrainingFund.DNN.Integration.Helpers
{

    public class DashboardHelper
    {
        public static string CONTEXT_MEMBER_DASHBOARD_POPUP = "MemberDashboardPopup";
        
        public static string EVENT_TRAINING_FUND_KEY = "training-fund-date";
        public static string EVENT_SCHOLARSHIP_KEY = "scholarship-application";
        public static string EVENT_TRAINING_FUND_ID_REGEX = @"enrollment\-(?<id>\d+)";

        public static bool AddTrainingPopup(IDictionary items)
        {
            bool addTrainingPopup = !items.Contains(CONTEXT_MEMBER_DASHBOARD_POPUP);

            if (addTrainingPopup)
            {
                items.Add(CONTEXT_MEMBER_DASHBOARD_POPUP, true);
            }

            return addTrainingPopup;
        }

        public static string GetEnrollmentIdFromEventId(string eventId)
        {
            Regex rgx = new Regex(EVENT_TRAINING_FUND_ID_REGEX);

            try
            {
                Match match = rgx.Match(eventId);
                if (match.Success)
                {
                    var group = match.Groups["id"];
                    if (group != null)
                    {
                        return group.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return null;

        }
    }
}
