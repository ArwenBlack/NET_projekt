using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NET_projekt.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using System.Security.Cryptography;

namespace NET_projekt.Controllers
{
    public class HomeController : Controller
    {
        private DefaultContext db = new DefaultContext();
      
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                string actsession = Session["Nickname"].ToString();
                User actuser = db.Users.Where(u => u.Nickname.Equals(actsession)).FirstOrDefault();
                //Taka ciekawostka - w Equals(...) nie można wstawić konwersji na stringa bo LINQ wywala błąd 
                return View(actuser);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Register()
        {
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
                    User NewUser = new User
                    {
                        Nickname = nickname,
                        EmailAddress = emailaddress,
                        Password = GetMD5(password)
                    };
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(NewUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Ta nazwa użytkownika jest już zajęta!";
                    return View(new User());
                }
            }
            return View(new User());
        }

        //HTTP GET
        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string nickname, string password)
        {
            if (ModelState.IsValid)
            {
                string hashedpassword = GetMD5(password);
                var PossibleUser = db.Users.Where(u => u.Nickname.Equals(nickname)).ToList();
                if(PossibleUser.Count() > 0)
                {
                    User u = PossibleUser.FirstOrDefault();
                    if(u.Password.Equals(hashedpassword))
                    {
                        Session["UserId"] = u.UserId;
                        Session["Nickname"] = u.Nickname;
                        Session["EmailAddress"] = u.EmailAddress;
                        Session["PremiumStatus"] = u.PremiumStatus;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Podano nieprawidłowe hasło!";
                        //return RedirectToAction("Login");
                        return View(new User());
                    }
                }
                else
                {
                    ViewBag.error = "Użytkownik o podanej nazwie nie istnieje.";
                    //return RedirectToAction("Login");
                    return View(new User());
                }
            }
            return View(new User());
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}