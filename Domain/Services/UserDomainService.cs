using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class UserDomainService
    {
        public UserDomainService() { }

        public UserEntity CreateUser(string username, string email, string password, string role, IPasswordService passwordService)
        {
            string salt = passwordService.GenerateSalt();

            return new UserEntity()
            {
                Salt = salt,
                Password = passwordService.HashPassword(password + salt),
                Email = email,
                Username = username,
                Role = role,
            };

        }
    }
}
