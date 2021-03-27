using api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUsers user);
    }
}
