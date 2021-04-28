using Microsoft.AspNetCore.Mvc;
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
        private readonly DataAccess.Interfaces.IAccountsRepository _accountsRepository;
        IndexViewModel indexViewModel;

        public HomeController(ILogger<HomeController> logger, DataAccess.Interfaces.IAccountsRepository accountsRepository)
        {
            _logger = logger;
            _accountsRepository = accountsRepository;
            indexViewModel = new IndexViewModel(_accountsRepository);
        }

        public async Task<IActionResult> Index()
        {
            indexViewModel.GetAll();
            return View(indexViewModel);
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
