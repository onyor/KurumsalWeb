using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class KimlikController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Kimlik
        public ActionResult Index()
        {
            return View(db.Kimlik.ToList());
        }

        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)
        {
            var kimlik = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault(); // Böyle bir bilgi geliyormu geliyorsa nerede?
            return View(kimlik);
        }

        // POST: Kimlik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]  //Request.From Algılandı!!!
        public ActionResult Edit(int id, Kimlik kimlik, HttpPostedFileBase LogoURL)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var k = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();
                    if (LogoURL != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath(k.LogoURL)))//LogoURL önce yüklenmiş bir resim var mı? Var sa sil.
                        {
                            System.IO.File.Delete(Server.MapPath(k.LogoURL));
                        }
                        WebImage img = new WebImage(LogoURL.InputStream);
                        FileInfo imginfo = new FileInfo(LogoURL.FileName);

                        string LogoName = LogoURL.FileName + imginfo.Extension;  //Guid.NewGuid().ToString() (FillName)
                        img.Resize(300, 200);
                        img.Save("~/Uploads/Kimlik/" + LogoName);

                        k.LogoURL = "/Uploads/Kimlik/" + LogoName;
                    }
                    k.Title = kimlik.Title;
                    k.Keywords = kimlik.Keywords;
                    k.Description = kimlik.Description;
                    k.Unvan = kimlik.Unvan;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(kimlik);
            }
            catch
            {
                return View();
            }
        }
    }
}
