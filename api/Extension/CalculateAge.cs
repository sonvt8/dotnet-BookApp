using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Extension
{
    public static class CalculateAge
    {
        public static int CalculateUserAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }
}
