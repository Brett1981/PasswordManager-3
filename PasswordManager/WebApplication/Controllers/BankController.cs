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
        
        public ActionResult Banks()
        {
            var banks = _bankRepository.GetBySessionId(17);
            ViewBag.Banks = banks;        
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Add(BankViewModel bankViewModel)
        {
            var bank = new Dbo.Bank();
            bank.Name = bankViewModel.Name;
            bank.Cvc = bankViewModel.Cvc;
            bank.Date = bankViewModel.Date;
            bank.NumberCard = bankViewModel.NumberCard;
            bank.SessionId = 17;

            await _bankRepository.Insert(bank);

            return RedirectToAction("Banks", "Bank");
        } 
        

        [HttpDelete]
        public async Task<ActionResult> Delete(BankViewModel bankViewModel)
        {
            return RedirectToAction("Banks", "Bank");
        }
    }
}
