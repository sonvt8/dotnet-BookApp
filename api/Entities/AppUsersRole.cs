using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class AppUsersRole : IdentityUserRole<int>
    {
        public AppUsers Users { get; set; }
        public AppRole Role { get; set; }
    }
}
