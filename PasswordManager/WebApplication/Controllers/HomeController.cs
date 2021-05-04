using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataAccess.Interfaces.IAccountRepository _accountRepository;
        IndexViewModel indexViewModel;

        public HomeController(ILogger<HomeController> logger, DataAccess.Interfaces.IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            indexViewModel = new IndexViewModel(_accountRepository);
        }

        public async Task<IActionResult> Index()
        {
            //indexViewModel.GetAll();
            //return View(indexViewModel);
            var session = HttpContext.Session.GetInt32("SessionId");
            if ( session != null)
            {
                RedirectToAction("Accounts", "Account");
            }
            return RedirectToAction("Login","User");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name, string login, string password)
        {
            indexViewModel.AddAccount(name, login, password);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
