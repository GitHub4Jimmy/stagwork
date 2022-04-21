using System;
using System.Runtime.Serialization;

namespace StagwellTech.ServiceBusRPC.Entities
{
    [DataContract(Name = "APIMessageResponse")]
    public class APIMessageResponse : SerializableToJSON<APIMessageResponse>
    {
        public static readonly string RESPONSE_SUCCESS = "200";
        public static readonly string RESPONSE_404 = "404";
        public static readonly string RESPONSE_401 = "401";
        public static readonly string RESPONSE_400 = "400";
        public static readonly string RESPONSE_500 = "500";

        [DataMember(Name = "response_id")]
        public Guid ResponseId { get; set; }

        [DataMember(Name = "request_id")]
        public Guid RequestId { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "response")]
        public object Response { get; set; }

        [DataMember(Name = "response_type")]
        public string ResponseType { get; set; }


        // error status code, message - on prod ups, an error acourd only.
        public static APIMessageResponse SendResponse(APIMessageRequest apiRequest, string responseStatus, string responseType, string responseBody)
        {
            var response = new APIMessageResponse();

            response.ResponseId = Guid.NewGuid();

            if (apiRequest != null)
            {
                response.RequestId = apiRequest.RequestId;
            }

            response.Status = responseStatus;
            response.ResponseType = responseType;
            response.Response = responseBody;

            return response;
        }

        public static APIMessageResponse SendSuccessResponse(APIMessageRequest apiRequest, string responseType, string responseBody = null)
        {
            return SendResponse(apiRequest, RESPONSE_SUCCESS, responseType, responseBody);
        }

        public static APIMessageResponse Send404Response(APIMessageRequest apiRequest, string responseBody = null)
        {
            // TODO: if production - no or standard message - "An error occurred"
            return SendResponse(apiRequest, RESPONSE_404, "ERROR", responseBody);
        }

        public static APIMessageResponse Send401Response(APIMessageRequest apiRequest, string responseBody = null)
        {
            // TODO: if production - no or standard message - "An error occurred"
            return SendResponse(apiRequest, RESPONSE_401, "ERROR", responseBody);
        }

        public static APIMessageResponse Send400Response(APIMessageRequest apiRequest, string responseBody = null)
        {
            // TODO: if production - no or standard message - "An error occurred"
            return SendResponse(apiRequest, RESPONSE_400, "ERROR", responseBody);
        }

        public static APIMessageResponse Send500Response(APIMessageRequest apiRequest, string responseBody = null)
        {
            // TODO: if production - no or standard message - "An error occurred"
            return SendResponse(apiRequest, RESPONSE_500, "ERROR", responseBody);
        }
    }
}
