using Business_Logic.Services;
using Business_Logic.Services.IServices;
using Data_Access.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var user = userService.Authenticate(username, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid User.");
                return View(user);
            }
            //if (!user.IsVerified)
            //{
            //    ModelState.AddModelError("", "Please verify your email first.");
            //    return View(user);

            //}


             HttpContext.Session.SetString("UserLoggedIn", user.Username);
    
            return RedirectToAction("Index", "Home");
         
        }
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userService.Register(user);
                    TempData["success"] = "Registration successful!";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View();
                }
            }
            return View(user);
        }
        
        public IActionResult VerifyEmail(string token)
        {
            try
            {
                userService.VerifyEmail(token);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Logout()
        {
            userService.Logout();
            HttpContext.Session.Remove("UserLoggedIn"); 
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }

      
    
}
