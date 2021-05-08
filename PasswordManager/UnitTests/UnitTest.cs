using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Controllers;
using WebApplication.DataAccess;
using WebApplication.DataAccess.Interfaces;
using WebApplication.Models;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        private IUserRepository _userRepository;
        private IAccountRepository _accountRepository;
        private IBankRepository _bankRepository;
        private ICategoryRepository _categoryRepository;

        public UnitTest()
        {
            PasswordManagerContext context = new PasswordManagerContext();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            Logger<UserRepository> userLogger = null;
            Logger<AccountRepository> accountLogger = null;
            Logger<BankRepository> bankLogger = null;

            _userRepository = new UserRepository(context, userLogger, mapper);
            _accountRepository = new AccountRepository(context, accountLogger, mapper);
            _bankRepository = new BankRepository(context, bankLogger, mapper);
            _categoryRepository = new CategoryRepository(context, accountLogger, mapper);
        }

        [Fact]
        [TestMethod]
        public async Task USER_INSERT()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);

            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            Assert.IsNotNull(dbUser);
            Assert.AreEqual(dbUser.Username, "testlogin");
            Assert.AreEqual(dbUser.Password, "testpassword");
            Assert.AreEqual(dbUser.Email, "test@email.com");

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task USER_GET_EMAIL()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);

            var dbEmail = _userRepository.GetEmail("test@email.com");
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            Assert.AreEqual(dbEmail, "test@email.com");

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task USER_INSERT_WITH_HASH_GET_USER()
        {
            var hashPassword = BCrypt.Net.BCrypt.HashPassword("testpassword");

            await Task.Delay(500);

            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = hashPassword,
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);

            var dbUser = _userRepository.GetUser("testlogin", "testpassword");

            Assert.IsNotNull(dbUser);
            Assert.AreEqual(dbUser.Username, "testlogin");
            Assert.AreEqual(dbUser.Password, hashPassword);
            Assert.AreEqual(dbUser.Email, "test@email.com");

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_INSERT()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = "testpassword",
                Url = "testurl",
                SessionId = dbUser.Id
            };

            await _accountRepository.Insert(account);

            var dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testname" && x.Login == "testlogin" && x.Password == "testpassword" && x.Url == "testurl").FirstOrDefault();

            Assert.IsNotNull(dbAccount);
            Assert.AreEqual(dbAccount.Name, "testname");
            Assert.AreEqual(dbAccount.Login, "testlogin");
            Assert.AreEqual(dbAccount.Password, "testpassword");
            Assert.AreEqual(dbAccount.Url, "testurl");
            Assert.AreEqual(dbAccount.SessionId, dbUser.Id);
            Assert.IsNull(dbAccount.CategoryId);

            await _accountRepository.Delete(dbAccount.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_UPDATE()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = "testpassword",
                Url = "testurl",
                SessionId = dbUser.Id
            };

            await _accountRepository.Insert(account);
            var dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testname" && x.Login == "testlogin" && x.Password == "testpassword" && x.Url == "testurl").FirstOrDefault();

            var accountUpdated = new WebApplication.Dbo.Account()
            {
                Id = dbAccount.Id,
                Name = "testnameupdated",
                Login = "testloginupdated",
                Password = "testpasswordupdated",
                Url = "testurlupdated",
                SessionId = dbUser.Id
            };

            await _accountRepository.Update(accountUpdated);

            dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testnameupdated" && x.Login == "testloginupdated" && x.Password == "testpasswordupdated" && x.Url == "testurlupdated").FirstOrDefault();

            Assert.IsNotNull(dbAccount);
            Assert.AreEqual(dbAccount.Name, "testnameupdated");
            Assert.AreEqual(dbAccount.Login, "testloginupdated");
            Assert.AreEqual(dbAccount.Password, "testpasswordupdated");
            Assert.AreEqual(dbAccount.Url, "testurlupdated");
            Assert.AreEqual(dbAccount.SessionId, dbUser.Id);

            await _accountRepository.Delete(dbAccount.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_INSERT_ENCRPYT_DECRYPT()
        {
            var encryptionKey = "CjvAIukcoQVKZaku6rat";

            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = AccountController.Encrypt("testpassword", encryptionKey),
                Url = "testurl",
                SessionId = dbUser.Id
            };

            await _accountRepository.Insert(account);

            var dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testname" && x.Login == "testlogin" && x.Url == "testurl").FirstOrDefault();

            var decryptedPassword = AccountController.Decrypt(dbAccount.Password, encryptionKey);

            Assert.IsNotNull(dbAccount);
            Assert.AreEqual(dbAccount.Name, "testname");
            Assert.AreEqual(dbAccount.Login, "testlogin");
            Assert.AreNotEqual(dbAccount.Password, "testpassword");
            Assert.AreEqual(decryptedPassword, "testpassword");
            Assert.AreEqual(dbAccount.Url, "testurl");
            Assert.AreEqual(dbAccount.SessionId, dbUser.Id);
            Assert.IsNull(dbAccount.CategoryId);

            await _accountRepository.Delete(dbAccount.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_INSERT_WITH_CATEGORY()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var category = new WebApplication.Dbo.Category()
            {
                Name = "testcategory"
            };

            await _categoryRepository.Insert(category);
            var dbCategory = _categoryRepository.Get().Result.Where(x => x.Name == "testcategory").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = "testpassword",
                Url = "testurl",
                SessionId = dbUser.Id,
                CategoryId = dbCategory.Id
            };

            await _accountRepository.Insert(account);

            var dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testname" && x.Login == "testlogin" && x.Password == "testpassword" && x.Url == "testurl").FirstOrDefault();

            Assert.IsNotNull(dbAccount);
            Assert.AreEqual(dbAccount.Name, "testname");
            Assert.AreEqual(dbAccount.Login, "testlogin");
            Assert.AreEqual(dbAccount.Password, "testpassword");
            Assert.AreEqual(dbAccount.Url, "testurl");
            Assert.AreEqual(dbAccount.SessionId, dbUser.Id);
            Assert.AreEqual(dbAccount.CategoryId, dbCategory.Id);

            await _accountRepository.Delete(dbAccount.Id);

            await _categoryRepository.Delete(dbCategory.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_INSERT_COMPROMISED()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = "testpassword",
                Url = "adobe.com",
                SessionId = dbUser.Id,
                CreatedAt = new DateTime(2010, 12, 31)
            };

            await _accountRepository.Insert(account);

            var dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testname" && x.Login == "testlogin" && x.Password == "testpassword" && x.Url == "adobe.com").FirstOrDefault();

            Assert.IsNotNull(dbAccount);
            Assert.AreEqual(dbAccount.Name, "testname");
            Assert.AreEqual(dbAccount.Login, "testlogin");
            Assert.AreEqual(dbAccount.Password, "testpassword");
            Assert.AreEqual(dbAccount.Url, "adobe.com");
            Assert.AreEqual(dbAccount.SessionId, dbUser.Id);
            Assert.AreEqual(dbAccount.CreatedAt, new DateTime(2010, 12, 31));

            var breachList = new List<Breach>();

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://haveibeenpwned.com/api/v3/breaches"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    breachList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Breach>>(apiResponse);
                }
            }

            var breach = breachList.Find((b) => b.Domain.Length > 0 && dbAccount.Url.Contains(b.Domain) && b.BreachDate > dbAccount.CreatedAt);
            Assert.IsNotNull(breach);

            await _accountRepository.Delete(dbAccount.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_GET_BY_ID()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = "testpassword",
                Url = "testurl",
                SessionId = dbUser.Id
            };

            await _accountRepository.Insert(account);

            var dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testname" && x.Login == "testlogin" && x.Password == "testpassword" && x.Url == "testurl").FirstOrDefault();
            dbAccount = _accountRepository.GetById(dbAccount.Id);

            Assert.IsNotNull(dbAccount);
            Assert.AreEqual(dbAccount.Name, "testname");
            Assert.AreEqual(dbAccount.Login, "testlogin");
            Assert.AreEqual(dbAccount.Password, "testpassword");
            Assert.AreEqual(dbAccount.Url, "testurl");
            Assert.AreEqual(dbAccount.SessionId, dbUser.Id);
            Assert.IsNull(dbAccount.CategoryId);

            await _accountRepository.Delete(dbAccount.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_GET_BY_SESSION_ID()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = "testpassword",
                Url = "testurl",
                SessionId = dbUser.Id
            };

            await _accountRepository.Insert(account);

            var dbAccount = _accountRepository.GetBySessionId(account.SessionId);

            Assert.IsNotNull(dbAccount);
            Assert.AreEqual(dbAccount.Count, 1);
            Assert.AreEqual(dbAccount[0].Name, "testname");
            Assert.AreEqual(dbAccount[0].Login, "testlogin");
            Assert.AreEqual(dbAccount[0].Password, "testpassword");
            Assert.AreEqual(dbAccount[0].Url, "testurl");
            Assert.AreEqual(dbAccount[0].SessionId, dbUser.Id);
            Assert.IsNull(dbAccount[0].CategoryId);

            await _accountRepository.Delete(dbAccount[0].Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task ACCOUNT_GET_CATEGORIES_BY_SESSION_ID()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var category = new WebApplication.Dbo.Category()
            {
                Name = "testcategory"
            };

            await _categoryRepository.Insert(category);
            var dbCategory = _categoryRepository.Get().Result.Where(x => x.Name == "testcategory").FirstOrDefault();

            var account = new WebApplication.Dbo.Account()
            {
                Name = "testname",
                Login = "testlogin",
                Password = "testpassword",
                Url = "testurl",
                SessionId = dbUser.Id,
                CategoryId = dbCategory.Id
            };

            await _accountRepository.Insert(account);

            var dbAccount = _accountRepository.Get().Result.Where(x => x.Name == "testname" && x.Login == "testlogin" && x.Password == "testpassword" && x.Url == "testurl").FirstOrDefault();

            var dbCategories = _accountRepository.GetCategoriesBySessionId(dbUser.Id);

            Assert.IsNotNull(dbCategories);
            Assert.AreEqual(dbCategories.Count, 1);
            Assert.AreEqual(dbCategories[0].Id, dbCategory.Id);
            Assert.AreEqual(dbCategories[0].Name, "testcategory");

            await _accountRepository.Delete(dbAccount.Id);

            await _categoryRepository.Delete(dbCategory.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task BANK_INSERT()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var bank = new WebApplication.Dbo.Bank()
            {
                NumberCard = "testnumber",
                Name = "testname",
                Cvc = "testcvc",
                Date = "testdate",
                SessionId = dbUser.Id
            };

            await _bankRepository.Insert(bank);

            var dbBank = _bankRepository.Get().Result.Where(x => x.NumberCard == "testnumber" && x.Name == "testname" && x.Cvc == "testcvc" && x.Date == "testdate").FirstOrDefault();

            Assert.IsNotNull(dbBank);
            Assert.AreEqual(dbBank.NumberCard, "testnumber");
            Assert.AreEqual(dbBank.Name, "testname");
            Assert.AreEqual(dbBank.Cvc, "testcvc");
            Assert.AreEqual(dbBank.Date, "testdate");
            Assert.AreEqual(dbBank.SessionId, dbUser.Id);

            await _bankRepository.Delete(dbBank.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task BANK_UPDATE()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var bank = new WebApplication.Dbo.Bank()
            {
                NumberCard = "testnumber",
                Name = "testname",
                Cvc = "testcvc",
                Date = "testdate",
                SessionId = dbUser.Id
            };

            await _bankRepository.Insert(bank);

            var dbBank = _bankRepository.Get().Result.Where(x => x.NumberCard == "testnumber" && x.Name == "testname" && x.Cvc == "testcvc" && x.Date == "testdate").FirstOrDefault();

            var bankUpdated = new WebApplication.Dbo.Bank()
            {
                Id = dbBank.Id,
                NumberCard = "testnumberupdated",
                Name = "testnameupdated",
                Cvc = "testcvcupdated",
                Date = "testdateupdated",
                SessionId = dbUser.Id
            };

            await _bankRepository.Update(bankUpdated);

            dbBank = _bankRepository.Get().Result.Where(x => x.NumberCard == "testnumberupdated" && x.Name == "testnameupdated" && x.Cvc == "testcvcupdated" && x.Date == "testdateupdated").FirstOrDefault();

            Assert.IsNotNull(dbBank);
            Assert.AreEqual(dbBank.NumberCard, "testnumberupdated");
            Assert.AreEqual(dbBank.Name, "testnameupdated");
            Assert.AreEqual(dbBank.Cvc, "testcvcupdated");
            Assert.AreEqual(dbBank.Date, "testdateupdated");
            Assert.AreEqual(dbBank.SessionId, dbUser.Id);

            await _bankRepository.Delete(dbBank.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task BANK_GET_BY_ID()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var bank = new WebApplication.Dbo.Bank()
            {
                NumberCard = "testnumber",
                Name = "testname",
                Cvc = "testcvc",
                Date = "testdate",
                SessionId = dbUser.Id
            };

            await _bankRepository.Insert(bank);

            var dbBank = _bankRepository.Get().Result.Where(x => x.NumberCard == "testnumber" && x.Name == "testname" && x.Cvc == "testcvc" && x.Date == "testdate").FirstOrDefault();
            dbBank = _bankRepository.GetById(dbBank.Id);

            Assert.IsNotNull(dbBank);
            Assert.AreEqual(dbBank.NumberCard, "testnumber");
            Assert.AreEqual(dbBank.Name, "testname");
            Assert.AreEqual(dbBank.Cvc, "testcvc");
            Assert.AreEqual(dbBank.Date, "testdate");
            Assert.AreEqual(dbBank.SessionId, dbUser.Id);

            await _bankRepository.Delete(dbBank.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task BANK_GET_BY_SESSION_ID()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var bank = new WebApplication.Dbo.Bank()
            {
                NumberCard = "testnumber",
                Name = "testname",
                Cvc = "testcvc",
                Date = "testdate",
                SessionId = dbUser.Id
            };

            await _bankRepository.Insert(bank);

            var dbBank = _bankRepository.GetBySessionId(bank.SessionId);

            Assert.IsNotNull(dbBank);
            Assert.AreEqual(dbBank.Count, 1);
            Assert.AreEqual(dbBank[0].NumberCard, "testnumber");
            Assert.AreEqual(dbBank[0].Name, "testname");
            Assert.AreEqual(dbBank[0].Cvc, "testcvc");
            Assert.AreEqual(dbBank[0].Date, "testdate");
            Assert.AreEqual(dbBank[0].SessionId, dbUser.Id);

            await _bankRepository.Delete(dbBank[0].Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task CATEGORY_GET_BY_ID()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var category = new WebApplication.Dbo.Category()
            {
                Name = "testcategory"
            };

            await _categoryRepository.Insert(category);

            var dbCategory = _categoryRepository.Get().Result.Where(x => x.Name == "testcategory").FirstOrDefault();
            dbCategory = _categoryRepository.GetById(dbCategory.Id);

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(dbCategory.Name, "testcategory");

            await _categoryRepository.Delete(dbCategory.Id);

            await _userRepository.Delete(dbUser.Id);
        }

        [Fact]
        [TestMethod]
        public async Task CATEGORY_GET_BY_NAME()
        {
            var user = new WebApplication.Dbo.User()
            {
                Username = "testlogin",
                Password = "testpassword",
                Email = "test@email.com"
            };

            await _userRepository.Insert(user);
            var dbUser = _userRepository.Get().Result.Where(x => x.Username == "testlogin" && x.Password == "testpassword" && x.Email == "test@email.com").FirstOrDefault();

            var category = new WebApplication.Dbo.Category()
            {
                Name = "testcategory"
            };

            await _categoryRepository.Insert(category);

            var dbCategory = _categoryRepository.Get().Result.Where(x => x.Name == "testcategory").FirstOrDefault();
            var dbCategoryId = _categoryRepository.GetByName(dbCategory.Name);

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(dbCategory.Id, dbCategoryId);
            Assert.AreEqual(dbCategory.Name, "testcategory");

            await _categoryRepository.Delete(dbCategory.Id);

            await _userRepository.Delete(dbUser.Id);
        }
    }
}
