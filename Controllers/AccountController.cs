using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Models;

namespace Project1.Controllers
{
    public class AccountController : Controller
    {
        private readonly Project1Context obj;
        public AccountController(Project1Context context)
        {
            obj = context;
        }

        [HttpGet]
        public IActionResult Index([Bind("ID,UserName,Password")] Account account)
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Account account)
        {
                  if (ModelState.IsValid)
                    {
                     obj.Account.Add(account);
                     obj.SaveChanges();
                   }
            return RedirectToAction("","");
        }
        
        
    }
}
