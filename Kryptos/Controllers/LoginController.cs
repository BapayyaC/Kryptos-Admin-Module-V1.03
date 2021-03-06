﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kryptos.Models;
using Newtonsoft.Json;
using System.Data;

namespace Kryptos.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        kryptoEntities1 _context = new kryptoEntities1();
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult View2()
        {
            return View();
        }

        public static UserLoginInformation ActiveUser
        {
            get { return (UserLoginInformation)System.Web.HttpContext.Current.Session["ACTIVEUSER"]; }
            set
            {
                System.Web.HttpContext.Current.Session["ACTIVEUSER"] = value;
                if (value.Facility.SessionTimeout != null)
                {
                    System.Web.HttpContext.Current.Session.Timeout = (int)(value.Facility.SessionTimeout);
                    ActiveUser_SESSIONID = System.Web.HttpContext.Current.Session.SessionID;
                }
            }
        }


        public static String DomainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        public static String ActiveUser_SESSIONID { get; private set; }



        [HttpPost]
        public ActionResult LoginCredentials(UserLoginInformation obj)
        {

            var user = _context.UserLoginInformations.FirstOrDefault(x => x.EmailId.Equals(obj.EmailId) && x.Password.Equals(obj.Password));

            if (user != null)
            {
                ActiveUser = user;
                return RedirectToAction("List", "Facility");
            }
            TempData["errormsg"] = "Please Enter Correct Username And Password";
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Logout()
        {
            ActiveUser = null;
            TempData["ErrorMessage"] = "Successfully Logged Out";
            return RedirectToAction("Login", "Login");
        }

        public int SetResetPassword(string oldPassword, int userid, string newpassword)
        {
            var user = _context.UserLoginInformations.Find(userid);
            if (oldPassword != user.Password) return 2;
            user.Password = newpassword;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return 1;
        }
    }

}

