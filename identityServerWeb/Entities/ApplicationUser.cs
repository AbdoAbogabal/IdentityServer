
using Microsoft.AspNetCore.Identity;

namespace identityServerWeb.Entities;

public class ApplicationUser : IdentityUser
{
    public Guid CreatedBy { get; set; }
    public Guid ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
