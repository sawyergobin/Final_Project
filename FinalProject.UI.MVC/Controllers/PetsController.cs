using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.DATA.EF;
using FinalProject.UI.MVC.Utilities;
using Microsoft.AspNet.Identity;

namespace FinalProject.UI.MVC.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {
        private FinalProjectEntities db = new FinalProjectEntities();

        // GET: Pets
        public ActionResult Index()
        {
            if (User.IsInRole("Pet Owner"))
            {
                var userId = User.Identity.GetUserId();

                var POPets = db.Pets.Where(x => x.OwnerId == userId).Include(p => p.UserDetail);
                return View(POPets.ToList());
            }
            var pets = db.Pets.Include(p => p.UserDetail);
            return View(pets.ToList());
        }

        // GET: Pets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // GET: Pets/Create
        [Authorize(Roles = "Admin, Pet Owner")]
        public ActionResult Create()
        {
            if (User.IsInRole("Pet Owner"))
            {
                var userId = User.Identity.GetUserId();
                ViewBag.OwnerId = new SelectList(db.UserDetails.Where(x => x.UserId == userId), "UserId", "FullName");
                return View();

            }

            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FullName");
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Pet Owner")]
        public ActionResult Create([Bind(Include = "PetId,AssetName,OwnerId,AssetPhoto,SpecialNotes,IsActive,DateAdded")] Pet pet, HttpPostedFileBase petImg)
        {
            pet.DateAdded = DateTime.Now;

            if (ModelState.IsValid)
            {
                #region File Upload
                string file = "NoImage.png";

                if (petImg != null)
                {
                    file = petImg.FileName;
                    string ext = file.Substring(file.LastIndexOf('.'));
                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    //check that the uploaded file is in our approved list of extensions
                    if (goodExts.Contains(ext.ToLower()) && petImg.ContentLength <= 4194304)//check thet the file is less than 4mb, is the default allowed file size by .NET
                    {
                        //greate a new file name using a GUID
                        file = Guid.NewGuid() + ext;
                        #region Rezise Image
                        string savePath = Server.MapPath("~/Content/images/pets/");

                        Image convertedImage = Image.FromStream(petImg.InputStream);
                        int maxImageSize = 500; //the full size image width
                        int maxThumbSize = 100; //thumbnail size width

                        ImageService.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);
                        #endregion
                    }

                }
                //no matter what update the photo url with the value of the file variable
                pet.AssetPhoto = file;
                #endregion


                db.Pets.Add(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //This sends back the filtered ownerid (for POs) if the object doesn't pass validation
            if (User.IsInRole("Pet Owner"))
            {
                var userId = User.Identity.GetUserId();
                ViewBag.OwnerId = new SelectList(db.UserDetails.Where(x => x.UserId == userId), "UserId", "FullName");
                return View();

            }

            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FullName", pet.OwnerId);
            return View(pet);
        }

        // GET: Pets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Pet Owner"))
            {
                var userId = User.Identity.GetUserId();
                ViewBag.OwnerId = new SelectList(db.UserDetails.Where(x => x.UserId == userId), "UserId", "FullName", pet.OwnerId);
                return View(pet);
            }

            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FullName", pet.OwnerId);
            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PetId,AssetName,OwnerId,AssetPhoto,SpecialNotes,IsActive,DateAdded")] Pet pet, HttpPostedFileBase petImg)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                string file = "NoImage.png";

                if (petImg != null)
                {
                    file = petImg.FileName;
                    string ext = file.Substring(file.LastIndexOf('.'));
                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    //check that the uploaded file is in our approved list of extensions
                    if (goodExts.Contains(ext.ToLower()) && petImg.ContentLength <= 4194304)//check thet the file is less than 4mb, is the default allowed file size by .NET
                    {
                        //greate a new file name using a GUID
                        file = Guid.NewGuid() + ext;
                        #region Rezise Image
                        string savePath = Server.MapPath("~/Content/images/pets/");

                        Image convertedImage = Image.FromStream(petImg.InputStream);
                        int maxImageSize = 500; //the full size image width
                        int maxThumbSize = 100; //thumbnail size width

                        ImageService.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);
                        #endregion
                        if (pet.AssetPhoto != null && pet.AssetPhoto != "NoImage.png")
                        {
                            //string path = Server.MapPath("~/Content/imgstore/books/");
                            ImageService.Delete(savePath, pet.AssetPhoto);

                        }
                        pet.AssetPhoto = file;
                    }

                }
                //no matter what update the photo url with the value of the file variable

                #endregion

                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (User.IsInRole("Pet Owner"))
            {
                var userId = User.Identity.GetUserId();
                ViewBag.OwnerId = new SelectList(db.UserDetails.Where(x => x.UserId == userId), "UserId", "FirstName", pet.OwnerId);
                return View(pet);
            }

            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FirstName", pet.OwnerId);
            return View(pet);
        }

        // GET: Pets/Delete/5
        [Authorize(Roles = "Admin, Pet Owner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            pet.IsActive = false;
            db.Entry(pet).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Uncomment this when hard deleting
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
