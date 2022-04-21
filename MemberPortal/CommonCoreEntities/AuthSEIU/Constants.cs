using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.AuthSEIU
{
    public static class Constants
    {
        public const string SEIUScheme = "SEIU";
        public const string SEIUPolicy = "SEIU";


        public const string SEIUDocumentsScheme = "SEIUDOCS";
        public const string SEIUDocumentsPolicy = "SEIUDOCS";

        public const string DocumentIdOrFileName = "DocumentIdOrFileName";
        public const string SignatureUsed = "SignatureUsed";

        public static string PrepareCookie(string cookie)
        {
            return SEIUScheme + " " + cookie;
        }
    }
}
