namespace IdentityServerHost.Pages.Error;

[AllowAnonymous]
[SecurityHeaders]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;

    public ViewModel View { get; set; } = new();

    public Index(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
    {
        _interaction = interaction;
        _environment = environment;
    }

    public async Task OnGet(string? errorId)
    {
        var message = await _interaction.GetErrorContextAsync(errorId);
        if (message is not null)
        {
            View.Error = message;

            if (!_environment.IsDevelopment())
                message.ErrorDescription = null;
        }
    }
}
