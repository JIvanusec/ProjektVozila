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
    public class VehicleModelsController : Controller
    {
        private BazaVozilaEntities db = new BazaVozilaEntities();

        // GET: VehicleModels
        public ActionResult Index(string sortOrderModele)
        {
            ViewBag.ModelSortParametar = String.IsNullOrEmpty(sortOrderModele) ? "Model_desc" : "";
            ViewBag.ModelsortParametar = sortOrderModele == "Naziv modela" ? "naziv_desc" : "Naziv modela";
            var vehicleModel = from v in db.VehicleModels
                               select v;

            switch (sortOrderModele)
            {
                case "Model_desc":
                    vehicleModel = vehicleModel.OrderByDescending(v => v.ModelName);
                    break;
                case "Naziv modela":
                    vehicleModel = vehicleModel.OrderBy(v => v.ModelName);
                    break;
            }

            return View(vehicleModel.ToList());
        }
        // GET: VehicleModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }

        // GET: VehicleModels/Create
        public ActionResult Create()
        {
            ViewBag.MakeId = new SelectList(db.VehicleMakes, "Id", "Name");
            return View();
        }

        // POST: VehicleModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ModelName,Abbreviation,MakeId")] VehicleModel vehicleModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.VehicleModels.Add(vehicleModel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Poruka ukoliko dođe do errora pri unosu.
                ModelState.AddModelError("", "Došlo je do greške. Pokušajte ponovo, ako ponovo dođe do greške provjerite unesene podatke.");
            }
            ViewBag.MakeId = new SelectList(db.VehicleMakes, "Id", "Name", vehicleModel.MakeId);
            return View(vehicleModel);
        }

        // GET: VehicleModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.MakeId = new SelectList(db.VehicleMakes, "Id", "Name", vehicleModel.MakeId);
            return View(vehicleModel);
        }

        // POST: VehicleModels/Edit/5
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
            var VehicleModel = db.VehicleModels.Find(id);
            if (TryUpdateModel(VehicleModel, "", new string[] { "ModelName", "Abbreviation","MakeId"}))
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
            return View(VehicleModel);
        }

            // GET: VehicleModels/Delete/5
            public ActionResult Delete(int? id,bool? spremipromjene= false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (spremipromjene.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Brisanje nije uspjelo. Pokušajte ponovo";
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }
        // POST: VehicleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                VehicleModel brisanjeVozila = new VehicleModel() { Id = id};
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
