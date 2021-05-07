using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public static string encryptionKey = "VWF4FZqVHW7l5lvoazeQ";

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
            var unattachedAccounts = accounts.Where(x => x.CategoryId == null).ToList();
            var categories = _accountRepository.GetCategoriesBySessionId(sessionId);

            accounts.ForEach(x => x.Password = Decrypt(x.Password, encryptionKey));
            unattachedAccounts.ForEach(x => x.Password = Decrypt(x.Password, encryptionKey));

            ViewBag.Accounts = checkCompromisedPasswords(accounts).Result;
            ViewBag.Unattached = checkCompromisedPasswords(unattachedAccounts).Result;
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(AccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var account = new Dbo.Account();
            account.Name = accountViewModel.Name;
            account.Login = accountViewModel.Login;
            account.Password = Encrypt(accountViewModel.Password, encryptionKey);
            if (!String.IsNullOrEmpty(accountViewModel.Url))
                account.Url = "http://" + accountViewModel.Url;
            account.Name = accountViewModel.Name;

            account.SessionId = sessionId;

            if (!String.IsNullOrEmpty(accountViewModel.Category))
            {
                var categoryId = _categoryRepository.GetByName(accountViewModel.Category);
                if (categoryId != null)     // Category already exists
                    account.CategoryId = categoryId;
                else     // Category does not exist
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
            account.Password = Encrypt(accountViewModel.Password, encryptionKey);
            if (!String.IsNullOrEmpty(accountViewModel.Url))
                account.Url = "http://" + accountViewModel.Url;
            account.Name = accountViewModel.Name;

            account.SessionId = sessionId;

            if (!String.IsNullOrEmpty(accountViewModel.Category))
            {
                var categoryId = _categoryRepository.GetByName(accountViewModel.Category);
                if (categoryId != null)     // Category already exists
                    account.CategoryId = categoryId;
                else     // Category does not exist
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
            accountViewModel.Password = Decrypt(account.Password, encryptionKey);
            accountViewModel.Url = account.Url.Substring(7);

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

        [HttpPost]
        public ActionResult Accounts(AccountViewModel accountViewModel)
        {
            if (String.IsNullOrEmpty(accountViewModel.Search.Value))
                return RedirectToAction("Accounts", "Account");

            if (accountViewModel.Search.SearchBy == "name")
            {
                var accounts = _accountRepository.GetBySessionId(sessionId);
                var filteredAccounts = accounts.Where(x => x.Name.ToLower().Contains(accountViewModel.Search.Value.ToLower())).ToList();
                var unattachedAccounts = filteredAccounts.Where(x => x.CategoryId == null).ToList();

                var categories = _accountRepository.GetCategoriesBySessionId(sessionId);
                var filteredCategories = categories.Where(x => filteredAccounts.Any(y => y.CategoryId == x.Id)).Distinct().ToList();

                filteredAccounts.ForEach(x => x.Password = Decrypt(x.Password, encryptionKey));
                unattachedAccounts.ForEach(x => x.Password = Decrypt(x.Password, encryptionKey));

                ViewBag.Accounts = checkCompromisedPasswords(filteredAccounts).Result;
                ViewBag.Unattached = checkCompromisedPasswords(unattachedAccounts).Result;
                ViewBag.Categories = filteredCategories;
            }
            else
            {
                var accounts = _accountRepository.GetBySessionId(sessionId);
                var categories = _accountRepository.GetCategoriesBySessionId(sessionId);
                var filteredCategories = categories.Where(x => x.Name.ToLower().Contains(accountViewModel.Search.Value.ToLower())).ToList();

                accounts.ForEach(x => x.Password = Decrypt(x.Password, encryptionKey));

                ViewBag.Accounts = checkCompromisedPasswords(accounts).Result;
                ViewBag.Unattached = new Dictionary<Dbo.Account, string>();
                ViewBag.Categories = filteredCategories;
            }

            return View();
        }

        public string Encrypt(string text, string encryptionKey)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        public string Decrypt(string encryptedText, string encryptionKey)
        {
            byte[] bytes = Convert.FromBase64String(encryptedText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    encryptedText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return encryptedText;
        }

        public async Task<Dictionary<Dbo.Account, string>> checkCompromisedPasswords(List<Dbo.Account> accounts)
        {
            var res = new Dictionary<Dbo.Account, string>();

            var breachList = new List<Breach>();

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://haveibeenpwned.com/api/v3/breaches"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    breachList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Breach>>(apiResponse);
                }
            }

            accounts.ForEach((e) => {
                if (e.Url != null)
                {
                    var breach = breachList.Find((b) => b.Domain.Length > 0 && e.Url.Contains(b.Domain));
                    if (breach != null)
                    {
                        var message = "";
                        if (breach.DataClasses.Contains("Email addresses"))
                            message += "Email address";
                        if (breach.DataClasses.Contains("Usernames"))
                            message += ", Username";
                        if (breach.DataClasses.Contains("Passwords"))
                            message += " and Password";

                        res.Add(e, message + " compromised !");
                    }
                    else
                        res.Add(e, "");
                }
                else
                    res.Add(e, "");
            });
            return res;
        }
    }
}
