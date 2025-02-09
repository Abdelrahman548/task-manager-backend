using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Person person, string role);
    }
}
