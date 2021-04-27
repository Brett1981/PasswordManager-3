using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplication.DataAccess;

namespace WebApplication.Views.Home
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DataAccess.Interfaces.IAccountsRepository _accountsRepository;
        public List<Dbo.Account> Accounts { get; set; }

        public IndexModel(ILogger<IndexModel> logger, DataAccess.Interfaces.IAccountsRepository accountsRepository)
        {
            _logger = logger;
            _accountsRepository = accountsRepository;
            Accounts = new List<Dbo.Account>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Accounts = new List<Dbo.Account>();
            Accounts = _accountsRepository.GetById(2);

            return Page();
        }
    }
}
