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
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var userExist = _userRepository.GetUser(login, password);
            if (userExist)
                return RedirectToAction("Privacy", "Home");
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email,string login, string password, string confirmpassword)
        {
            if (password != confirmpassword)
            {
                ViewBag.error = "Password wrong";
                return View();
            }


            var emailExist = _userRepository.GetEmail(email);

            if (emailExist == null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                await _userRepository.Insert(new Dbo.User() { Username = login, Password = passwordHash ,Email = email });;

            }
            return RedirectToAction("Login","User");

        }
    }
}
