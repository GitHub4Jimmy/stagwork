using System.Collections.Generic;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;
using ITranslatable = TrainingFund.Shared.Interfaces.ITranslatable;
using TranslatableAttribute = TrainingFund.Shared.Attributes.TranslatableAttribute;

namespace TrainingFund.DNN.Integration.Extensions
{
    public static class TranslationExtensions
    {
        public static List<TranslationObjectDescriptor> GetTranslatable(this ITranslatable obj)
        {
            var list = StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation.TranslationExtensions.GetTranslationInputs<ITranslatable, TranslatableAttribute>(obj);

            return list;
        }
    }
}
