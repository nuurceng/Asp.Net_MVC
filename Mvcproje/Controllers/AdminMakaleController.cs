using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproje.Models;//
using System.Web.Helpers;
using System.IO;
using PagedList;//
using PagedList.Mvc;


namespace Mvcproje.Controllers
{
    public class AdminMakaleController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: AdminMakale
        public ActionResult Index(int Page=1)
        {
            var makales = db.Makales.OrderByDescending(m=>m.makaleid).ToPagedList(Page,5);
            return View(makales);
        }

      
        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.kategoriid = new SelectList(db.Kategoris,"kategoriid","kategoriadi");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Makale makale,string etiketler,HttpPostedFileBase makaleresim)/*FormCollection collection*/
        {
            if(ModelState.IsValid)
            {
                if(makaleresim!=null)
                {
                    WebImage img = new WebImage(makaleresim.InputStream);
                    FileInfo resiminfo = new FileInfo(makaleresim.FileName);
                    string newresim = Guid.NewGuid().ToString() + resiminfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleResim/" + newresim);
                    makale.makaleresim = "~/Uploads/MakaleResim/" + newresim;

                }
                


                if (etiketler!=null)
                {
                    string[] etiketdizi = etiketler.Split(',');
                    foreach (var i in etiketdizi)
                    {
                        var yenietiket = new Etiket { etiketadi = i };
                        db.Etikets.Add(yenietiket);
                        makale.Etikets.Add(yenietiket);
                    }
                }
                makale.tarih = DateTime.Now;
                makale.uyeid =Convert.ToInt32(Session["Uyeid"]);
                makale.okunma = 1;
                db.Makales.Add(makale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                // TODO: Add insert logic here

                return View(makale);
           
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {
            var makale = db.Makales.Where(m => m.makaleid == id).SingleOrDefault();
            if(makale==null)
            {
                return HttpNotFound();
            }
            ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriadi", makale.kategoriid);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, HttpPostedFileBase makaleresim,Makale makale )
        {
            try
            {
                // TODO: Add update logic here

                var guncellemakale = db.Makales.Where(m => m.makaleid == id).SingleOrDefault();
                if(makaleresim!=null)
                {
                    if(System.IO.File.Exists(Server.MapPath(guncellemakale.makaleresim)))
                    {
                        System.IO.File.Delete(Server.MapPath(guncellemakale.makaleresim));
                        WebImage img = new WebImage(makaleresim.InputStream);
                        FileInfo resiminfo = new FileInfo(makaleresim.FileName);
                        string newresim = Guid.NewGuid().ToString() + resiminfo.Extension;
                        img.Resize(800, 350);
                        img.Save("~/Uploads/MakaleResim/" + newresim);
                        guncellemakale.makaleresim = "~/Uploads/MakaleResim/" + newresim;
                    }
                   
                   
                }
                    guncellemakale.baslik = makale.baslik;
                    guncellemakale.icerik = makale.icerik;
                    guncellemakale.kategoriid = makale.kategoriid;
                    guncellemakale.tarih = makale.tarih;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
            }
            catch
            {
                ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriadi", makale.kategoriid);
                return View(makale);
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var makale = db.Makales.Where(m => m.makaleid == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var makale = db.Makales.Where(m => m.makaleid == id).SingleOrDefault();
                if (makale == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(makale.makaleresim)))
                {
                    System.IO.File.Delete(Server.MapPath(makale.makaleresim));
                }

                foreach(var i in makale.Yorums.ToList())
                {
                    db.Yorums.Remove(i);
                }
                foreach (var i in makale.Etikets.ToList())
                {
                    db.Etikets.Remove(i);
                }
                db.Makales.Remove(makale);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
