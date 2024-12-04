using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPasswordService
    {
        string GenerateSalt();
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
