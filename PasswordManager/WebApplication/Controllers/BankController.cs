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
        public static int sessionId = 0;


        public BankController(DataAccess.Interfaces.IBankRepository bankRepository)
        {
            bankViewModel = new BankViewModel();
            _bankRepository = bankRepository;
            
        }
        
        public ActionResult Banks()
        {
            var session = HttpContext.Session.GetInt32("SessionId");
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            sessionId = (int)session;
            var banks = _bankRepository.GetBySessionId(sessionId);
            ViewBag.Banks = banks;
            if(TempData["error"] != null)
                ViewBag.error = TempData["error"].ToString();
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Update(BankViewModel bankViewModel)
        {
            var bank = _bankRepository.GetById(bankViewModel.Id);
            bank.Name = bankViewModel.Name;
            bank.NumberCard = bankViewModel.NumberCard;
            bank.Cvc = bankViewModel.Cvc;
            bank.Date = bankViewModel.Date;
            bank.SessionId = sessionId;
            TempData["error"] = "";

            var banks = _bankRepository.GetBySessionId(sessionId);
            foreach (var elt in banks)
            {
                if (elt.Name.Equals(bank.Name) &&
                    elt.Cvc.Equals(bank.Cvc) &&
                    elt.Date.Equals(bank.Date) &&
                    elt.NumberCard.Equals(bank.NumberCard))
                {
                    TempData["error"] = "this credit card already exist!";
                    return RedirectToAction("Banks", "Bank");
                }
            }

            await _bankRepository.Update(bank);

            return RedirectToAction("Banks", "Bank");
        }
        [HttpPost]
        public async Task<ActionResult> Add(BankViewModel bankViewModel)
        {
            var bank = new Dbo.Bank();
            bank.Name = bankViewModel.Name;
            bank.Cvc = bankViewModel.Cvc;
            bank.Date = bankViewModel.Date;
            bank.NumberCard = bankViewModel.NumberCard;
            bank.SessionId = sessionId;
            TempData["error"] = "";

            var banks = _bankRepository.GetBySessionId(sessionId);
            foreach(var elt in banks)
            {
                if(elt.Name.Equals(bank.Name) &&
                    elt.Cvc.Equals(bank.Cvc) &&
                    elt.Date.Equals(bank.Date) &&
                    elt.NumberCard.Equals(bank.NumberCard))
                {
                    TempData["error"] = "this credit card already exist!";
                    return RedirectToAction("Banks", "Bank");
                }
            }
            await _bankRepository.Insert(bank);

            return RedirectToAction("Banks", "Bank");
        }

        [HttpGet]
        public BankViewModel GetById(int id)
        {
            var bank = _bankRepository.GetById(id);

            var bankViewModel = new BankViewModel();
            bankViewModel.Id = bank.Id;
            bankViewModel.Name = bank.Name;
            bankViewModel.NumberCard = bank.NumberCard;
            bankViewModel.Cvc = bank.Cvc;
            bankViewModel.Date = bank.Date;

            return bankViewModel;
        }


        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            await _bankRepository.Delete(id);
            return RedirectToAction("Banks", "Bank");
        }
    }
}
