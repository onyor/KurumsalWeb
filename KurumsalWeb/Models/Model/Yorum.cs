﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Yorum")]
    public class Yorum
    {
        [Key]
        public int YorumId { get; set; }

        [Required, StringLength(50,ErrorMessage ="50 Karakter Olmalıdır.")]
        [DisplayName("Ad Soyad")]

        public string AdSoyad { get; set; }
        [DisplayName("E Posta")]

        public string Eposta { get; set; }
        [DisplayName("Yorumunuz")]

        public string Icerik { get; set; }

        public bool Onay { get; set; }

        public Blog Blog { get; set; }   // Bu yorym hangi blog kaydına ait!

        public int? BlogId { get; set; }
    }
}