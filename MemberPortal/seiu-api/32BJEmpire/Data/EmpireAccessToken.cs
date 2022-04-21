using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIU32BJEmpire.Data
{
    public class EmpireAccessToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("issued_at")]
        public string IssuedAt { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
/*
 * {
  "token_type": "BearerToken",
  "issued_at": "1631186149037",
  "client_id": "qkUsMMXGbwQChHDEcfKH6VZkyUbZzv4O",
  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IlNpZ25pbmdDZXJ0IiwieDV0IjoiSW45TGd4a21CNzlrZ2h2VmYydGpVeFI2bU5NIiwicGkuYXRtIjoiZGZwbCIsInR5cCI6IkpXVCJ9.eyJzY29wZSI6InB1YmxpYyIsImNsaWVudF9pZCI6IjFkZTZlYWE0ZDU0ZDRhMjg5ZjRjNTk5ZWU0NjkyMmJlIiwiaXNzIjoiaHR0cHM6Ly9zZWN1cmUtZmVkLnVhdC5hbnRoZW1pbmMuY29tIiwiYXVkIjoiaHR0cHM6Ly9zZWN1cmUtZmVkLnVhdC5hbnRoZW1pbmMuY29tIiwianRpIjoicmZZVHIyYW95TExjTEFSckJNY0pPc0pQVjdGd2h1b0kiLCJleHAiOjE2MzExODcwNDh9.XSaud94MrO_fameEFPz49X-UjBPsneGkbKcsmU0tlNowtgmCgWSpD2cPWAuTvEZ_PSlizeRHg1F34rSRQ421-FjWsb0SqVw_sdR766WPRDttRj_Y46yGY3o-D3YeYuCqmGaqAxgU9gVMS7M9dBJPGHcirY0jZG47WleWO56Fw4SXoyTIV0E6uk0aC23qYP7U-ArmRr-Tud73YmW7IeAxja8pOmKE1MmPgSlpElrXdpcqJf75SmoLFcusfqhpRXNLyWmkVI1eTTjUcgmRC9ytjmwuvvzuoUchjvFQwKc_DZk1Y9E5YvCsCI-by_Oh_m9FRuW4RxKGkMxFB2oGg7eOaA",
  "application_name": "32bj",
  "scope": "",
  "expires_in": "897",
  "status": "approved"
}
 */
