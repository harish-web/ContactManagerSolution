using Microsoft.AspNetCore.Identity;


namespace ContacManager.Core.Domain.IdentityEntities
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
