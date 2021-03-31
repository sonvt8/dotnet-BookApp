using api.Extension;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class AppUsers : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<AppUsersRole> UserRoles { get; set; }
        public int GetAge()
        {
            return DateOfBirth.CalculateUserAge();
        }
    }
}
