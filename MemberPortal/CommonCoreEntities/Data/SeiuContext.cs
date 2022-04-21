
using Microsoft.EntityFrameworkCore;
using StagwellTech.SEIU.CommonEntities.Alert;
using StagwellTech.SEIU.CommonEntities.CRM;
using StagwellTech.SEIU.CommonEntities.User;
using StagwellTech.SEIU.CommonEntities.Member;
using StagwellTech.SEIU.CommonEntities.Employer;
using StagwellTech.SEIU.CommonEntities.Document;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReadOnlyAddress = StagwellTech.SEIU.CommonEntities.ReadOnly.Address.Address;
using StagwellTech.SEIU.CommonEntities.Delta.Address;
using ReadOnlyPerson = StagwellTech.SEIU.CommonEntities.ReadOnly.Person.Person;
using ReadOnlyMember = StagwellTech.SEIU.CommonEntities.ReadOnly.Member.Member;
using UnionContract = StagwellTech.SEIU.CommonEntities.UnionContract.UnionContract;
using UnionBenefit = StagwellTech.SEIU.CommonEntities.UnionBenefit.UnionBenefit;
using StagwellTech.SEIU.CommonEntities.Portal.EventRSVP;
using StagwellTech.SEIU.CommonEntities;
using StagwellTech.SEIU.CommonEntities.Delta.Person;
using StagwellTech.SEIU.CommonEntities.Delta.Member;
using StagwellTech.SEIU.CommonEntities.DNN;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Person;
using StagwellTech.SEIU.CommonEntities.Portal.Registration;
using StagwellTech.SEIU.CommonEntities.Territorial;
using StagwellTech.SEIU.CommonEntities.Med;
using StagwellTech.SEIU.CommonEntities.RefLang;
using StagwellTech.SEIU.CommonEntities.ProviderLoc;
using StagwellTech.SEIU.CommonEntities.ProviderLang;
using StagwellTech.SEIU.CommonEntities.ProviderEdu;
using StagwellTech.SEIU.CommonEntities.RefProviderTp;
using StagwellTech.SEIU.CommonEntities.ProviderSpe;
using StagwellTech.SEIU.CommonEntities.RefNet;
using ReadOnlyEvent = StagwellTech.SEIU.CommonEntities.ReadOnly.Event.Event;
using EventDate = StagwellTech.SEIU.CommonEntities.ReadOnly.Event.EventDate;
using PortalEvent = StagwellTech.SEIU.CommonEntities.Portal.Event.Event;
using System;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO.Translation;
using StagwellTech.SEIU.CommonEntities.ReadOnly.MPEligibility;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Med;
using StagwellTech.SEIU.CommonEntities.MedProviders;
using StagwellTech.SEIU.CommonEntities.ReadOnly.MedProviders;
using StagwellTech.SEIU.CommonEntities.Portal.MedProviders;
using StagwellTech.SEIU.CommonEntities.DBO.Copay;
using StagwellTech.SEIU.CommonEntities.DBO.MedProviders;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Copay;
using StagwellTech.SEIU.CommonEntities.DBO.Alert;
using StagwellTech.SEIU.CommonEntities.Portal.Alert;
using StagwellTech.SEIU.CommonEntities.DBO.KnowledgeArticles;
using StagwellTech.SEIU.CommonEntities.DBO.Category;
using StagwellTech.SEIU.CommonEntities.DBO.MPPlan;
using StagwellTech.SEIU.CommonEntities.DBO.Person.Pension;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Dependent;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Event;
using StagwellTech.SEIU.CommonEntities.UnionAdvantage;
using StagwellTech.SEIU.CommonEntities.ReadOnly.MPFundJob;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Member;
using StagwellTech.SEIU.CommonEntities.Portal.Document;
using StagwellTech.SEIU.CommonEntities.Portal.JH;
using StagwellTech.SEIU.CommonEntities.DBO.Address;
using StagwellTech.SEIU.CommonEntities.ReadOnly.Advert;
using StagwellTech.SEIU.CommonEntities.Portal.Advert;
using StagwellTech.SEIU.CommonEntities.DBO.Person;
using StagwellTech.SEIU.CommonEntities.ReadOnly.DentalWaitlist;
using StagwellTech.SEIU.CommonEntities.DBO.MPHealthProgram;
using StagwellTech.SEIU.CommonEntities.DataModels.Portal;
using System.Data.Entity.Infrastructure;
using StagwellTech.SEIU.CommonEntities.Utils;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO.SuffixAttribute;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO._Contact;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO.StringMap;

namespace StagwellTech.SEIU.CommonCoreEntities.Data
{
    public class SeiuDNNContext : DbContext
    {
        // Rebuild
        public SeiuDNNContext(DbContextOptions<SeiuDNNContext> options)
            : base(options)
        {
        }

