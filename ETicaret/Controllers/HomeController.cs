using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Models;

namespace ETicaret.Controllers
{
    public class HomeController : Controller
    {
        ETicaretDbEntities1 db = new ETicaretDbEntities1();

        public ActionResult Index()
        {
            ViewBag.KategoriListesi = db.Kategoriler.ToList();
            ViewBag.SonUrunler = db.Urunler.OrderByDescending(a => a.UrunId).Skip(0).Take(12).ToList();

            return View();
        }

        public ActionResult Urun(int id)
        {
            ViewBag.KategoriListesi = db.Kategoriler.ToList();

            return View(db.Urunler.Find(id));
        }

        public ActionResult Kategori(int id)
        {
            ViewBag.KategoriListesi = db.Kategoriler.ToList();
            ViewBag.Kategori = db.Kategoriler.Find(id);
            return View(db.Urunler.Where(a=>a.RefKategoriId==id).OrderBy(a=>a.UrunAdi).ToList());
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
    }
}