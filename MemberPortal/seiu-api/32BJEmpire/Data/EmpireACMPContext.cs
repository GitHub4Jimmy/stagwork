using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StagwellTech.SEIU.CommonEntities.DataModels.Empire;

namespace SEIU32BJEmpire.Data
{
    public class EmpireACMPContext : DbContext
    {
        public EmpireACMPContext(DbContextOptions<EmpireACMPContext> options)
            : base(options)
        {
        }
        public DbSet<EmpireACMPEncryptedRequest> EmpireACMPEncryptedRequests { get; set; }
        public DbSet<EmpireACMPRequest> EmpireACMPRequests { get; set; }
        public DbSet<Authorizationdetails> Authorizationdetailss { get; set; }
        public DbSet<PlaceOfService> PlaceOfServices { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberAdditionalProperty> MemberAdditionalPropertys { get; set; }
        public DbSet<FundActionNeeded> FundActionNeededs { get; set; }
        public DbSet<Diagnosis> Diagnosiss { get; set; }
        public DbSet<DiagnosisCode> DiagnosisCodes { get; set; }
        public DbSet<ServiceList> ServiceLists { get; set; }
        public DbSet<ProcedureCode> ProcedureCodes { get; set; }
        public DbSet<PlaceOfServiceCode> PlaceOfServiceCodes { get; set; }
        public DbSet<ServiceDecision> ServiceDecisions { get; set; }
        public DbSet<DecisionCode> DecisionCodes { get; set; }
        public DbSet<DecisionReasonCode> DecisionReasonCodes { get; set; }
        public DbSet<ServicingProvider> ServicingProviders { get; set; }
        public DbSet<ServicingProviderAdditionalProperty> ServicingProviderAdditionalPropertys { get; set; }
        public DbSet<ServiceListAdditionalProperty> ServiceListAdditionalPropertys { get; set; }
        public DbSet<LengthOfStay> LengthOfStays { get; set; }
        public DbSet<LevelOfCare> LevelOfCares { get; set; }
        public DbSet<LOSDecision> LOSDecisions { get; set; }
        public DbSet<LOSDecisionCode> LOSDecisionCodes { get; set; }
        public DbSet<LOSDecisionReasonCode> LOSDecisionReasonCodes { get; set; }
        public DbSet<LOSDecisionLevelofCare> LOSDecisionLevelofCares { get; set; }
        public DbSet<LengthOfStayAdditionalProperty> LengthOfStayAdditionalPropertys { get; set; }
        public DbSet<ProviderList> ProviderLists { get; set; }
        public DbSet<ProviderAddress> ProviderAddresses { get; set; }
        public DbSet<ServicingProviderAddress> ServicingProviderAddresses { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderListAdditionalProperty> ProviderListAdditionalPropertys { get; set; }
        public DbSet<SEIUEmpireResponse> SEIUEmpireResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
