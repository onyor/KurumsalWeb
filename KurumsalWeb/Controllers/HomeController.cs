using KurumsalWeb.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBContext db=new KurumsalDBContext();

        // GET: Home
        [Route("")]
        [Route("Anasayfa")]
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmet = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
            
            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x=>x.SliderId));
        }
        
        public  ActionResult HizmetPartial()
        {
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }

        [Route("Hakkimizda")]
        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.ToList().SingleOrDefault());
        }

        [Route("Hizmetlerimiz")]
        public ActionResult Hizmetlerimiz()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(x=>x.HizmetId));
        }

        public ActionResult FooterPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            ViewBag.Hizmet = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);

            ViewBag.Iletisim = db.Iletisim.Where(x => x.IletisimId == 1).SingleOrDefault();

            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);

            return PartialView();
        }

        public ActionResult IletisimPartial()
        {
            return View(db.Iletisim.ToString().SingleOrDefault());
        }

        public ActionResult BlogPartial()
        {

            return View(db.Blog.ToString().FirstOrDefault());
        }

        [Route("Iletisim")]
        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Iletisim(string AdSoyad=null,string Email=null,string Konu=null, string Mesaj=null)
        {
            if (AdSoyad!=null && Email!=null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";  // Mail Göndereceğimiz sınıf "Web Mail" sınıfı  // Gmail için "smtp.gmail.com"
                WebMail.EnableSsl = true; // Güvenli bağlantı oluşturulsun.
                WebMail.UserName = "onuryldz008@gmail.com";
                WebMail.Password = "740740740";
                WebMail.SmtpPort = 587;  // İnternetten araştırabilir siniz!
                WebMail.Send("onuryldz008@gmail.com", Konu, "</br>"  + Mesaj);
                ViewBag.Uyari = "Mesajınız başarı ile gönderilmiştir.";
            }
            else
            {
                ViewBag.Uyari = "Hata oluştu! Tekrar deneyiniz.";
            }
            return View();
        }
        [Route("BlogPost")]
        public ActionResult Blog(int Page=1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Kategori").OrderByDescending(x => x.BlogId).ToPagedList(Page,3));
        }

        public ActionResult BlogKategoriPartial()
        {
            return PartialView(db.Kategori.Include("Blogs").ToList().OrderBy(x => x.KategoriAd));
        }
        [Route("BlogPost/{kategoriad}/{id:int}")]
        public ActionResult KategoriBlog(int id, int Sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var b = db.Blog.Include("Kategori").Where(x => x.Kategori.KategoriId == id).OrderByDescending(x => x.BlogId).ToPagedList(Sayfa,3);
            return View(b);
        }
       
        public ActionResult BlogSonPartial()
        {
            return PartialView(db.Blog.ToList().OrderByDescending(x=>x.BlogId));
        }

        [Route("BlogPost/{baslik}-{id:int}")]
        public ActionResult BlogDetay(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var bd = db.Blog.Include("Kategori").Include("Yorums").Where(x => x.BlogId == id).SingleOrDefault();
            return View(bd);
        }

        public JsonResult YorumYap(string adsoyad, string eposta, string icerik, int blogId)
        {
            if (icerik==null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            db.Yorum.Add(new Models.Model.Yorum { AdSoyad = adsoyad, Eposta = eposta, Icerik = icerik, BlogId = blogId, Onay=false });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}