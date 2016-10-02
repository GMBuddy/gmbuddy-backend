using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;

namespace GMBuddyIdentity
{
    public class IdentityConfig
    {
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                new Scope
                {
                    Name = "GMBuddyApi",
                    Description = "GMBuddy Front-end API Access"
                },
                new Scope
                {
                    Name = "GMBuddyData",
                    Description = "GMBuddy Back-end data services"
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "GMBuddyFrontend",
                    ClientName = "GMBuddy.com",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:5002"
                    },
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        "GMBuddyApi"
                    }
                },
                new Client
                {
                    ClientId = "GMBuddyRestTesting",
                    ClientName = "GMBuddy Rest API",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        "GMBuddyApi"
                    }
                },
                new Client
                {
                    ClientId = "GMBuddyApi",
                    ClientName = "GMBuddy API Layer",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "GMBuddyData"
                    }
                }
            };
        }

        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "1",
                    Username = "horton7@purdue.edu",
                    Password = "password"
                }
            };
        }
    }
}
