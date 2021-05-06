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
            var result = "";

            for (int i = 0; i < 12; i++)
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
