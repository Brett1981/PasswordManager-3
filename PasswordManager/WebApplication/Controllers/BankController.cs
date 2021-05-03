using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class BankController : Controller
    {
        private readonly DataAccess.Interfaces.IBankRepository _bankRepository;
        BankViewModel bankViewModel;


        public BankController(DataAccess.Interfaces.IBankRepository bankRepository)
        {
            bankViewModel = new BankViewModel();
            _bankRepository = bankRepository;
            
        }
        /*
        public ActionResult Banks()
        {
            
        }

        [HttpPost]
        public async Task<ActionResult> Add(AccountViewModel accountViewModel)
        {
           
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(AccountViewModel accountViewModel)
        {
            return RedirectToAction("Accounts", "Account");
        }*/
    }
}