        public DbSet<DNNAuthCookie> DNNAuthCookies { get; set; }
        public DbSet<DNNUser> DNNUsers { get; set; }
        public DbSet<ASPNETUser> ASPNETUsers { get; set; }
        public DbSet<ASPNETMembership> ASPNETMemberships { get; set; }
        public DbSet<DNNUserRole> DNNUserRoles { get; set; }
        public DbSet<DNNUserAuthentication> DNNUserAuthentications { get; set; }

    }


    public class SeiuContext : DbContext
    {
        public SeiuContext(DbContextOptions<SeiuContext> options)
            : base(options)
        {
        }

        public DbSet<JHParticipantPlanSummary> JHParticipantPlanSummaries { get; set; }
        public DbSet<CopayRule> CopayRules { get; set; }
        public DbSet<PersonProviderCopay> PersonProviderCopays { get; set; }
        public DbSet<PersonProviderCopayBySpecialties> PersonProviderCopaysBySpecialties { get; set; }
        public DbSet<MPDependentPerson> MPDependentPersons { get; set; }
        public DbSet<RecommendedGuide> RecommendedGuides { get; set; }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<MPCategory> MPCategories { get; set; }
        public DbSet<MPAlert> MPAlerts { get; set; }
        public DbSet<MPAlertPerson> MPAlertPersons { get; set; }
        public DbSet<AlertPerson> AlertPersons { get; set; }
        public DbSet<AlertTargetType> AlertTargetTypes { get; set; }
        public DbSet<AlertType> AlertTypes { get; set; }

        public DbSet<Employer> Employers { get; set; }

        public DbSet<UpdateRequestLog> UpdateRequests { get; set; }

