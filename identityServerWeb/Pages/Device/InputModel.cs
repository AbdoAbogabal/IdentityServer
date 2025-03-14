namespace IdentityServerHost.Pages.Device;

public class InputModel
{
    public bool RememberConsent { get; set; } = true;

    public string? Button { get; set; }
    public string? UserCode { get; set; }
    public string? ReturnUrl { get; set; }
    public string? Description { get; set; }

    public IEnumerable<string> ScopesConsented { get; set; } = new List<string>();
}
