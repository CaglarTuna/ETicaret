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
    public class SiparisController : Controller
    {
        private ETicaretDbEntities1 db = new ETicaretDbEntities1();

        // GET: Siparis
        public ActionResult Index()
        {
            var siparis = db.Siparis.Include(s => s.AspNetUsers);
            return View(siparis.ToList());
        }


        //AKBANK
        public ActionResult SiparisTamamla()
        {
            string userId = User.Identity.GetUserId();
            IEnumerable<Sepet> sepetUrunleri = db.Sepet.Where(a => a.RefAspNetUserId == userId).ToList();

            string ClientId = "100300000";
            string Amount = sepetUrunleri.Sum(a => a.ToplamTutar).ToString();
            string Oid=string.Format ("{0:yyyyMMddmmsss}", DateTime.Now);
            string OnayUrl = "http://localhost:1125/Siparis/Tamamlandi";
            string HataUrl = "http://localhost:1125/Siparis/Hatali";

            string RDN = "SDADA";//HSN karsılastırması için eklenen rasgele dize
            string StoreKey = "123456";// bankanın sanal pos sayfasından alınıyor
            string TransActionType = "Auth";//sabit değişmiyor
            string Instalment = "";
            string HshStr = ClientId + Oid + Amount + OnayUrl + HataUrl + TransActionType + Instalment + RDN + StoreKey;

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] HashBytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(HshStr);
            byte[] InputBytes = sha.ComputeHash(HashBytes);
            string Hash =Convert.ToBase64String(InputBytes);

            ViewBag.ClientId = ClientId;
            ViewBag.Oid = Oid;
            ViewBag.okUrl = OnayUrl;
            ViewBag.failUrl = HataUrl;
            ViewBag.TransActionType = TransActionType;
            ViewBag.RDN = RDN;
            ViewBag.Hash = Hash;
            ViewBag.Amount = Amount;
            ViewBag.StoreType = "3d_pay_hosting";//ödeme modelimiz biz buna göre anlatıyoruz
            ViewBag.Description = "";
            ViewBag.XID = "";
            ViewBag.Lang = "tr";
            ViewBag.Email = "destek@destek.com";
            ViewBag.UserId = "mdurmaz";//bu id yi bankanın sanala pos ekranından biz olusturuyoruz
            ViewBag.PostURL = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";



            return View();
        }


        [HttpPost]
        public ActionResult Tamamlandi()
        {
            string UserId = User.Identity.GetUserId();
            Siparis siparis = new Siparis();
            siparis.Ad = Request.Form.Get("Ad");
            siparis.Soyad = Request.Form.Get("Soyad");
            siparis.Adres = Request.Form.Get("Adres");
            siparis.Tarih = DateTime.Now;
            siparis.TcKimlikNo = Request.Form.Get("TcKimlikNo");
            siparis.Telefon = Request.Form.Get("Telefon");
            siparis.EPosta = Request.Form.Get("EPosta");

            siparis.RefAspNetUserId = UserId;

            IEnumerable<Sepet> sepettekiUrunler = db.Sepet.Where(a => a.RefAspNetUserId == UserId).ToList();


            foreach (Sepet sepetUrunu in sepettekiUrunler)
            {
                SiparisKalem yenikalem = new SiparisKalem()
                {
                    Adet = sepetUrunu.Adet,
                    ToplamTutar=sepetUrunu.ToplamTutar,
                    RefUrunId=sepetUrunu.RefUrunId
                };
                siparis.SiparisKalem.Add(yenikalem);
                db.Sepet.Remove(sepetUrunu);

            }
            db.Siparis.Add(siparis);
            db.SaveChanges();
            return View();


        }

        public ActionResult Hatali()
        {
            ViewBag.Hata = Request.Form;
            return View();
        }


        // GET: Siparis/Details/5
        public ActionResult SiparisDetay(int id)
        {
            var SipariDetay = db.SiparisKalem.Where(a => a.RefSiparisId == id).ToList();
            return View(SipariDetay.ToList());
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