        public DbSet<CRMMessageRequest> CRMMessageRequests { get; set; }
        public DbSet<CRMMessageResponse> CRMMessageResponses { get; set; }
        public DbSet<CommunicationSetting> CommunicationSettings { get; set; }
        public DbSet<CommunicationSettingDelta> CommunicationSettingsDelta { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentCategory> DocumentCategories { get; set; }
        public DbSet<PortalDocument> PortalDocuments { get; set; }
        public DbSet<PersonDocument> PersonDocuments { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        /* PERSON */
        public DbSet<ReadOnlyPerson> Persons { get; set; }
        public DbSet<PersonDelta> PersonsDelta { get; set; }
        public DbSet<ContactInfo> ContactInfo { get; set; }
        public DbSet<ContactInfoDelta> ContactInfoDelta { get; set; }
        /* PERSON END */

        /* MPPERSON */
        public DbSet<MPPerson> MPPersons { get; set; }
        public DbSet<MPPersonExt> MPPersonExts { get; set; }
        public DbSet<MPPersonDelta> MPPersonsDelta { get; set; }
        public DbSet<MPPenPrj> MPPenPrjs { get; set; }
        public DbSet<MPSrspPrj> MPSrspPrjs { get; set; }
        public DbSet<MPSSN> MPSSNs { get; set; }
        public DbSet<MPSSNDelta> MPSSNDeltas { get; set; }
        public DbSet<PersonPCP> PersonPCPs { get; set; }
        public DbSet<DentalNotification> DentalNotifications { get; set; }
        public DbSet<MPHealthProgram> MPHealthPrograms { get; set; }
        /* MPPERSON END */
        public DbSet<Contact> Contacts { get; set; }
        /* ADDRESS */
        public DbSet<ReadOnlyAddress> Addresses { get; set; }
        public DbSet<AddressDelta> AddressesDelta { get; set; }
        /* ADDRESS END */

        /* MEMBER */
        public DbSet<ReadOnlyMember> Members { get; set; }
        public DbSet<MemberDelta> MembersDelta { get; set; }

        public DbSet<MemberJob> MemberJobs { get; set; }
        public DbSet<MemberProfile> MemberProfiles { get; set; }
        public DbSet<MemberAddress> MemberAddresses { get; set; }
        /* MEMBER END */

        /* COUNTRY */
        public DbSet<Country> Countries { get; set; }
        /* COUNTRY END */

        /* STATE */
        public DbSet<State> States { get; set; }
        /* STATE END */

        /* COUNTY */
        public DbSet<County> Counties { get; set; }
        /* COUNTY END */

        /* MEDICAL PRO CATEGORY */
        public DbSet<MedicalProCategory> MedicalProCategories { get; set; }
        /* MEDICAL PRO CATEGORY END */

        /* MEDICAL PRO CATEGORY SPECIALTY */
        public DbSet<MedicalProCategorySpecialty> MedicalProCategorySpecialties { get; set; }
        public DbSet<MPSpecialty> MPSpecialties { get; set; }
        public DbSet<MPSpecialtyMapping> MPSpecialtyMappings { get; set; }
        /* MEDICAL PRO CATEGORY SPECIALTY END */

        /* REF LANGUAGE */
        public DbSet<RefLanguage> RefLanguages { get; set; }
        public DbSet<MPRefLanguage> MPRefLanguages { get; set; }
        public DbSet<RegistrationLanguage> RegistrationLanguages { get; set; }

        /* REF LANGUAGE END */
        public DbSet<SuffixAttribute> SuffixAttributes { get; set; }

        /* PROVIDER LOCATION */
        public DbSet<ProviderLocation> ProviderLocations { get; set; }
        /* PROVIDER LOCATION END */

        public DbSet<SpecialtySearchTerm> SpecialtySearchTerms { get; set; }
        public DbSet<ProviderSearchTerm> ProviderSearchTerms { get; set; }
        public DbSet<MPDisplaySpecialty> MPDisplaySpecialties { get; set; }


        /* MP PROVIDER ADDRESS */
        public DbSet<MPProviderAddress> MPProviderAddresses { get; set; }
        public DbSet<MPProviderMessage> MPProviderMessages { get; set; }
        public DbSet<RefSpecialty> RefSpecialties { get; set; }
        public DbSet<SavedProviderAddress> SavedProviderAddresses { get; set; }
        /* MP PROVIDER ADDRESS END */

        /* PROVIDER LANGUAGE*/
        public DbSet<ProviderLanguage> ProviderLanguages { get; set; }
        /* PROVIDER LANGUAGE END */

        /* PROVIDER EDUCATION */
        public DbSet<ProviderEducation> ProviderEducations { get; set; }
        /* PROVIDER LOCATION END */

        /* REF PROVIDER TYPE */
        public DbSet<RefProviderType> RefProviderTypes { get; set; }
        /* REF PROVIDER TYPE END */

        /* PROVIDER SPECIALTY */
        public DbSet<ProviderSpecialty> ProviderSpecialties { get; set; }
        /* PROVIDER SPECIALTY END */

        /* REF NETWORK TYPE */
        public DbSet<RefNetwork> RefNetworks { get; set; }
        /* REF NETWORK TYPE END */

        /* UNION CONTRACT */
        public DbSet<UnionContract> UnionContracts { get; set; }
        public DbSet<UnionBenefit> UnionBenefits { get; set; }
        public DbSet<UnionAdvantage> UnionAdvantages { get; set; }
        /* UNION CONTRACT END */

        /* MP_FUND_JOB */
        public DbSet<MPFundJob> MPFundJobs { get; set; }
        /* MP_FUND_JOB END */

        /* MP_LOCAL_OFFICE */
        public DbSet<MPLocalOffice> MPLocalOffices { get; set; }
        /* MP_LOCAL_OFFICE END */

        /* MP_ELIGIBILITY */
        public DbSet<MPEligibility> MPEligibilities { get; set; }
        public DbSet<PersonSectionEligibility> PersonSectionEligibilities { get; set; }
        public DbSet<PersonPlanExternalLink> PersonPlanExternalLinks { get; set; }
        /* MP_ELIGIBILITY END */

        /* EVENT */
        public DbSet<PortalEvent> PortalEvents { get; set; }
        public DbSet<ReadOnlyEvent> ReadOnlyEvents { get; set; }
        public DbSet<EventRSVP> EventsRSVP { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventPerson> EventPersons { get; set; }
        /* EVENT END */

        /* MPPersonFivestar */
        public DbSet<MPPersonFivestar> MPPersonFivestars { get; set; }
        /* MPPersonFivestar END */

        /* MPPlan */
        public DbSet<MPPlan> MPPlans { get; set; }
        public DbSet<MPPlanType> MPPlanTypes { get; set; }
        public DbSet<MPExternalLinkByPlan> MPExternalLinkByPlans { get; set; }
        /* MPPlan END */

        public DbSet<RegistrationForm> RegistrationForms { get; set; }
        public DbSet<MPKnowledgeArticle> MPKnowledgeArticles { get; set; }

        public DbSet<AdvertType> AdvertTypes { get; set; }
        public DbSet<AdvertDisplay> AdvertsDisplay { get; set; }


        public DbSet<APIMessageRequestLog> APIMessageRequestLogs { get; set; }
        public DbSet<MemberDashboardDataLog> MemberDashboardDataLogs { get; set; }
        public DbSet<TranslationCache> TranslationCaches { get; set; }

        //Dependents Enrollment 
        public DbSet<GeneratedId> GeneratedIds { get; set; }
        public DbSet<PortalEnrollmentForm> PortalEnrollmentForms { get; set; }
        public DbSet<PortalEnrollmentFormDocument> PortalEnrollmentFormDocuments { get; set; }
        public DbSet<EnrollmentDocumentsByType> EnrollmentDocumentsByType { get; set; }
        public DbSet<DependentEnrollmentCrmForm> DependentsEnrollmentCrmForms { get; set; }
        public DbSet<DependentsEnrollmentCrmDocument> DependentsEnrollmentCrmDocuments { get; set; }
        public DbSet<StringMap> StringMaps { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<MPSpecialty>()
                .HasKey(mps => new { mps.MedicalCategoryId, mps.DisplaySpecialtyId });

            var converterStringDouble = new ValueConverter<string, decimal>(
                v => decimal.Parse(v),
                v => v.ToString()
            );

            /* PERSON */
            modelBuilder
                .Entity<ReadOnlyPerson>()
                .Property(e => e.SSN)
                .HasConversion(converterStringDouble);

            modelBuilder
                .Entity<PersonDelta>()
                .Property(e => e.SSN)
                .HasConversion(converterStringDouble);
            modelBuilder
                .Entity<MemberProfile>()
                .Property(e => e.MemberID)
                .HasConversion(converterStringDouble);


            modelBuilder
                .Entity<ReadOnlyEvent>()
                .HasMany<EventRSVP>(e => e.EventRSVP)
                .WithOne()
                .HasForeignKey(rsvp => rsvp.EventId)
                .HasPrincipalKey(e => e.EventId);

            modelBuilder
                .Entity<PortalEvent>()
                .HasMany<EventRSVP>(e => e.EventRSVP)
                .WithOne()
                .HasForeignKey(rsvp => rsvp.EventId)
                .HasPrincipalKey(e => e.EventId);

            /* MP PROVIDER ADDRESS */

            modelBuilder
                .Entity<MPProviderAddress>()
                .HasMany<MPProviderAddressDisplaySpecialityId>(mppa => mppa.DisplayProviderSpecs)
                .WithOne()
                .HasForeignKey(mppadsi => mppadsi.ProviderAddressId)
                .HasPrincipalKey(mpa => mpa.ProviderAddressId);

            modelBuilder
                .Entity<MPProviderAddress>()
                .HasMany<ProviderSpecialty>(pa => pa.ProviderSpecs)
                .WithOne()
                .HasForeignKey(ps => ps.ProviderAddressId)
                .HasPrincipalKey(pa => pa.ProviderAddressId);

            modelBuilder
                .Entity<MPProviderAddress>()
                .HasMany<ProviderLanguage>(pa => pa.Languages)
                .WithOne()
                .HasForeignKey(l => l.ProviderAddressId)
                .HasPrincipalKey(pa => pa.ProviderAddressId);

            modelBuilder
                .Entity<MPProviderAddress>()
                .HasMany<ProviderEducation>(pa => pa.Educations)
                .WithOne()
                .HasForeignKey(e => e.ProviderId)
                .HasPrincipalKey(pa => pa.ProviderId);

            modelBuilder
                .Entity<ProviderSpecialty>()
                .HasOne<RefSpecialty>(ps => ps.RefSpecialty)
                .WithMany()
                .HasForeignKey(ps => ps.SpecialtyId)
                .HasPrincipalKey(rs => rs.Id);

            modelBuilder
                .Entity<ProviderLanguage>()
                .HasOne<RefLanguage>(pl => pl.RefLanguage)
                .WithMany()
                .HasForeignKey(pl => pl.LanguageId)
                .HasPrincipalKey(pa => pa.Id);

            /* MP PROVIDER ADDRESS END */
            /* EVENT */


            modelBuilder
                .Entity<PortalEvent>()
                .Property(e => e.EventId)
                .HasConversion(v => new Guid(v), v => v.ToString().ToUpper());

            modelBuilder
                .Entity<ReadOnlyEvent>()
                .Property(e => e.EventId)
                .HasConversion(v => new Guid(v), v => v.ToString().ToUpper());

            modelBuilder
                .Entity<EventPerson>()
                .Property(ep => ep.EventId)
                .HasConversion(v => new Guid(v), v => v.ToString().ToUpper());

            /* EVENT END */


            modelBuilder
                .Entity<UnionContract>()
                .HasMany<UnionBenefit>(uc => uc.UnionBenefits)
                .WithOne()
                .HasForeignKey(ub => ub.ContractID)
                .HasPrincipalKey(uc => uc.ContractID);

            modelBuilder
                .Entity<UnionContract>()
                .HasOne<UnionAdvantage>(uc => uc.UnionAdvantage)
                .WithMany()
                .HasForeignKey(uc => uc.ContractID)
                .HasPrincipalKey(ua => ua.BargUnitId);
        }
    }

}