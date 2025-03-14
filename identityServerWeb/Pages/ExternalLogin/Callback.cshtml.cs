namespace IdentityServerHost.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public class Callback : PageModel
{
    private readonly TestUserStore _users;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<Callback> _logger;
    private readonly IEventService _events;

    public Callback(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<Callback> logger,
        TestUserStore? users = null)
    {
        // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
        _users = users ?? throw new InvalidOperationException("Please call 'AddTestUsers(TestUsers.Users)' on the IIdentityServerBuilder in Startup or remove the TestUserStore from the AccountController.");

        _interaction = interaction;
        _logger = logger;
        _events = events;
    }

    public async Task<IActionResult> OnGet()
    {
        var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        if (result.Succeeded != true)
            throw new InvalidOperationException($"External authentication error: {result.Failure}");

        var externalUser = result.Principal ??
            throw new InvalidOperationException("External authentication produced a null Principal");

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = externalUser.Claims.Select(c => $"{c.Type}: {c.Value}");
            _logger.ExternalClaims(externalClaims);
        }

        var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new InvalidOperationException("Unknown userid");

        var provider = result.Properties.Items["scheme"] ?? throw new InvalidOperationException("Null scheme in authentiation properties");
        var providerUserId = userIdClaim.Value;

        var user = _users.FindByExternalProvider(provider, providerUserId);
        if (user is null)
        {
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);
            user = _users.AutoProvisionUser(provider, providerUserId, claims.ToList());
        }

        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

        var isuser = new IdentityServerUser(user.SubjectId)
        {
            DisplayName = user.Username,
            IdentityProvider = provider,
            AdditionalClaims = additionalLocalClaims
        };

        await HttpContext.SignInAsync(isuser, localSignInProps);

        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.SubjectId, user.Username, true, context?.Client.ClientId));
        Telemetry.Metrics.UserLogin(context?.Client.ClientId, provider!);

        if (context is not null && context.IsNativeClient())
            return this.LoadingPage(returnUrl);

        return Redirect(returnUrl);
    }

    private static void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        ArgumentNullException.ThrowIfNull(externalResult.Principal, nameof(externalResult.Principal));
        localClaims.Add(new Claim(JwtClaimTypes.IdentityProvider, externalResult.Properties?.Items["scheme"] ?? "unknown identity provider"));

        var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid is not null)
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));

        var idToken = externalResult.Properties?.GetTokenValue("id_token");
        if (idToken is not null)
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
    }
}