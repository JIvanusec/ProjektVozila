using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjektVozila.Models;

namespace ProjektVozila.Controllers
{
    public class VehicleMakesController : Controller
    {
        private BazaVozilaEntities db = new BazaVozilaEntities();

        // GET: VehicleMakes
        public ActionResult Index( string sortOrder)
        {
            ViewBag.IdSortParametar = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.MarkaSortParametar = sortOrder == "Naziv marke" ? "naziv_desc" : "Naziv marke";
            var vehicleMakes = from v in db.VehicleMakes
                        select v;

            switch (sortOrder)
            {
                case "id_desc":
                    vehicleMakes = vehicleMakes.OrderByDescending(v => v.Id);
                        break;
                case "Naziv marke":
                    vehicleMakes = vehicleMakes.OrderBy(v => v.Name);
                        break;
                case "naziv_desc":
                    vehicleMakes = vehicleMakes.OrderByDescending(v => v.Name);
                        break;
                default:
                    vehicleMakes = vehicleMakes.OrderBy(v => v.Id);
                        break;
            }

            return View(vehicleMakes.ToList());
        }

        // GET: VehicleMakes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehicleMake = db.VehicleMakes.Find(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }
            return View(vehicleMake);
        }

        // GET: VehicleMakes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleMakes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Abbreviation")] VehicleMake vehicleMake)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.VehicleMakes.Add(vehicleMake);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Poruka ukoliko dođe do errora pri unosu.
                ModelState.AddModelError("", "Došlo je do greške. Pokušajte ponovo, ako ponovo dođe do greške provjerite unesene podatke.");
            }
            return View(vehicleMake);
        }

        // GET: VehicleMakes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehicleMake = db.VehicleMakes.Find(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }
            return View(vehicleMake);
        }

        // POST: VehicleMakes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var VehicleMake = db.VehicleMakes.Find(id);
            if (TryUpdateModel(VehicleMake, "", new string[] { "Name", "Abbreviation" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Nije moguće spremiti promjene. Pokušajte ponovo.");
                }
            }
            return View(VehicleMake);
        }
        // GET: VehicleMakes/Delete/5
        public ActionResult Delete(int? id, bool? spremipromjene=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (spremipromjene.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Brisanje nije uspjelo. Pokušajte ponovo";
            }
            VehicleMake vehicleMake = db.VehicleMakes.Find(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }
            return View(vehicleMake);
        }

        // POST: VehicleMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                VehicleMake brisanjeVozila = new VehicleMake() { Id = id };
                db.Entry(brisanjeVozila).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, spremipromjene = true });
            }
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
