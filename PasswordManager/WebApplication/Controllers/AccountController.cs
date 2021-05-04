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
        public static int sessionId = 0;

        public AccountController(DataAccess.Interfaces.IAccountRepository accountRepository, DataAccess.Interfaces.ICategoryRepository categoryRepository)
        {
            accountViewModel = new AccountViewModel();
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
        }

        public ActionResult Accounts()
        {
            var id = HttpContext.Session.GetInt32("SessionId");
            if (id != null)
                sessionId = (int)id;
            var accounts = _accountRepository.GetBySessionId(sessionId);
            var categories = _accountRepository.GetCategoriesBySessionId(sessionId);

            ViewBag.Accounts = accounts;
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(AccountViewModel accountViewModel)
        {
            var account = new Dbo.Account();
            account.Name = accountViewModel.Name;
            account.Login = accountViewModel.Login;
            account.Password = accountViewModel.Password;
            account.Url = accountViewModel.Url;
            account.Name = accountViewModel.Name;

            account.SessionId = sessionId;

            if (!String.IsNullOrEmpty(accountViewModel.Category))
            {
                var categoryId = _categoryRepository.GetByName(accountViewModel.Category);
                if (categoryId != null) // Category already exists
                    account.CategoryId = categoryId;
                else // Category does not exist
                {
                    var newCategory = new Dbo.Category();
                    newCategory.Name = accountViewModel.Category;

                    await _categoryRepository.Insert(newCategory);

                    account.CategoryId = _categoryRepository.GetByName(accountViewModel.Category);
                }
            }

            await _accountRepository.Insert(account);

            return RedirectToAction("Accounts", "Account");
        }

        [HttpPost]
        public async Task<ActionResult> Update(AccountViewModel accountViewModel)
        {
            var account = _accountRepository.GetById(accountViewModel.Id);
            account.Name = accountViewModel.Name;
            account.Login = accountViewModel.Login;
            account.Password = accountViewModel.Password;
            account.Url = accountViewModel.Url;
            account.Name = accountViewModel.Name;

            account.SessionId = sessionId;

            if (!String.IsNullOrEmpty(accountViewModel.Category))
            {
                var categoryId = _categoryRepository.GetByName(accountViewModel.Category);
                if (categoryId != null) // Category already exists
                    account.CategoryId = categoryId;
                else // Category does not exist
                {
                    var newCategory = new Dbo.Category();
                    newCategory.Name = accountViewModel.Category;

                    await _categoryRepository.Insert(newCategory);

                    account.CategoryId = _categoryRepository.GetByName(accountViewModel.Category);
                }
            }

            await _accountRepository.Update(account);

            return RedirectToAction("Accounts", "Account");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            await _accountRepository.Delete(id);

            return RedirectToAction("Accounts", "Account");
        }

        [HttpGet]
        public AccountViewModel GetById(int id)
        {
            var account = _accountRepository.GetById(id);

            var accountViewModel = new AccountViewModel();
            accountViewModel.Id = account.Id;
            accountViewModel.Name = account.Name;
            accountViewModel.Login = account.Login;
            accountViewModel.Password = account.Password;
            accountViewModel.Url = account.Url;

            if (account.CategoryId != null)
                accountViewModel.Category = _categoryRepository.GetById((int)account.CategoryId).Name;

            return accountViewModel;
        }

        [HttpGet]
        public Dbo.Category GetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id);

            return category;
        }
    }
}
