using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproje.Models;

namespace Mvcproje.Controllers
{
    public class AdminController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.makalesayisi = db.Makales.Count();
            ViewBag.yorumsayisi = db.Yorums.Count();
            ViewBag.kategorisayisi = db.Kategoris.Count();
            ViewBag.uyesayisi = db.Uyes.Count();
            return View();
        }
    }
}