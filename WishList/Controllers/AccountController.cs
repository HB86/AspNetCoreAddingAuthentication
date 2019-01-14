using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;


    
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManger)
        {

            _userManager = userManager;
            _signInManager = signInManger;


        }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
        {


            return View();


        }



    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);

            }



            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,

            };

            var result =_userManager.CreateAsync(user, model.Password).Result;

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                    return View(model);

                }

            }

            return RedirectToAction("Index", "Home");


        }




    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {


        return View();


    }



    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {

            if (!ModelState.IsValid)
            {

                return View(model);

            }



            var loginResult = _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false).Result;


            if (!loginResult.Succeeded)
            {

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View();


            }


            return RedirectToAction("Index", "Home");



    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {

            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");



        }



  }

}
