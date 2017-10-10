using Microsoft.AspNetCore.Http;
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
        public const string ISSUER = "MyLibraryApp";
        public const string AUDIENCE = "http://localhost:51794/";
        const string KEY = "secretkey1mdkgkdmvki-25of050=9ejh";
        public const int LIFETIME = 1; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
