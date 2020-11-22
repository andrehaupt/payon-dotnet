using System;
using System.Text;

namespace PayOn.Models
{//Authentication
 // To make REST API calls, include the access token in the Authorization header with the Bearer authentication scheme.
 // 
 // Parameter / Header	Description	Format	Required
 // entityId	The entity required to authorize the request. This should be the channel entity identifier. In case channel dispatching is activated then it should be the merchant entity identifier.	AN32
 // [a-f0-9]{32}	Conditional
 // Authorization Bearer <access-token>	Authorization header with Bearer authentication scheme. Access token can be taken from the backend UI under Administration > Account data > Merchant / Channel Info only if you have specific administration rights.	Header	Required

    internal class Authentication
    {
        private string _userId;
        private string _password;

        public Authentication(string userId, string password)
        {
            _userId = userId;
            _password = password;
        }

        public string AccessToken => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userId}|{_password}"));
    }
}
