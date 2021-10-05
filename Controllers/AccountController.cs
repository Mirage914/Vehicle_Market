using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
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
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Account account, AccountViewModel _account)
        {

            if (_account.UserName == null || _account.Password == null)
            {
                return View();

            }
            account.UserName = _account.UserName;
            account.Password = _account.Password;
            var user = obj.Account.Where(x => x.UserName == _account.UserName).FirstOrDefault();
            if (user != null)
            {
                ViewBag.message = "Account with this username already exist";
                return View();
            }
            else if (ModelState.IsValid)
            {
                obj.Account.Add(account);
                obj.SaveChanges();
            }
            return RedirectToAction("", "");
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountViewModel _account, Account account)
        {

            if (_account.UserName == null || _account.Password == null)
            {

                return View();
            }
            account.UserName = _account.UserName;
            account.Password = _account.Password;
            var user = obj.Account.Where(x => x.UserName == account.UserName && x.Password == account.Password).FirstOrDefault();
            if (user == null)
            {
                ViewBag.message = "This account doesn't exist.";
                return View();
            }
            else
            {
                return RedirectToAction("", "");

            }
        }


    }
}
