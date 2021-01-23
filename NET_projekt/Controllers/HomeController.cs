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
    public class HomeController : Controller
    {
        private DefaultContext db = new DefaultContext();

        //HTTP: GET------------------------------------------------------------------
        public ActionResult Main_view()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path)), @"NET_projekt\");
            string[] filePaths = System.IO.Directory.GetFiles(path, "*.csv");
            foreach (string filePath in filePaths)

                System.IO.File.Delete(filePath);
            return View(new Example_Data());
        }
        [HttpPost]
        public ActionResult Main_view(HttpPostedFileBase file)
        {
            List<String> fields = new List<String>();
            if (Path.GetExtension(file.FileName) == ".csv")
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
            else if (Path.GetExtension(file.FileName) == ".dat")
            {
                string path = Path.Combine(Server.MapPath("~"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                Dat_to_Csv(path);
                System.IO.File.Delete(path);
                ViewBag.Message_ok = "1";
                path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path)), @"NET_projekt\test.csv");
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
                ViewBag.Message_err = "Zły format pliku, wybierz plik csv lub dat ";
                return View();
            }


            return RedirectToAction("Main_view");

        }
        private static void Dat_to_Csv(string file)
        {
            System.Data.DataTable table = new System.Data.DataTable("dataFromFile");

            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    string[] items = line.Trim().Split(',');
                    if (table.Columns.Count == 0)
                    {
                        for (int i = 0; i < items.Length; i++)
                            table.Columns.Add(new DataColumn("Column" + i, typeof(string)));
                    }
                    table.Rows.Add(items);
                }
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path)), @"NET_projekt\test.csv");
            if (table != null && table.Rows.Count > 0)
            {
                StringBuilder data = new StringBuilder();
                for (int column = 0; column < table.Columns.Count; column++)
                {
                    if (column == table.Columns.Count - 1)
                        data.Append(table.Columns[column].ColumnName.ToString().Replace(",", ";"));
                    else
                        data.Append(table.Columns[column].ColumnName.ToString().Replace(",", ";") + ',');
                }

                data.Append(Environment.NewLine);

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    for (int column = 0; column < table.Columns.Count; column++)
                    {
                        if (column == table.Columns.Count - 1)
                            data.Append(table.Rows[row][column].ToString().Replace(",", ";"));
                        else
                            data.Append(table.Rows[row][column].ToString().Replace(",", ";") + ',');
                    }
                    if (row != table.Rows.Count - 1)
                        data.Append(Environment.NewLine);
                }
                using (StreamWriter objWriter = new StreamWriter(path))
                {
                    objWriter.WriteLine(data);
                }
            }
        }
    }
}