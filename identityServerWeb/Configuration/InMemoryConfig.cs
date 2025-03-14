namespace identityServerWeb.Configuration;

public static class InMemoryConfig
{
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
                                                new List<IdentityResource>
                                                {
                                                    new IdentityResources.OpenId(),
                                                    new IdentityResources.Profile()
                                                };

    public static IEnumerable<ApiScope> GetApiScopes() =>
                                         new List<ApiScope>
                                            {
                                                new("companyApi", "CompanyEmployee API")
                                            };

    public static IEnumerable<ApiResource> GetApiResources() =>
                                            new List<ApiResource>
                                            {
                                                new("companyApi", "CompanyEmployee API")
                                                {
                                                    Scopes = { "companyApi" }
                                                }
                                            };
    public static List<TestUser> GetUsers() =>
                                 new List<TestUser>
                                 {
                                     new TestUser
                                     {
                                         SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                                         Username = "Abdelrahman",
                                         Password = "AbdelrahmanPassword",
                                         Claims = new List<Claim>
                                         {
                                             new Claim("given_name", "Mick"),
                                             new Claim("family_name", "Mining")
                                         }
                                     },
                                     new TestUser
                                     {
                                         SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                                         Username = "Jane",
                                         Password = "JanePassword",
                                         Claims = new List<Claim>
                                         {
                                             new Claim("given_name", "Jane"),
                                             new Claim("family_name", "Downing")
                                         }
                                     }
                                 };

    public static IEnumerable<Client> GetClients() =>
                                      new List<Client>
                                      {
                                         new Client
                                         {
                                              ClientId = "abogabal-origin",
                                              ClientSecrets = new [] { new Secret("abogabalsecret".Sha512()) },
                                              AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                                              AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "companyApi" }
                                     
                                          }
                                      };
}