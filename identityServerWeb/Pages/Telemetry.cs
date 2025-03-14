namespace IdentityServerHost.Pages;

public static class Telemetry
{
    private static readonly string ServiceVersion = typeof(Telemetry).Assembly.GetName().Version!.ToString();

    public static readonly string ServiceName = typeof(Telemetry).Assembly.GetName().Name!;

    public static class Metrics
    {
        public static class Counters
        {
            public const string Consent = "tokenservice.consent";
            public const string GrantsRevoked = "tokenservice.grants_revoked";
            public const string UserLogin = "tokenservice.user_login";
            public const string UserLogout = "tokenservice.user_logout";
        }
        public static class Tags
        {
            public const string Client = "client";
            public const string Error = "error";
            public const string Idp = "idp";
            public const string Remember = "remember";
            public const string Scope = "scope";
            public const string Consent = "consent";
        }

        public static class TagValues
        {
            public const string Granted = "granted";
            public const string Denied = "denied";
        }

        private static readonly Meter Meter = new Meter(ServiceName, ServiceVersion);

        private static Counter<long> ConsentCounter = Meter.CreateCounter<long>(Counters.Consent);

        public static void ConsentGranted(string clientId, IEnumerable<string> scopes, bool remember)
        {
            ArgumentNullException.ThrowIfNull(scopes);

            foreach (var scope in scopes)
            {
                ConsentCounter.Add(1,
                    new(Tags.Client, clientId),
                    new(Tags.Scope, scope),
                    new(Tags.Remember, remember),
                    new(Tags.Consent, TagValues.Granted));
            }
        }
        public static void ConsentDenied(string clientId, IEnumerable<string> scopes)
        {
            ArgumentNullException.ThrowIfNull(scopes);
            foreach (var scope in scopes)
                ConsentCounter.Add(1, new(Tags.Client, clientId), new(Tags.Scope, scope), new(Tags.Consent, TagValues.Denied));
        }

        private static Counter<long> GrantsRevokedCounter = Meter.CreateCounter<long>(Counters.GrantsRevoked);

        public static void GrantsRevoked(string? clientId) =>
                           GrantsRevokedCounter.Add(1, tag: new(Tags.Client, clientId));

        private static Counter<long> UserLoginCounter = Meter.CreateCounter<long>(Counters.UserLogin);

        public static void UserLogin(string? clientId, string idp) =>
                           UserLoginCounter.Add(1, new(Tags.Client, clientId), new(Tags.Idp, idp));

        public static void UserLoginFailure(string? clientId, string idp, string error) =>
                           UserLoginCounter.Add(1, new(Tags.Client, clientId), new(Tags.Idp, idp), new(Tags.Error, error));

        private static Counter<long> UserLogoutCounter = Meter.CreateCounter<long>(Counters.UserLogout);

        public static void UserLogout(string? idp) =>
                           UserLogoutCounter.Add(1, tag: new(Tags.Idp, idp));
    }
}
