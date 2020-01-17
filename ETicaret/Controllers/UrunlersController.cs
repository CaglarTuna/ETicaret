using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ETicaret.Models;

using System.IO;

namespace ETicaret.Controllers
{
    public class UrunlersController : Controller
    {
        private ETicaretDbEntities1 db = new ETicaretDbEntities1();

        // GET: Urunlers
        public ActionResult Index()
        {
            var urunlers = db.Urunler.Include(u => u.Kategoriler);
            return View(urunlers.ToList());
        }

        // GET: Urunlers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // GET: Urunlers/Create
        public ActionResult Create()
        {
            ViewBag.RefKategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: Urunlers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [ValidateInput(false)]    /*biz ekledik*/

        public ActionResult Create([Bind(Include = "UrunId,UrunAdi,RefKategoriId,UrunAciklamasi,UrunFiyat")] Urunler urunler,HttpPostedFileBase UrunResmi)   /*HttpPostedFileBase UrunResmi: biz ekledik*/
        {
            if (ModelState.IsValid)
            {
                db.Urunler.Add(urunler);
                db.SaveChanges();

                if(UrunResmi!=null && UrunResmi.ContentLength>0) /*resim ekleme*/
                {
                    string filePath = Path.Combine(Server.MapPath("~/Resim"), urunler.UrunId + ".jpg");
                    UrunResmi.SaveAs(filePath);
                }
                /*resim klosörü olustur*/


                return RedirectToAction("Index");
            }

            ViewBag.RefKategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi", urunler.RefKategoriId);
            return View(urunler);
        }

        // GET: Urunlers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            ViewBag.RefKategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi", urunler.RefKategoriId);
            return View(urunler);
        }

        // POST: Urunlers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [ValidateInput(false)] /*attribute ekledk*/

        public ActionResult Edit([Bind(Include = "UrunId,UrunAdi,RefKategoriId,UrunAciklamasi,UrunFiyat")] Urunler urunler,HttpPostedFileBase UrunResmi) /*HttpPostedFileBase UrunResmi: ekledik*/
        {
            if (ModelState.IsValid)
            {
                db.Entry(urunler).State = EntityState.Modified;
                db.SaveChanges();

                if (UrunResmi != null && UrunResmi.ContentLength > 0) /*resim güncelleme*/
                {
                    string filePath = Path.Combine(Server.MapPath("~/Resim"), urunler.UrunId + ".jpg");
                    UrunResmi.SaveAs(filePath);
                }


                return RedirectToAction("Index");
            }
            ViewBag.RefKategoriId = new SelectList(db.Kategoriler, "KategoriId", "KategoriAdi", urunler.RefKategoriId);
            return View(urunler);
        }

        // GET: Urunlers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // POST: Urunlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Urunler urunler = db.Urunler.Find(id);
            db.Urunler.Remove(urunler);
            db.SaveChanges();
            return RedirectToAction("Index");
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
