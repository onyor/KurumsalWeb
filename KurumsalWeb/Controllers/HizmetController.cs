using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HizmetController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Hizmet
        public ActionResult Index()
        {
            return View(db.Hizmet.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Hizmet hizmet, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string LogoName = Guid.NewGuid().ToString() + imginfo.Extension;  //Guid.NewGuid().ToString() (FillName)
                    img.Resize(800, 600);
                    img.Save("~/Uploads/Hizmet/" + LogoName);

                    hizmet.ResimURL = "/Uploads/Hizmet/" + LogoName;
                }
                db.Hizmet.Add(hizmet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hizmet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var hizmet = db.Hizmet.Find(id);

            if (hizmet==null)
            {
                return HttpNotFound();
            }

            return View(hizmet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(int? id, Hizmet hizmet, HttpPostedFileBase ResimURL)
        {
            var h = db.Hizmet.Where(x => x.HizmetId == id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL)))//LogoURL önce yüklenmiş bir resim var mı? Var sa sil.
                    {
                        System.IO.File.Delete(Server.MapPath(h.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string HizmetName = Guid.NewGuid().ToString() + imginfo.Extension;  //Guid.NewGuid().ToString() (FillName)
                    img.Resize(800, 600);
                    img.Save("~/Uploads/Hizmet/" + HizmetName);
                    h.ResimURL = "/Uploads/Hizmet/" + HizmetName;
                }
                h.Baslik = hizmet.Baslik;
                h.Aciklama=hizmet.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View();
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // return HttpNotFound();
            }
            Hizmet hizmet = db.Hizmet.Find(id);
            if (hizmet == null)
            {
                return HttpNotFound();
            }
            System.IO.File.Delete(Server.MapPath(hizmet.ResimURL));
            db.Hizmet.Remove(hizmet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hizmet hizmet = db.Hizmet.Find(id);
            if (hizmet == null)
            {
                return HttpNotFound();
            }
            return View(hizmet);
        }

    }
}