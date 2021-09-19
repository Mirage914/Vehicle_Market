﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Project1.Data;
using Project1.Models;

namespace Project1.Controllers
{
    public class AccountController : Controller
    {
        public bool IsLogin = false;
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
        public IActionResult Register(Account account)
        {
            if (account.UserName == null || account.Password == null)
            {
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
        public IActionResult Login(Account account)
        {
            if (account.UserName == null || account.Password == null)
            {

                return View();
            }
            var user = obj.Account.Where(x => x.UserName == account.UserName && x.Password == account.Password).FirstOrDefault();
            if (user == null)
            {

                return View();
            }
            else
            {
                return RedirectToAction("", "");

            }
        }


    }
}
