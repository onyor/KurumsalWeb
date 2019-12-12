using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KurumsalWeb.Models;
using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;

namespace KurumsalWeb.Controllers
{
    public class AdminController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();

        [Route("yönetimpaneli")]
        public ActionResult Index()
        {
            ViewBag.BlogSay = db.Blog.Count();
            ViewBag.KategoriSay = db.Blog.Count();
            ViewBag.HizmetSay = db.Blog.Count();
            ViewBag.YorumSay = db.Blog.Count();
            ViewBag.YorumOnay = db.Yorum.Where(x => x.Onay == false).Count();
            var sorgu = db.Kategori.ToList();
            return View(sorgu);
        }

        [Route("yönetimpaneli/giris")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            try
            {
                var login = db.Admin.Where(x => x.Eposta == admin.Eposta).FirstOrDefault();
                if (login.Eposta == admin.Eposta && login.Sifre == Crypto.Hash(admin.Sifre, "MD5"))
                {
                    Session["adminid"] = login.AdminId;
                    Session["eposta"] = login.Eposta;
                    Session["yetki"] = login.Yetki;
                    return RedirectToAction("Index", "Admin");
                }
                return View(admin);
            }
            catch (Exception)
            {
                ViewBag.Uyari = "Kullanıcı Adı yada Şifre yanlış!";
                return View(admin);
                throw;
            }
        }

        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Adminler()
        {
            return View(db.Admin.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin, string eposta, string sifre)
        {
            if (ModelState.IsValid)
            {
                admin.Sifre = Crypto.Hash(sifre, "MD5");
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }

        public ActionResult Edit(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            return View(a);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string eposta, string sifre, Admin admin)
        {
            if (ModelState.IsValid)
            {
                var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
                a.Sifre = Crypto.Hash(sifre, "MD5");
                a.Eposta = eposta;
                a.Yetki = admin.Yetki;
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }

        public  ActionResult Delete(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            if (a!=null)
            {
                db.Admin.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View();
        }

        public ActionResult RememberMe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RememberMe(string eposta)
        {
            var mail = db.Admin.Where(x => x.Eposta == eposta).SingleOrDefault();
            if (mail != null)
            {
                Random rnd = new Random();
                int yeniSifre = rnd.Next();
                mail.Sifre = Crypto.Hash(Convert.ToString(yeniSifre), "MD5");
                db.SaveChanges();
                WebMail.SmtpServer = "smtp.gmail.com";  // Mail Göndereceğimiz sınıf "Web Mail" sınıfı  // Gmail için "smtp.gmail.com"
                WebMail.EnableSsl = true; // Güvenli bağlantı oluşturulsun.
                WebMail.UserName = "onuryldz008@gmail.com";
                WebMail.Password = "740740740";
                WebMail.SmtpPort = 587;  // İnternetten araştırabilir siniz!
                WebMail.Send(eposta,"Admin Panel Giriş Şifreniz", "Şifreniz: " + yeniSifre);
                ViewBag.Uyari = "Şifreniz başarılı bir şekilde gönderilmiştir.";
            }
            else
            {
                ViewBag.Uyari = "Hata oluştu! Tekrar deneyiniz.";
            }
            return View();
        }
    }
}