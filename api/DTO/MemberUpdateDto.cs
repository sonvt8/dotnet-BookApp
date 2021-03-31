using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class MemberUpdateDto
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
