using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.DATA.EF;
using Microsoft.AspNet.Identity;

namespace FinalProject.UI.MVC.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private FinalProjectEntities db = new FinalProjectEntities();

        // GET: Reservations
        public ActionResult Index()
        {
            if (User.IsInRole("Pet Owner"))
            {
                var userId = User.Identity.GetUserId();

                var POReservations = db.Reservations.Where(x => x.Pet.OwnerId == userId).Include(r => r.Location).Include(r => r.Pet);
                return View(POReservations.ToList());
            }
            var reservations = db.Reservations.Include(r => r.Location).Include(r => r.Pet);
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        //Custom DatePick View Here ++++
        [Authorize(Roles = "Admin, Pet Owner")]
        public ActionResult DatePick()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Pet Owner")]
        [HttpPost]
        public ActionResult DatePick(DateTime resDate)
        {

            return RedirectToAction("Create", new { resDate = resDate });
        }

        
        // GET: Reservations/Create
        [Authorize(Roles ="Admin, Pet Owner")]
        public ActionResult Create(DateTime resDate)
        {
            ViewBag.ResDate = resDate; //This allows the date to be displayed in the view

            if (User.IsInRole("Pet Owner"))
            {
                //Used to filter the pets list in the case of user being a PO
                var userId = User.Identity.GetUserId();

                //This is the filtering for reservations to match the selected date and check that it's LESS than the res limit
                var locsList = db.Locations.Where(x => x.Reservations.Where(y => y.ReservationDate == resDate && y.LocationId == x.LocationId).Count() < x.ReservationLimit);
                
                ViewBag.LocationId = new SelectList(locsList, "LocationId", "LocationName");
                //This .Where ensures owners only see their own pets
                ViewBag.PetId = new SelectList(db.Pets.Where(x => x.OwnerId == userId), "PetId", "AssetName");
                return View();
            }

            //this will only run (and return the default list) if the user ISN'T a PO
            //These 2 lists aren't filtered because we don't want those restrictions to apply to admins
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
            ViewBag.PetId = new SelectList(db.Pets, "PetId", "AssetName");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,PetId,LocationId,ReservationDate,SpecialRequests")] Reservation reservation)
        {
            
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.PetId = new SelectList(db.Pets, "PetId", "AssetName", reservation.PetId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.PetId = new SelectList(db.Pets, "PetId", "AssetName", reservation.PetId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,PetId,LocationId,ReservationDate,SpecialRequests")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.PetId = new SelectList(db.Pets, "PetId", "AssetName", reservation.PetId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
