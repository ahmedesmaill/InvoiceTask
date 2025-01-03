﻿using Business_Logic.Services;
using Business_Logic.Services.IServices;
using Data_Access.Models;
using Business_Logic.Utility;
using InvoiceTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InvoiceTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IConfiguration configuration;

        // Constructor to inject dependencies
        public AccountController(IUserService userService,IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }


        // Display the login page
        public IActionResult Login()
        {
            return View();
        }


        // Handle login post 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var captchaResponse = model.RecaptchaToken ;

            // Verify the CAPTCHA response
            if (string.IsNullOrEmpty(captchaResponse))
            {
                ModelState.AddModelError("", "Please complete the CAPTCHA.");
                return View(model);
            }
            string secretkey = configuration["GoogleRecaptcha:SecretKey"];
            bool seccess = await RecaptchaService.verifyReCaptcha(captchaResponse, secretkey);
            if (!seccess)
            {
                ModelState.AddModelError("", "Invalid CAPTCHA. Please try again.");
                       return View(model);
            }
            // Authenticate the user
            var user = userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            // Set session or other user-specific data
            HttpContext.Session.SetString("UserLoggedIn", user.Username);

            return RedirectToAction("Index", "Home");
        }


        // Display the registration page
        public IActionResult Register() => View();


        // Handle user registration
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        //public IActionResult VerifyEmail(string token)
        //{
        //    try
        //    {
        //        userService.VerifyEmail(token);
        //        return RedirectToAction("Login");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        // Handle user logout
        public IActionResult Logout()
        {
            userService.Logout();
            HttpContext.Session.Remove("UserLoggedIn"); 
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }

      
    
}
