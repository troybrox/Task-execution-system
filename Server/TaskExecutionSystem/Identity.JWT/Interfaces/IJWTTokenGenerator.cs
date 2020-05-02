using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskExecutionSystem.Identity.JWT.Models;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.Identity.JWT.Interfaces
{
    public interface IJWTTokenGenerator
    {
        JWTTokenResult Generate(User user, IList<string> roles);
    }
}
