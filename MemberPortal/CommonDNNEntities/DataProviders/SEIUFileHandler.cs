using StagwellTech.SEIU.CommonCoreEntities.Handlers.Requests;
using StagwellTech.SEIU.CommonDNNEntities.Helpers;
using StagwellTech.SEIU.CommonEntities.DataModels.Portal.Document;
using StagwellTech.SEIU.CommonEntities.Document;
using StagwellTech.SEIU.CommonEntities.HttpClients;
using StagwellTech.SEIU.CommonEntities.Utils.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Twilio.Http;
using DocumentModel = StagwellTech.SEIU.CommonEntities.Document.Document;

namespace StagwellTech.SEIU.CommonDNNEntities.DataProviders
{
    public static class SEIUFileHandler
    {

        static DocumentClient _client;
        static DocumentClient client { get { if (_client == null) _client = new DocumentClient(Utilities.GetWebAPIUrls()["SEIU_API_documentsDomain"]); return _client; } }

        static EncryptionHelper encryptionHelper = new EncryptionHelper();

        private static string Token(HttpRequest request)
        {
            return request.Cookies.Get(".DOTNETNUKE").Value;
        }

        private static string APIEndpoint(HttpRequest request)
        {
            return Utilities.GetWebAPIUrls()["SEIU_API_documentsDomain"];
        }

        private static string GenerateTokenSignature(HttpRequest request, string id)
        {
            var cookie = Token(request);
            var tokenEncryptionObject = EncryptionHelper.GenerateTokenEncryptionObject("SEIU " + cookie, id);
            var encrypted = encryptionHelper.EncryptObject(tokenEncryptionObject);
            return "nonce=" + tokenEncryptionObject.Nonce + "&token=" + encrypted.data + "&signature=" + encrypted.signature;
        }

        public static async Task<DocumentModel> UploadFileAsync(Stream inputStream, string fileName, HttpRequest request, NewDocumentRequest ndr)
        {
            return await client.Upload(inputStream, fileName, Token(request), ndr);
        }

        public static string GetDownloadUrl(string documentId, HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/{documentId}/download?{GenerateTokenSignature(request, documentId)}";
        }

        public static string GetFADTileIconUrl(int id, HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/fad-tile-icon/{id}?{GenerateTokenSignature(request, id.ToString())}";
        }

        public static string GetFADProviderImageUrl(int id, HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/provider-photo/{id}?{GenerateTokenSignature(request, id.ToString())}";
        }

        public static string GetAppleWalletPass(HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/download-pkpass?{GenerateTokenSignature(request, "pkpass")}";
        }
        public static string GetAppleWalletEmpirePass(HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/download-pkpass-empire?{ GenerateTokenSignature(request, "pkpass")}";
        }
        public static string GetAppleWalletDentalPass(HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/download-pkpass-dental?{ GenerateTokenSignature(request, "pkpass")}";
        }
        public static string GetAppleWalletVisionPass(HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/download-pkpass-vision?{ GenerateTokenSignature(request, "pkpass")}";
        }
        public static string GetAppleWalletPrescriptionPass(HttpRequest request)
        {
            return $"{APIEndpoint(request)}/api/documents/download-pkpass-prescription?{ GenerateTokenSignature(request, "pkpass")}";
        }

        public static string SaveToGooglePay(HttpRequest request)
        {
            var url = $"{APIEndpoint(request)}/api/documents/get-googlePass?{GenerateTokenSignature(request, "googlePass")}";
            return url;
        }

        public static string SaveToGooglePayEmpire(HttpRequest request)
        {
            var url = $"{APIEndpoint(request)}/api/documents/get-googlePass-empire?{GenerateTokenSignature(request, "googlePass")}";
            return url;
        }

        public static string SaveToGooglePayDental(HttpRequest request)
        {
            var url = $"{APIEndpoint(request)}/api/documents/get-googlePass-dental?{GenerateTokenSignature(request, "googlePass")}";
            return url;
        }

        public static string SaveToGooglePayVision(HttpRequest request)
        {
            var url = $"{APIEndpoint(request)}/api/documents/get-googlePass-vision?{GenerateTokenSignature(request, "googlePass")}";
            return url;
        }

        public static string SaveToGooglePayPrescription(HttpRequest request)
        {
            var url = $"{APIEndpoint(request)}/api/documents/get-googlePass-prescription?{GenerateTokenSignature(request, "googlePass")}";
            return url;
        }

        public async static Task<PortalEnrollmentFormDocumentResponse> UploadEnrollmentDocument(UploadEnrollmentDocumentHandlerRequest documentRequest, HttpRequest request)
        {
            var doc = await client.UploadPortalEnrollmentDocument(documentRequest, Token(request));
            return doc;
        }
    }
}
