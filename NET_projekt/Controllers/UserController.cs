using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NET_projekt.Models;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Data;

namespace NET_projekt.Controllers
{
    public class UserController : Controller
    {
        private DefaultContext db = new DefaultContext();
        //HTTP: GET------------------------------------------------------------------
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                string actsession = Session["Nickname"].ToString();
                User actuser = db.Users.Where(u => u.Nickname.Equals(actsession)).FirstOrDefault();
                return View(actuser);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        //HTTP: GET------------------------------------------------------------------
        public ActionResult Register()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "User");
            }
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string nickname, string emailaddress, string password)
        {
            if (ModelState.IsValid)
            {
                var ExistingUsername = db.Users.Where(u => u.Nickname.Equals(nickname)).ToList();
                if (ExistingUsername.Count() == 0)
                {
                    var tuple_p = hash(password);
                    User NewUser = new User
                    {

                        Nickname = nickname,
                        EmailAddress = emailaddress,
                        Password = tuple_p.Item2,
                        Salt = tuple_p.Item1

                    };
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(NewUser);
                    db.SaveChanges();
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ViewBag.error = "Ta nazwa użytkownika jest już zajęta!";
                    return View(new User());
                }
            }
            return View();
        }

        //HTTP: GET------------------------------------------------------------------
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "User");
            }
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string nickname, string password)
        {

            if (ModelState.IsValid)
            {

                var PossibleUser = db.Users.Where(u => u.Nickname.Equals(nickname)).ToList();
                if (PossibleUser.Count() > 0)
                {
                    User u = PossibleUser.FirstOrDefault();
                    string salted_password = password + u.Salt;
                    string hash_password = only_hash(salted_password);
                    if (u.Password.Equals(hash_password))
                    {
                        Session["UserId"] = u.UserId;
                        Session["Nickname"] = u.Nickname;
                        Session["EmailAddress"] = u.EmailAddress;
                        Session["PremiumStatus"] = u.PremiumStatus;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Podano nieprawidłowy login lub hasło";
                        //return RedirectToAction("Login");
                        return View(new User());
                    }
                }
                else
                {
                    ViewBag.error = "Podano nieprawidłowy login lub hasło";
                    //return RedirectToAction("Login");
                    return View(new User());
                }
            }
            return View(new User());
        }
        public (string, string) hash(string password)
        {
            //salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[32]);
            string salt_string = Convert.ToBase64String(salt);
            string salted_password = password + salt_string;

            // hash
            HashAlgorithm hasgAlg = new SHA256CryptoServiceProvider();
            byte[] Value = System.Text.Encoding.UTF8.GetBytes(salted_password);
            byte[] Hash = hasgAlg.ComputeHash(Value);
            string hashed_password = Convert.ToBase64String(Hash);
            return (salt_string, hashed_password);
        }
        public string only_hash(string salted_password)
        {
            HashAlgorithm hasgAlg = new SHA256CryptoServiceProvider();
            byte[] Value = System.Text.Encoding.UTF8.GetBytes(salted_password);
            byte[] Hash = hasgAlg.ComputeHash(Value);
            string hashed_password = Convert.ToBase64String(Hash);
            return hashed_password;
        }

        //HTTP: GET------------------------------------------------------------------
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}