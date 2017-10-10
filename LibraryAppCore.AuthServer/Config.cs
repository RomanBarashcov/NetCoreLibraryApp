using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAppCore.AuthServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("library_app_core_wep_api", "LibraryAppCore.WebApi")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Hybrid Flow = OpenId Connect + OAuth
                // To use both Identity and Access Tokens
                new Client
                {
                    ClientId = "library_app_core_client_side",
                    ClientName = "LibraryAppCore.ClientSide",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowOfflineAccess = true,
                    RequireConsent = false,

                    RedirectUris = { "http://localhost:51794/login" },
                    PostLogoutRedirectUris = { "http://localhost:51794/" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "library_app_core_wep_api"
                    },
                }
            };
        }
    }
}
