using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.AuthServer
{
    public class AuthOptions
    {
        public const string ISSUER = "library_app_core_client_side";
        public const string AUDIENCE = "http://localhost:51794/";
        const string KEY = "sec-ret-key1m-dk-gkdm-v-ki-250-of050=9ejh-283X8-20-7A-hA#1s-j80-k-a0-s821-O91";
        public const int LIFETIME = 5;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

    }
}
