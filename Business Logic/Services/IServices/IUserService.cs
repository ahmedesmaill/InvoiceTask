using Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Services.IServices
{
    public interface IUserService
    {
        User? Authenticate(string username, string password);
        void Register(User user);
        void VerifyEmail(string token);
        void Logout();
    }
}
