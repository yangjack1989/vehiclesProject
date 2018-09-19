﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using vehiclesProject.Models;

namespace vehiclesProject.Controllers
{
    public class FinancialReportController : Controller
    {
        private Connection db = new Connection();

        // GET: FinancialReport
        public ActionResult Index()
        {
            var vehicles = db.Vehicles.Include(v => v.Make).Include(v => v.Model).Include(v => v.Model.VehicleType);
            var filtered = vehicles.Where(a => a.SoldDate.HasValue);
            ViewBag.sn = vehicles.Where(a => a.SoldDate.HasValue).Count().ToString();
            ViewBag.an = vehicles.Where(a => a.SoldDate == null).Count().ToString();
            ViewBag.income = filtered.Sum(a=>a.Price).ToString();
            return View();
            
        }

        // GET: FinancialReport/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: FinancialReport/Create
        public ActionResult Create()
        {
            ViewBag.MakeId = new SelectList(db.Makes, "MakeId", "Name");
            ViewBag.ModelId = new SelectList(db.Models, "ModelId", "Colour");
            return View();
        }

        // POST: FinancialReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VehicleId,MakeId,ModelId,Year,Price,SoldDate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MakeId = new SelectList(db.Makes, "MakeId", "Name", vehicle.MakeId);
            ViewBag.ModelId = new SelectList(db.Models, "ModelId", "Colour", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: FinancialReport/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.MakeId = new SelectList(db.Makes, "MakeId", "Name", vehicle.MakeId);
            ViewBag.ModelId = new SelectList(db.Models, "ModelId", "Colour", vehicle.ModelId);
            return View(vehicle);
        }

        // POST: FinancialReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleId,MakeId,ModelId,Year,Price,SoldDate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MakeId = new SelectList(db.Makes, "MakeId", "Name", vehicle.MakeId);
            ViewBag.ModelId = new SelectList(db.Models, "ModelId", "Colour", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: FinancialReport/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: FinancialReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
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
