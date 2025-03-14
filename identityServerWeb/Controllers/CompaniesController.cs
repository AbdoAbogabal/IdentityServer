namespace identityServerWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult GetCompanies()
    {
        var claims = User.Claims;

        var companies = InMemoryConfig.GetUsers();

        return Ok(companies);
    }
}
