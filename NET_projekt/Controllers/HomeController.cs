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
using System.Net.Http.Headers;
using System.Web.Razor.Generator;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.WebControls;
using System.Drawing.Printing;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace NET_projekt.Controllers
{
    public class HomeController : Controller
    {
        private DefaultContext db = new DefaultContext();


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


        public ActionResult Main_view()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Example_plot(bool ecg, bool emg, int choose_Hz, int time)
        {
            string path = @"C:\\Users\\arwen\\source\\repos\\NET_projekt\\Dane";
            List<DataPoint> ECG = new List<DataPoint>();
            List<DataPoint> EMG = new List<DataPoint>();
            double a = 0;
            switch (choose_Hz)
            {
                case 10:
                    path = @"C:\\Users\\arwen\\source\\repos\\NET_projekt\\Dane\\ecg_emg_10Hz_30s.csv";
                    a = 0.1;
                    break;
                case 100:
                    path = @"C:\\Users\\arwen\\source\\repos\\NET_projekt\\Dane\\ecg_emg_100Hz_30s.csv";
                    a = 0.01;
                    break;

                case 250:
                    path = @"C:\\Users\\arwen\\source\\repos\\NET_projekt\\Dane\\ecg_emg_250Hz_30s.csv";
                    a = 0.004;
                    break;
                defult:
                    break;
            }
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.ReadLine();
                double ile = 0;
                while (!csvParser.EndOfData && ile<=time)
                {
                    string[] fields = csvParser.ReadFields();
                    
                    ECG.Add(new DataPoint(ile, Convert.ToDouble(fields[1])));
                    
                    EMG.Add(new DataPoint(ile, Convert.ToDouble(fields[2])));
                    ile += a;
                }
            }
            if (ecg == true)
            {
                ViewBag.DataPoints = JsonConvert.SerializeObject(ECG);
                ViewBag.MyMessage = "ECG";
            }
            if (emg == true)
            {
                ViewBag.DataPoints = JsonConvert.SerializeObject(EMG);
                ViewBag.MyMessage = "EMG";
            }

            return View();
        }
    }
}