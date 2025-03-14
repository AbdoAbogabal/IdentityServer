namespace IdentityServerHost.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    public ViewModel View { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        if (HttpContext.Connection.IsRemote())
            return NotFound();

        View = new ViewModel(await HttpContext.AuthenticateAsync());
            
        return Page();
    }
}
