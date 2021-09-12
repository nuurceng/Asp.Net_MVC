using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproje.Models;
using System.Web.Helpers;
using System.IO;

namespace Mvcproje.Controllers
{
    public class UyeController : Controller
    {

        mvcblogDB db = new mvcblogDB();
        // GET: Uye
        public ActionResult Index(int id)
        {
            var uye = db.Uyes.Where(u => u.uyeid == id).SingleOrDefault();
            if(Convert.ToInt32(Session["Uyeid"])!=uye.uyeid)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye uye)
        {

            var login = db.Uyes.Where(u => u.kullaniciadi == uye.kullaniciadi).SingleOrDefault();
            if(login.kullaniciadi==uye.kullaniciadi && login.email==uye.email && login.sifre==uye.sifre)
            {
                Session["Uyeid"] = login.uyeid;
                Session["Kullaniciadi"] = login.kullaniciadi;
                Session["Yetkiid"] = login.yetkiid;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Uyari = "Kullanıcı adı,mail ya da şifrenizi kontrol ediniz!!";
                return View();
            }
            
        }

        public ActionResult Logout()
        {
            Session["Uyeid"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create (Uye uye)
        {
            if(ModelState.IsValid)
            {
                uye.yetkiid = 2;
                db.Uyes.Add(uye);
                db.SaveChanges();
                Session["Uyeid"] = uye.uyeid;
                Session["Kullaniciadi"] = uye.kullaniciadi;
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View(uye);
            }
          
        }

        public ActionResult Edit(int id)
        {
            var uye = db.Uyes.Where(u => u.uyeid == id).SingleOrDefault();
            if (Convert.ToInt32(Session["Uyeid"]) != uye.uyeid)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        [HttpPost]
        public ActionResult Edit(Uye uye,int id)
        {
            if(ModelState.IsValid)
            {
                var uyeguncelle = db.Uyes.Where(u => u.uyeid == id).SingleOrDefault();
                uyeguncelle.kullaniciadi = uye.kullaniciadi;
                uyeguncelle.sifre = uye.sifre;
                uyeguncelle.email = uye.email;
                uyeguncelle.adsoyad = uye.adsoyad;
                db.SaveChanges();
                Session["Kullaniciadi"] = uye.kullaniciadi;
                return RedirectToAction("Index", "Home");/*,new { id = uyeguncelle }*/
            }
            return View();
        }
    }
}