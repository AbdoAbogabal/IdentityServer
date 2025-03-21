namespace IdentityServerHost.Pages.Grants;

public class ViewModel
{
    public List<GrantViewModel>? Grants { get; set; }
}

public class GrantViewModel
{
    public string? ClientId { get; set; }
    public string? ClientName { get; set; }
    public string? ClientUrl { get; set; }
    public string? Description { get; set; }
    public string? ClientLogoUrl { get; set; }

    public DateTime Created { get; set; }
    public DateTime? Expires { get; set; }

    public IEnumerable<string> ApiGrantNames { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string> IdentityGrantNames { get; set; } = Enumerable.Empty<string>();
}
