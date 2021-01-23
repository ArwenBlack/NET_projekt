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
    public class ExamplePlotController : Controller
    {
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

                    ECG.Add(new DataPoint(ile, double.Parse(fields[1], System.Globalization.CultureInfo.InvariantCulture)));

                    EMG.Add(new DataPoint(ile, double.Parse(fields[2], System.Globalization.CultureInfo.InvariantCulture)));
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
                            user_data[i - 1].Add(new DataPoint(ile, double.Parse(f[i], System.Globalization.CultureInfo.InvariantCulture)));
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
            return RedirectToAction("User_plot", new { choose_Hz = Session["choose_Hz"], start_time = time, wyw = 1 });
        }
        public ActionResult Previous_data()
        {
            int time = Convert.ToInt32(Session["start_time"]) - 30;
            Session["data_list_2"] = Session["data_list"];
            return RedirectToAction("User_plot", new { choose_Hz = Session["choose_Hz"], start_time = time, wyw = 1 });
        }
    }
}