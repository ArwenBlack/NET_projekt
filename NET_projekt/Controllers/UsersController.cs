using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NET_projekt.Models;

namespace NET_projekt.Controllers
{
    public class UsersController : Controller
    {
        private DefaultContext db = new DefaultContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
