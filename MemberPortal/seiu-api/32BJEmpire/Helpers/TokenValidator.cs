using SEIU32BJEmpire.Data;
using StagwellTech.SEIU.CommonEntities.Utils.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIU32BJEmpire.Helpers
{
    public class TokenValidator
    {
        private readonly static string PRIVATE_KEY_NAME = "EMPIRE_ENCRYPTION_PRIVATE_KEY";
        private readonly static string PUBLIC_KEY_NAME = "EMPIRE_ENCRYPTION_PUBLIC_KEY";
        private readonly static string PrivateKey = Environment.GetEnvironmentVariable(PRIVATE_KEY_NAME);
        private readonly static string PublicKey = Environment.GetEnvironmentVariable(PUBLIC_KEY_NAME);

        public static bool Validate(string headerValue, out EmpirePostErrorResponse error)
        {
            List<EmpireException> exceptions = new List<EmpireException>();
            bool tokenValid = EncryptionHelper.TryValidateToken(headerValue, "Empire", "api", EncryptionHelper.GetRSAPublicKey(PublicKey));
            error = null;
            if (!tokenValid)
            {
                exceptions.Add(new EmpireException()
                {
                    code = "401",
                    message = "Unauthorized",
                    detail = "AuthorizationDetails are missing"
                });

                error = new EmpirePostErrorResponse()
                {
                    ackid = "",
                    type = "400",
                    exceptionList = exceptions
                };
            }

            return tokenValid;
        }
    }
}
