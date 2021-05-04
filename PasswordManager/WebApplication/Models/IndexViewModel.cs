using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class IndexViewModel
    {
        private readonly DataAccess.Interfaces.IAccountRepository _accountRepository;
        public List<Dbo.Account> Accounts { get; set; }

        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public IndexViewModel(/*ILogger<IndexModel> logger, */DataAccess.Interfaces.IAccountRepository accountRepository)
        {
            //_logger = logger;
            _accountRepository = accountRepository;
            Accounts = new List<Dbo.Account>();
        }

        public async void AddAccount(string name, string login, string password)
        {
            await _accountRepository.Insert(new Dbo.Account() { Name = name, Login = login, Password = password });
        }
    }
}
