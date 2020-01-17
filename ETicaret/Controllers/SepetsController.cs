using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ETicaret.Models;
using Microsoft.AspNet.Identity;

namespace ETicaret.Controllers
{
    public class SepetsController : Controller
    {
        private ETicaretDbEntities1 db = new ETicaretDbEntities1();

        
        public ActionResult SepeteEkle(int? adet, int id)
        {
            string userId = User.Identity.GetUserId();
            //Aktif olan kullanıcı id si
            Sepet sepettekiUrun = db.Sepet.FirstOrDefault(a => a.RefUrunId == id && a.RefAspNetUserId == userId);
            Urunler urun = db.Urunler.Find(id);
            if(sepettekiUrun==null) //sepette urun yoksa
            {
                Sepet yeniUrun = new Sepet()
                {
                    RefAspNetUserId = userId,
                    RefUrunId = id,
                    Adet = adet ?? 1, //null ise 1 yap
                    ToplamTutar = (adet ?? 1) * urun.UrunFiyat
                   

                };
                db.Sepet.Add(yeniUrun);
            }
            else  // sepete aynı üründen 1 tane daha ekler
            {
                sepettekiUrun.Adet = sepettekiUrun.Adet + (adet ?? 1);
                sepettekiUrun.ToplamTutar = sepettekiUrun.Adet * urun.UrunFiyat;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult SepeteGuncelle(int ? adet,int id)
        {
           if(id==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sepet sepet = db.Sepet.Find(id);
            if (sepet==null)
            {
                return HttpNotFound();
            }
            Urunler urun = db.Urunler.Find(sepet.RefUrunId );
            sepet.Adet = adet ?? 1;
            sepet.ToplamTutar = sepet.Adet * urun.UrunFiyat;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Sil(int id)
        {

            Sepet sepet = db.Sepet.Find(id);
            db.Sepet.Remove(sepet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Sepets
        public ActionResult Index()
        {
            var sepets = db.Sepet.Include(s => s.AspNetUsers).Include(s => s.Urunler);
            return View(sepets.ToList());
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
