using Business_Logic.Services.IServices;
using Data_Access.Models;
using Data_Access.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Services
{
     public class UserService : IUserService
        {
            private readonly IUserRepository userRepository;
            private readonly IHttpContextAccessor httpContextAccessor;

            public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
                {
                this.userRepository = userRepository;
                this.httpContextAccessor = httpContextAccessor;
            }

            public User? Authenticate(string username, string password)
            {
                var user = userRepository.GetOne(expression: u => u.Username == username);
                if (user == null || user.Password != HashPassword(password))
                {
                    return null; 
                }
                return user;
            }

            public void Register(User user)
            {
                user.Password = HashPassword(user.Password);
                user.VerificationToken = Guid.NewGuid().ToString();
                userRepository.Create(user);
                userRepository.Commit();
            }
        //public void Logout(string username)
        //{
        //    var user = _userRepository.GetOne(expression:u => u.Username == username);
        //    if (user != null)
        //    {
        //        user.VerificationToken = null;
        //        _userRepository.Edit(user);
        //        _userRepository.Commit();
        //    }
        //}
            public void Logout()
            {
                var username = httpContextAccessor.HttpContext.User?.Identity?.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    httpContextAccessor.HttpContext.Session.Remove("UserLoggedIn"); 
                }
            }

        public void VerifyEmail(string token)
            {
                var user = userRepository.GetOne(expression: u => u.VerificationToken == token);
            if (user == null) throw new Exception("Invalid token");

                user.IsVerified = true;
                userRepository.Edit(user);
                userRepository.Commit();
            }

            private string HashPassword(string password)
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha256.ComputeHash(bytes));
            }
     }
    
}
