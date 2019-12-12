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
    public class BlogController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Blog
        public ActionResult Index()
        {
            db.Configuration.LazyLoadingEnabled = false; //Sorgumu yazarken, bu sorgunun içerisine KatogoriId eklenmesini sağlıyorum.
            //var b = db.Blog.ToList();         2. YOL
            return View(db.Blog.Include("Kategori").ToList().OrderByDescending(x=>x.BlogId));
        }

        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog, HttpPostedFileBase ResimURL)
        {
            try
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string BlogImgName = Guid.NewGuid().ToString() + imginfo.Extension;  //Guid.NewGuid().ToString() (FillName)
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + BlogImgName);

                    blog.ResimURL = "/Uploads/Blog/" + BlogImgName;
                }
                db.Blog.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return HttpNotFound();
            }

            var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();
            if (b == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd", b.KategoriId);
            return View(b);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int? id, Blog blog, HttpPostedFileBase ResimURL)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();
                    if (ResimURL != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath(b.ResimURL)))
                        {
                            System.IO.File.Delete(Server.MapPath(b.ResimURL));
                        }
                        WebImage img = new WebImage(ResimURL.InputStream);
                        FileInfo imgInfo = new FileInfo(ResimURL.FileName);

                        string BlogImgName = Guid.NewGuid().ToString() + imgInfo.Extension;
                        img.Resize(600, 400);
                        img.Save("~/Uploads/Blog/" + BlogImgName);
                        b.ResimURL = "/Uploads/Blog/" + BlogImgName; 
                    }
                    b.Baslik = blog.Baslik;
                    b.Icerik = blog.Icerik;
                    b.KategoriId = blog.KategoriId;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(blog);
            }
            catch (Exception)
            {
                return View();
            }
            
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Blog blog = db.Blog.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blog.Find(id);
            if (System.IO.File.Exists(Server.MapPath(blog.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(blog.ResimURL));
            }
            db.Blog.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}