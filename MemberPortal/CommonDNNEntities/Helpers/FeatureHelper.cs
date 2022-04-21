using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetNuke.Entities.Content.Taxonomy;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public class FeatureHelper
    {

        private const string FEATURES_VOCABULARY = "FEATURES";
        private const string ON = "ON";
        public enum Feature
        {
            PageTranslation,
            OmniChannel,
            PageTranslationLogin,
            Simulation,
            SimulationEmails,
            EDelivery
        }

        public static bool IsFeatureViewable(Feature feature)
        {
            var term = GetTerm(feature);

            if (term != null
                && term.Description.ToUpper() == ON)
            {
                return true;
            }

            return false;
        }

        public static string GetFeatureDescription(Feature feature)
        {
            var term = GetTerm(feature);

            if (term != null)
            {
                return term.Description;
            }

            return null;
        }

        public static List<string> GetSimulationEmails()
        {
            var term = GetTerm(Feature.SimulationEmails);

            if (term != null)
            {
                var removeNewLines = Regex.Replace(term.Description, @"\s+", "");
                return removeNewLines.Split(',').ToList();
            }

            return null;
        }

        private static Term GetTerm(Feature feature)
        {
            var terms = GetFeatureTerms();

            if (terms != null)
            {
                return terms.FirstOrDefault(t => t.Name == feature.ToString());
            }

            return null;
        }

        private static List<Term> GetFeatureTerms()
        {
            var controller = new VocabularyController();

            var vocabularies = controller.GetVocabularies();

            var vocabulary = vocabularies.FirstOrDefault(v => v.Name == FEATURES_VOCABULARY);

            if(vocabulary != null)
            {
                return vocabulary.Terms;
            }

            return null;
        }

        public static bool ShowPageTranslation(int userId)
        {
            if (IsFeatureViewable(Feature.PageTranslation) && userId > 0)
            {
                return true;
            }

            return false;
        }
    }
}