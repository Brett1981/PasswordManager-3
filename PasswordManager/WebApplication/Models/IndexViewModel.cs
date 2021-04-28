using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class IndexViewModel
    {
        private readonly DataAccess.Interfaces.IAccountsRepository _accountsRepository;
        public List<Dbo.Account> Accounts { get; set; }

        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public IndexViewModel(/*ILogger<IndexModel> logger, */DataAccess.Interfaces.IAccountsRepository accountsRepository)
        {
            //_logger = logger;
            _accountsRepository = accountsRepository;
            Accounts = new List<Dbo.Account>();
        }

        public void GetById(int id)
        {
            Accounts = _accountsRepository.GetById(id);
        }

        public void GetAll()
        {
            Accounts = _accountsRepository.GetAll();
        }

        public async void AddAccount(string name, string login, string password)
        {
            await _accountsRepository.Insert(new Dbo.Account() { Name = name, Login = login, Password = password });
        }
    }
}
