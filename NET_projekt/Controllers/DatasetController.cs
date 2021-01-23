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
    public class DatasetController : Controller
    {
        private DefaultContext db = new DefaultContext();

        //HTTP: GET------------------------------------------------------------------
        public ActionResult AddDataset(int? UserId)
        {
            if (UserId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            User User = db.Users.Find(UserId);
            if (User == null) return HttpNotFound();
            return View(new Dataset { ConcreteUser = User });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDataset(HttpPostedFileBase file, string DatasetName, string DatasetColumnsInfo, int DatasetHzFrequency)
        {
            if (ModelState.IsValid) //Model-binding validation - odwołuje się do adnotacji w Modelach
            {
                { //poprawność DatasetColumnsInfo
                    string[] lines = DatasetColumnsInfo.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    DatasetColumnsInfo = "";
                    foreach (string line in lines)
                    {
                        string[] name_numbers = line.Split(new char[] { ':' });
                        if (name_numbers.Length != 2 || name_numbers[0].Length == 0)
                        {
                            ViewBag.dataseterror = "Przykładowa prawidłowa postać: ECG:1,2,3 EMG:4,5 AAA:6";
                            return View();
                        }
                        string[] potentialdigits = name_numbers[1].Split(new char[] { ',' });
                        foreach (var potentialdigit in potentialdigits)
                        {
                            if (potentialdigit.Length == 0)
                            {
                                ViewBag.dataseterror = "Przykładowa prawidłowa postać: ECG:1,2,3 EMG:4,5 AAA:6";
                                return View();
                            }
                            int number;
                            bool success = Int32.TryParse(potentialdigit, out number);
                            if (!success)
                            {
                                ViewBag.dataseterror = "Przykładowa prawidłowa postać: ECG:1,2,3 EMG:4,5 AAA:6";
                                return View();
                            }
                        }
                        DatasetColumnsInfo += line + " ";
                    }
                    DatasetColumnsInfo = DatasetColumnsInfo.Substring(0, DatasetColumnsInfo.Length - 1);
                }
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    if (extension == ".csv")
                    {
                        ViewBag.Message = "Wybrano odpowiedni plik";
                        //string path = Path.Combine(Server.MapPath("~"), Path.GetFileName(file.FileName));
                        string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path.GetDirectoryName
                            (AppDomain.CurrentDomain.BaseDirectory)), @"Dane\\" + DatasetName + Session["Nickname"].ToString() + ".csv");
                        file.SaveAs(path);
                        Dataset NewDataSet = new Dataset();
                        NewDataSet.DatasetName = DatasetName;
                        NewDataSet.DatasetColumnsInfo = DatasetColumnsInfo;
                        NewDataSet.DatasetHzFrequency = DatasetHzFrequency;
                        NewDataSet.ConcreteUser = db.Users.Find(Convert.ToInt32(Session["UserId"]));
                        NewDataSet.Reference = path.ToString();
                        NewDataSet.DateAdded = DateTime.Now;
                        db.Datasets.Add(NewDataSet);
                        db.SaveChanges();
                        return RedirectToAction("Index", "User");
                    }
                    else ViewBag.Message = "Należy wybrać plik '.csv'";
                }
                else ViewBag.Message = "Nie wybrano konkretnego pliku.";
                return View();
            }
            return View();
        }
        //HTTP: GET------------------------------------------------------------------
        public ActionResult DeleteDataset(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Dataset Dts = db.Datasets.Find(Id);
            if (Dts == null) return HttpNotFound();
            return View(Dts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDataset(int Id)
        {
            Dataset Dts = db.Datasets.Find(Id);
            //Usuwanie z folderu
            string path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName
                (AppDomain.CurrentDomain.BaseDirectory)), @"Dane\\" + Dts.DatasetName + Session["Nickname"].ToString() + ".csv");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            //Usuwanie z DB
            db.Datasets.Remove(Dts);
            db.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        //HTTP: GET------------------------------------------------------------------
        public ActionResult Graph(int? Id, int time = 0, bool previousData = false)
        {
            if (time < 0) time = 0;
            if (previousData == true) Session["StopForward"] = "false";
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dataset Dts = db.Datasets.Find(Id); //Wyszukuje odpowiedni dataset w DB
            if (Dts == null) return HttpNotFound();
            StreamReader Sr = new StreamReader(Dts.Reference); //Chwyta się odp. csv'ki
            List<String> Lines = new List<String>(); //Pusta lista na przekazane linie

            if (Convert.ToString(Session["StopForward"]) == "true") time = time - 30;
            for (int i = 0; i <= time * Dts.DatasetHzFrequency; i++) Sr.ReadLine();
            for (int i = 0; i < 30 * Dts.DatasetHzFrequency; i++) //Doczytuje tyle linii, by było na 30 sekund
            {
                if (Sr.EndOfStream == true)
                {
                    Session["StopForward"] = "true";
                    break;
                }
                string s = Sr.ReadLine();
                Lines.Add(s);
            }
            ViewBag.DataLines = JsonConvert.SerializeObject(Lines); //Wstawia linie do vievbaga by łatwo je uzyskać w widoku
            GraphModel Gm = new GraphModel { Dataset = Dts, Time = time };
            Sr.Close();
            return View(Gm);
        }


    }
}