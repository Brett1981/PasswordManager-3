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
    public class PasswordController : Controller
    {
        public PasswordController()
        {
        }

        public ActionResult Generate()
        {
            var session = HttpContext.Session.GetInt32("SessionId");
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Generate(PasswordViewModel passwordViewModel)
        {

            var chars = "abcdefghijklmnopqrstuvwxyz";
            
            if (passwordViewModel.Uppercase)
                chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (passwordViewModel.Number)
                chars += "0123456789";

            if (passwordViewModel.Symbol)
                chars += "~`!@#$%^&*()_-+={[}]|\\:;\"'<,>.?/";

            Random random = new Random();
            var passwordLength = Int32.Parse(passwordViewModel.Length);
            var result = "";

            for (int i = 0; i < passwordLength; i++)
            {
                char c = chars[random.Next(chars.Length)];
                result += c;
            }

            ModelState.Clear();
            passwordViewModel.Password = result;

            return View(passwordViewModel);
        }
    }
}
