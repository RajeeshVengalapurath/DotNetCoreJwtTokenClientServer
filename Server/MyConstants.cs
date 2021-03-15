using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreJwtAuthorization
{
    public static class MyConstants
    {
        public const string Audiance = "https://localhost:44304/"; //right click on project then properties\debug to get this url
        public const string Issuer = Audiance;
        public const string SecretPhrase = "not_too_short_key_here_gshdgshfhewrgtwgdasbcdgldfgfhshdgfds";
    }
}
