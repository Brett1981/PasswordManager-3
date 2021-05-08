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
            var lowers = "abcdefghijklmnopqrstuvwxyz";
            var uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numerics = "0123456789";
            var symbols = "~`!@#$%^&*()_-+={[}]|\\:;\"'<,>.?/";

            var chars = lowers;

            if (passwordViewModel.Uppercase)
                chars += uppers;
            if (passwordViewModel.Number)
                chars += numerics;
            if (passwordViewModel.Symbol)
                chars += symbols;

            Random random = new Random();
            var passwordLength = Int32.Parse(passwordViewModel.Length);

            var result = Enumerable.Range(0, passwordLength)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray();

            List<int> excludedIdxs = new List<int>();

            var idx = random.Next(result.Length);
            result[idx] = lowers[random.Next(lowers.Length)];
            excludedIdxs.Add(idx);

            if (passwordViewModel.Uppercase)
            {
                idx = randomIdxWithExclusion(random, result, excludedIdxs);
                result[idx] = uppers[random.Next(uppers.Length)];
                excludedIdxs.Add(idx);
            }
            if (passwordViewModel.Number)
            {
                idx = randomIdxWithExclusion(random, result, excludedIdxs);
                result[idx] = numerics[random.Next(numerics.Length)];
                excludedIdxs.Add(idx);
            }
            if (passwordViewModel.Symbol)
            {
                idx = randomIdxWithExclusion(random, result, excludedIdxs);
                result[idx] = symbols[random.Next(symbols.Length)];
            }

            ModelState.Clear();
            passwordViewModel.Password = new String(result);

            return View(passwordViewModel);
        }

        public int randomIdxWithExclusion(Random random, char[] array, List<int> excludedIdxs)
        {
            var randomIdx = random.Next(array.Length);

            while (excludedIdxs.Any(x => x == randomIdx))
                randomIdx = random.Next(array.Length);

            return randomIdx;
        }
    }
}
