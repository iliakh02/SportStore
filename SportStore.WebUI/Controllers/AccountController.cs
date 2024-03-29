﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models.Entities;
using SportStore.WebUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("/Register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Username,
                    PhoneNumber = registerViewModel.PhoneNumber
                };

                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newUser, new List<string> { "User" });
                    await _signInManager.SignInAsync(newUser, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View(registerViewModel);
        }

        [HttpGet]
        [Route("Login")]
        [Route("Account/Login")]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
        [Route("Login")]
        [Route("Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Login, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Login or password is not correct.");
                }

            }
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
