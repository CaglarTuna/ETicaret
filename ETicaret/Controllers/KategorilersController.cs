﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ETicaret.Models;

namespace ETicaret.Controllers
{
    public class KategorilersController : Controller
    {
        private ETicaretDbEntities1 db = new ETicaretDbEntities1();

        // GET: Kategorilers
        public ActionResult Index()
        {
            return View(db.Kategoriler.ToList());
        }

        // GET: Kategorilers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // GET: Kategorilers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategorilers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KategoriId,KategoriAdi")] Kategoriler kategoriler)
        {
            if (ModelState.IsValid)
            {
                db.Kategoriler.Add(kategoriler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategoriler);
        }

        // GET: Kategorilers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // POST: Kategorilers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategoriId,KategoriAdi")] Kategoriler kategoriler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategoriler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategoriler);
        }

        // GET: Kategorilers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // POST: Kategorilers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            db.Kategoriler.Remove(kategoriler);
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
