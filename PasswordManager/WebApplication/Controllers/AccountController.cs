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
    public class AccountController : Controller
    {
        private readonly DataAccess.Interfaces.IAccountRepository _accountRepository;
        private readonly DataAccess.Interfaces.ICategoryRepository _categoryRepository;
        AccountViewModel accountViewModel;

        public AccountController(DataAccess.Interfaces.IAccountRepository accountRepository, DataAccess.Interfaces.ICategoryRepository categoryRepository)
        {
            accountViewModel = new AccountViewModel();
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
        }

        public ActionResult Accounts()
        {
            var accounts = _accountRepository.GetBySessionId(17);
            var categories = _accountRepository.GetCategoriesBySessionId(17);

            ViewBag.Accounts = accounts;
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Accounts(AccountViewModel accountViewModel)
        {
            var account = new Dbo.Account();
            account.Name = accountViewModel.Name;
            account.Login = accountViewModel.Login;
            account.Password = accountViewModel.Password;
            account.Url = accountViewModel.Url;
            account.Name = accountViewModel.Name;

            account.CategoryId = _accountRepository.GetCategoryByName(accountViewModel.Category, 17);
            account.SessionId = 17;

            await _accountRepository.Insert(account);
            return RedirectToAction("Accounts", "Account");
        }
    }
}
