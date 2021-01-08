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
using System.Net;
using System.IO;

namespace NET_projekt.Controllers
{
    public class HomeController : Controller
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
                return RedirectToAction("Login");
            }
        }
        //HTTP: GET------------------------------------------------------------------
        public ActionResult Graph(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dataset Dts = db.Datasets.Find(Id);
            if (Dts == null) return HttpNotFound();
            StreamReader Sr = new StreamReader(Dts.Reference);
            List<String> Lines = new List<String>();
            Sr.ReadLine();
            for (int i = 0; i < 50; i++)
            {
                string s = Sr.ReadLine();
                Lines.Add(s);
            }
            ViewBag.DataLines = JsonConvert.SerializeObject(Lines);
            return View(Dts);
        }
        //HTTP: GET------------------------------------------------------------------
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

        //HTTP: GET------------------------------------------------------------------
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

        //HTTP: GET------------------------------------------------------------------
        public ActionResult Main_view()
        {
            return View(new Example_Data());
        }

        //HTTP: GET------------------------------------------------------------------
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        
        public ActionResult Example_plot(bool ecg, bool emg, int choose_Hz, int time)
        {
            string path = @"../../Dane";
            List<DataPoint> ECG = new List<DataPoint>();
            List<DataPoint> EMG = new List<DataPoint>();

            double a = 0;
            switch (choose_Hz)
            {
                case 10:
                    path = AppDomain.CurrentDomain.BaseDirectory;
                    path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path)), @"Dane\ecg_emg_10Hz_30s.csv");
                    a = 0.1;
                    break;
                case 100:
                    path = AppDomain.CurrentDomain.BaseDirectory;
                    path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path)), @"Dane\ecg_emg_100Hz_30s.csv");
                    a = 0.01;
                    break;

                case 250:
                    path = AppDomain.CurrentDomain.BaseDirectory;
                    path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path)), @"Dane\ecg_emg_250Hz_30s.csv");
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
                while (!csvParser.EndOfData && ile <= time)
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

        [HttpPost]
        public ActionResult Main_view(HttpPostedFileBase file)
        {
            List<String> fields = new List<String>();
            if ( Path.GetExtension(file.FileName) == ".csv")
            {
                ViewBag.Message_ok = "1";
                string path = Path.Combine(Server.MapPath("~"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                using (TextFieldParser csvParser = new TextFieldParser(path))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    string[] f = csvParser.ReadFields();
                    fields = f.ToList();
                }
                ViewBag.Fields = fields;
                Session["fields"] = fields;
                Session["path"] = path;
                return View();
            }
            else
            {
                ViewBag.Message_err = "Zły format pliku, wybierz plik csv ";
                return View();
            }
            

            return RedirectToAction("Main_view");

        }
        
        public ActionResult User_plot(List<String> data_list, int choose_Hz, int start_time, int wyw)
        {
            
            Session["choose_Hz"] = choose_Hz;
            Session["start_time"] = start_time;
            string[] fields_name = new string[8];
            int end_time = start_time + 30;
            List<List<DataPoint>> user_data = new List<List<DataPoint>>();
            var path = (string)Session["path"];
            var fields = (List<String>)Session["fields"];
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.ReadLine();
                double skok = 1 / Convert.ToDouble(choose_Hz);
                for (int i = 1; i < fields.Count; i++)
                {
                    
                    double ile = start_time;
                    List<DataPoint> pom = new List<DataPoint>();
                    user_data.Add(pom);
                    double count = 0;
                    while (!csvParser.EndOfData && ile < end_time)
                    {
                        if (count >= start_time)
                        {
                            string[] f = csvParser.ReadFields();
                            user_data[i - 1].Add(new DataPoint(ile, Convert.ToDouble(f[i])));
                            ile += skok;
                        }
                        else
                        {
                            csvParser.ReadLine();
                            count += skok;
                        }
                    }
                    
                }    
            }
            List<DataPoint> pom1 = new List<DataPoint>();
            for (int i = fields.Count; i <= 8; i++)
            {
                user_data.Add(pom1);

            }
            if (wyw == 0)
            {
                for (int i = 0; i < data_list.Count; i++)
                {
                    fields_name[i] = data_list[i];
                }
                for (int i = data_list.Count; i < 8; i++)
                {
                    fields_name[i] = "";

                }
                Session["data_list"] = data_list;
            }
            else
            {
                var data = (List<String>)Session["data_list_2"];
                for (int i = 0; i < data.Count; i++)
                {
                    fields_name[i] = data[i];
                }
                for (int i = data.Count; i < 8; i++)
                {
                    fields_name[i] = "";
                }
                Session["data_list"] = data;
            }
            ViewBag.UserData1 = JsonConvert.SerializeObject(user_data[0]);
            ViewBag.UserData2 = JsonConvert.SerializeObject(user_data[1]);
            ViewBag.UserData3 = JsonConvert.SerializeObject(user_data[2]);
            ViewBag.UserData4 = JsonConvert.SerializeObject(user_data[3]);
            ViewBag.UserData5 = JsonConvert.SerializeObject(user_data[4]);
            ViewBag.UserData6 = JsonConvert.SerializeObject(user_data[5]);
            ViewBag.UserData7 = JsonConvert.SerializeObject(user_data[6]);
            ViewBag.UserData8 = JsonConvert.SerializeObject(user_data[7]);
            ViewBag.Fields1 = fields_name;
            return View();
        }

        public ActionResult Next_data()
        {
            int time = Convert.ToInt32(Session["start_time"]) + 30;
            Session["data_list_2"] = Session["data_list"];
            return RedirectToAction("User_plot", new { choose_Hz = Session["choose_Hz"], start_time = time, wyw=1 }) ;
        }
        public ActionResult Previous_data()
        {
            int time = Convert.ToInt32(Session["start_time"]) - 30;
            Session["data_list_2"] = Session["data_list"];
            return RedirectToAction("User_plot", new {choose_Hz = Session["choose_Hz"], start_time = time, wyw=1});
        }
        //Additional methods-----------------------------------------------------
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
    }
}