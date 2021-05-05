using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess;
using WebApplication.Models;

namespace WebApplication.Controllers
{

    public class UserController : Controller
    {
        private readonly DataAccess.Interfaces.IUserRepository _userRepository;
        UserViewModel userViewModel;

        public UserController(DataAccess.Interfaces.IUserRepository userRepository)
        {
            userViewModel = new UserViewModel();
            _userRepository = userRepository;
        }
        // GET: UserController
        //[Route("User/Login")]
        public IActionResult Login()
        {
            var name = HttpContext.Session.GetString("Username");
            if (name != null)
                return RedirectToAction("Accounts", "Account");
            return View();
        }

        public IActionResult Deconnexion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserViewModel user)
        {
            if (!ModelState.IsValid)
                return View();
            var userExist = _userRepository.GetUser(user.login, user.password);
            if (userExist != null)
            {
                HttpContext.Session.SetInt32("SessionId", userExist.Id);

                HttpContext.Session.SetString("Username", userExist.Username);

                return RedirectToAction("Accounts", "Account");
            }
            else if (user.login != "" && user.password != "")
            {
                ViewBag.ErrorMessage = "User does not exist";
                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel user)
        {
            if (!ModelState.IsValid)
                return View();
            if (user.password != user.confirmpassword)
            {
                ViewBag.ErrorMessage = "Password not match";
                return View();
            }
            var emailExist = _userRepository.GetEmail(user.email);
            if (emailExist == null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.password);
                await _userRepository.Insert(new Dbo.User() { Username = user.login, Password = passwordHash ,Email = user.email });;
                return RedirectToAction("Login", "User");
            }
            else
            {
                ViewBag.ErrorEmail = "Email already exist";
                return View();
            }
        }
    }
}
