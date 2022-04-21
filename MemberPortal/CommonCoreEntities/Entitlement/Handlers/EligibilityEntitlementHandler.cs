using StagwellTech.SEIU.API.MPPersonAPI;
using StagwellTech.SEIU.CommonCoreEntities.Services;
using StagwellTech.SEIU.CommonCoreEntities.Services.MPPersonAPI;
using StagwellTech.SEIU.CommonCoreEntities.Services.UserSettingsAPI;
using StagwellTech.SEIU.CommonEntities.BusClients;
using StagwellTech.SEIU.CommonEntities.DependencyHelper;
using StagwellTech.SEIU.CommonEntities.Entitlement;
using StagwellTech.SEIU.CommonEntities.ReadOnly.MPEligibility;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using StagwellTech.SEIU.CommonEntities.Utils;
using StagwellTech.SirenSDK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Entitlement.Handlers
{
    [SingletonDependency]
    class EligibilityEntitlementHandler : BaseEntitlementHandler
    {
        protected readonly PersonSectionEligibilitService personSectionEligibilitService;
        protected readonly MPPersonService mpPersonService;
        protected readonly UserSettingsService userSettingsService;
        protected readonly MPPenPrjService pensionService;

        public EligibilityEntitlementHandler(
            PersonSectionEligibilitService personSectionEligibilitService,
            MPPersonService mpPersonService,
            UserSettingsService userSettingsService,
            MPPenPrjService pensionService
            ) : base(EntitlementTypes.EligibilityTypes.DOMAIN)
        {
            this.personSectionEligibilitService = personSectionEligibilitService;
            this.mpPersonService = mpPersonService;
            this.userSettingsService = userSettingsService;
            this.pensionService = pensionService;
        }

        public override IList<EntitlementPermission> handleRequest(EntitlementRequest eRequest, IList<string> entitlements)
        {
            var task = Task.Run(() => _handleRequest(eRequest, entitlements));
            task.Wait(); // Not sure if needed
            return task.Result;
        }

        public async Task<IList<EntitlementPermission>> _handleRequest(EntitlementRequest eRequest, IList<string> entitlements)
        {
            var sw = Stopwatch.StartNew();

            int userId = eRequest.UserId;
            long startingAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var sections = await personSectionEligibilitService.GetAllByUserId(userId);

            long endingAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine("\r\n\nGetting data from DB => " + (endingAt - startingAt) + "\r\n\n");

            List<EntitlementPermission> results = new List<EntitlementPermission>();

            foreach (string entitlement in entitlements)
            {
                var parts = entitlement.Split(".");
                var domain = parts[0];
                var permission = parts[1];

                switch (permission)
                {
                    case EntitlementTypes.EligibilityTypes.DENTAL:

                        var isNotDentalCenterPatient = sections.Any(s => String.IsNullOrWhiteSpace(s.PATIENT_ID) == true);
                        var dental = IsDental(sections);

                        if (dental.HasValue() && isNotDentalCenterPatient)
                        {
                            var startDate = GetStartDate(dental);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.DENTAL_CENTER_PATIENT:

                        var isDentalCenterPatient = sections.Any(s => String.IsNullOrWhiteSpace(s.PATIENT_ID) == false);
                        var dentalCenter = IsDental(sections);

                        if (dentalCenter.HasValue() && isDentalCenterPatient)
                        {
                            var startDate = GetStartDate(dentalCenter);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.VISION:

                        var vision = sections.Where(s => String.IsNullOrWhiteSpace(s.Vision) == false);

                        if (vision.HasValue())
                        {
                            var startDate = GetStartDate(vision);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.HEALTH:

                        var health = sections.Where(section =>
                                    section.PlanTypeName.IsNotNullAndEquals("Health") &&
                                    section.MedicalHospital.IsNotNullAndEquals("Empire")
                                    );

                        if (health.HasValue())
                        {
                            var startDate = GetStartDate(health);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.LEAGAL:

                        var legal = sections.Where(s => s.PlanTypeName.IsNotNullAndEquals("Legal"));

                        if (legal.HasValue())
                        {
                            var startDate = GetStartDate(legal);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.PENSION:

                        var pension = sections.Where(s => s.PlanTypeName.IsNotNullAndEquals("Pension") || s.SHOW_PENSION.GetValueOrDefault(0) > 0);
                        var pensionStartDate = sections.Where(s => s.PlanTypeName.IsNotNullAndEquals("Pension"));

                        if (pension.HasValue())
                        {
                            var startDate = GetStartDate(pensionStartDate);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.K401:
                        // TODO: check for 401K
                        var pension401k = sections.Where(s => (s.PlanTypeName.IsNotNullAndEquals("SRSP") && s.FundName.IsNotNullAndEquals("401K (SRSP)")) || s.SHOW_SRSP.GetValueOrDefault(0) > 0);
                        var pension401kStartDate = sections.Where(s => s.PlanTypeName.IsNotNullAndEquals("SRSP") && s.FundName.IsNotNullAndEquals("401K (SRSP)"));

                        if (pension401k.HasValue())
                        {
                            var startDate = GetStartDate(pension401kStartDate);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.PROFITSHARE: //Added by request on this ticket SEIUTM2020-57, comment on 13/03/21
                        var profitShare = sections.Where(s => s.PlanCode.IsNotNullAndEquals("PS"));

                        if (profitShare.HasValue())
                        {
                            var startDate = GetStartDate(profitShare);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.SCHOLARSHIP:

                        var scholarship = sections.Where(s => s.PlanTypeName.IsNotNullAndEquals("Scholarship"));

                        if (scholarship.HasValue())
                        {
                            var startDate = GetStartDate(scholarship);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.TRAINING:

                        var training = sections.Where(s => s.PlanTypeName.IsNotNullAndEquals("Training"));

                        if (training.HasValue())
                        {
                            var startDate = GetStartDate(training);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.PRESCRIPTION:

                        var prescription = sections.Where(s => s.Prescription.IsNotNullAndEquals("OptumRx"));

                        if (prescription.HasValue())
                        {
                            var startDate = GetStartDate(prescription);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }

                        break;
                    case EntitlementTypes.EligibilityTypes.KAISER_SUBURBAN:

                        var kaiserSuburban = sections.Where(s => s.PlanCode.IsNotNullAndEquals("TU"));

                        if (kaiserSuburban.HasValue())
                        {
                            var startDate = GetStartDate(kaiserSuburban);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.KAISER_TRI_STATE:

                        var kaiserTriState = sections.Where(s => s.PlanCode.IsNotNullAndEquals("TK"));

                        if (kaiserTriState.HasValue())
                        {
                            var startDate = GetStartDate(kaiserTriState);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;

                    case EntitlementTypes.EligibilityTypes.LIFE_INSURANCE:

                        var lifeInsurance = sections.Where(s => s.PlanTypeName.IsNotNullAndEquals("Life"));

                        if (lifeInsurance.HasValue())
                        {
                            var startDate = GetStartDate(lifeInsurance);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;

                    case EntitlementTypes.EligibilityTypes.COBRA:

                        var cobra = sections.Where(s => s.SHOW_COBRA.GetValueOrDefault(false));

                        if (cobra.HasValue())
                        {
                            var startDate = GetStartDate(cobra);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.DEPENDENTS:

                        var deps = sections.Where(s => s.DependentCount > 0 && s.JhWithBeneficiaryCount > 0);

                        if (deps.HasValue())
                        {
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = DateTime.Now
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.VISION_IS_DAVIS:

                        //Case insensitive comparison
                        var visionIsDavis = sections.Where(s => s.Vision.IsNotNullAndEquals("Davis Vision"));

                        if (visionIsDavis.HasValue())
                        {
                            var startDate = GetStartDate(visionIsDavis);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.MEDICAL_IS_EMPIRE:

                        //Case insensitive comparison
                        var medicalIsEmpire = sections.Where(s => s.MedicalHospital.IsNotNullAndEquals("Empire"));

                        if (medicalIsEmpire.HasValue())
                        {
                            var startDate = GetStartDate(medicalIsEmpire);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.UNION_SECTION:
                        //SEIUTM2020-134
                        //Show Union section if Security_type = N (null) And Person_type=Member
                        var unionSection = sections
                            .Where(s =>
                                (s.SecurityType is null || s.SecurityType.IsNotNullAndEquals("N"))
                                && s.PersonType.IsNotNullAndEquals("MEMBER")
                            );

                        if (unionSection.HasValue())
                        {
                            var startDate = GetStartDate(unionSection);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.LEGAL_LEGAL:
                        //SEIUTM2020-199
                        //Add 3 new eligibilities for each LegalPlan (LP, NL and MPL1)
                        var legalLegal = sections.Where(s => s.PlanCode.IsNotNullAndEquals("LP"));

                        if (legalLegal.HasValue())
                        {
                            var startDate = GetStartDate(legalLegal);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.LEGAL_NORTH:
                        //SEIUTM2020-199
                        //Add 3 new eligibilities for each LegalPlan (LP, NL and MPL1)
                        var legalNorth = sections.Where(s => s.PlanCode.IsNotNullAndEquals("NL"));

                        if (legalNorth.HasValue())
                        {
                            var startDate = GetStartDate(legalNorth);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    case EntitlementTypes.EligibilityTypes.LEGAL_RETIREE:
                        //SEIUTM2020-199
                        //Add 3 new eligibilities for each LegalPlan (LP, NL and MPL1)
                        var legalRetiree = sections.Where(s => s.PlanCode.IsNotNullAndEquals("MPL1"));

                        if (legalRetiree.HasValue())
                        {
                            var startDate = GetStartDate(legalRetiree);
                            results.Add(new EntitlementPermission()
                            {
                                Entitlement = entitlement,
                                Permission = EntitlementPermission.PermissionType.View,
                                StartDate = startDate
                            });
                        }
                        break;
                    default:

                        results.Add(new EntitlementPermission()
                        {
                            Entitlement = entitlement,
                            Permission = EntitlementPermission.PermissionType.None
                        });

                        break;
                }

            }

            sw.Stop();
            SirenFactory.SirenDNN.Provider.LogMetric(new Metric($"entitlements-request", sw.ElapsedMilliseconds));

            return results;
        }

        private static IEnumerable<PersonSectionEligibility> IsDental(IEnumerable<PersonSectionEligibility> sections)
        {
            return sections.Where(section => section.Dental == true && (
                    section.AccessToDeltaDentalDeltacare == true ||
                    section.AccessToDeltaDentalNyselect == true ||
                    section.AccessToDeltaDentalPpo == true
                    )
            );
        }

        DateTime? GetStartDate(IEnumerable<PersonSectionEligibility> sections)
        {
            return sections?.OrderBy(s => s.StartDate)?.FirstOrDefault()?.StartDate;
        }
    }
}
