using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.DataContext
{
    public class KurumsalDBContext : DbContext   //Database oluşturduk.
    {
        public KurumsalDBContext() : base("KurumsalWebDB")  //ctor tab tab  //Ad == Web.Config
        {

        }
        public DbSet<Admin> Admin { get; set; } //Admin tablomuzu Veri tabanına set ettik.
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Hakkimizda> Hakkimizda { get; set; }
        public DbSet<Hizmet> Hizmet { get; set; }
        public DbSet<Iletisim> Iletisim { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<Kimlik> Kimlik { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Yorum> Yorum { get; set; }
    }
}

/*  Package Manager Console
 *  Code First yöntemiyle Package Manager Console veri tabanını sql tarafında her hangi bir işlem yapmadan kod ile generate edilmesini inceledik.
 *  Test yapıldı.
 *  PM> Enable-Migrattions
 *  PM> Update-Database
*/