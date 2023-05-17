using Microsoft.AspNetCore.Identity;

namespace NEST.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get;set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }

    }
}
