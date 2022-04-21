using StagwellTech.SEIU.CommonEntities.DBO.MedProviders;
using StagwellTech.SEIU.CommonEntities.Filters;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using System.Collections;
using StagwellTech.SEIU.CommonDNNEntities.DataProviders;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public static class FADHelper
    {
        public static void HandleTip(ProviderFilter filter, List<MPDisplaySpecialty> SpecialistDisplaySpecialties, MPPerson person, List<MPDisplaySpecialty> DentistDisplaySpecialties,HtmlGenericControl divTip,Literal ltrTipText,Hashtable settings)
        {
            //Hardcoded rules from SEIU32BJ-622
            divTip.Visible = false;

            var dps = filter?.DisplayProviderSpecs;
            if (dps == null)
            {
                return;
            }
            //If Ophthalmologist; Rule: Display on results page when member searches for Ophthalmologist in Guided search
            if (dps.Contains(58))
            {
                if (settings["FADOpen_Tips_Ophthalmologist"] != null)
                {
                    divTip.Visible = true;
                    ltrTipText.Text = settings["FADOpen_Tips_Ophthalmologist"]?.ToString();
                }
            }
            //If Specialist; Rule: Display on results page when member searches for Specialist in Guided search
            else if (SpecialistDisplaySpecialties.Where(dds => dps.Contains(dds.Id)).Any())
            {
                if (settings["FADOpen_Tips_Specialist"] != null)
                {
                    divTip.Visible = true;
                    ltrTipText.Text = settings["FADOpen_Tips_Specialist"]?.ToString();
                }
            }
            //If Dental Center patient search for Dentists
            else if (person.IsDentalCenterPatient && DentistDisplaySpecialties.Where(dds => dps.Contains(dds.Id)).Any())
            {
                if (settings["FADOpen_Tips_DentalCenter"] != null)
                {
                    divTip.Visible = true;
                    ltrTipText.Text = settings["FADOpen_Tips_DentalCenter"]?.ToString();
                }
            }
        }
        public static async Task<Tuple<List<MPDisplaySpecialty>,List<MPDisplaySpecialty>>> LoadTipsData(SEIUDNNContext context)
        {
            //Hardcoded dentist tileId
            var dentistTask = context.GetDentistDisplaySpecialties();
            //Hardcoded specialist tileId
            var specialistTask = context.GetSpecialistDisplaySpecialties();
            await Task.WhenAll(dentistTask, specialistTask);
            return new Tuple<List<MPDisplaySpecialty>, List<MPDisplaySpecialty>>(dentistTask.Result, specialistTask.Result);
        }
    }
}
