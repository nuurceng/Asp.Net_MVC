using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproje.Models;
using PagedList;//
using PagedList.Mvc;

namespace Mvcproje.Controllers
{
    public class HomeController : Controller
    {
        mvcblogDB db = new mvcblogDB();

        // GET: Home
        public ActionResult Index(int page=1)
        {
            var makale = db.Makales.OrderByDescending(m => m.makaleid).ToPagedList(page,3);
            return View(makale);
        }
        public ActionResult BlogAra(string Ara = null)
        {
            var aranan = db.Makales.Where(m => m.baslik.Contains(Ara)).ToList();
            return View(aranan.OrderByDescending(m => m.tarih));
        }
        public ActionResult SonYorumlar()
        {
            return View(db.Yorums.OrderByDescending(y => y.yorumid).Take(5));
        }
        public ActionResult PopulerMakaleler()
        {
            return View(db.Makales.OrderByDescending(m => m.okunma).Take(10));
        }
        public ActionResult KategoriMakale(int id)
        {
            var makaleler = db.Makales.Where(m => m.Kategori.kategoriid == id).ToList();
            return View(makaleler);
        }
        public ActionResult MakaleDetay(int id)
        {
            var makale = db.Makales.Where(m => m.makaleid == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }
        public ActionResult KategoriKismi()
        {
            return View(db.Kategoris.ToList());
        }
       

        public JsonResult YorumYap(string yorum, int Makaleid)
        {

            var Uyeid = Session["Uyeid"];
            if (yorum == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            db.Yorums.Add(new Yorum { uyeid = Convert.ToInt32(Uyeid), makaleid = Makaleid, icerik = yorum, tarih = DateTime.Now });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);//yorum yapmaya izin ver
        }
        public ActionResult YorumSil(int id)
        {
            var Uyeid = Session["Uyeid"];
            var yorum = db.Yorums.Where(y => y.yorumid == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.makaleid == yorum.makaleid).SingleOrDefault();
            if (yorum.uyeid == Convert.ToInt32(Uyeid))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.makaleid });
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult OkunmaArttir(int Makaleid)
        {
            var makale = db.Makales.Where(m => m.makaleid == Makaleid).SingleOrDefault();
            makale.okunma += 1;
            db.SaveChanges();
            return View();
        }

    }
}