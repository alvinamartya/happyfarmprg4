using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class ProcessGoodsModelView
    {
        [Required(ErrorMessage = "Kategori harus diisi")]
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Nama harus diisi")]
        [DisplayName("Nama")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Deskripsi harus diisi")]
        [DisplayName("Deskripsi")]
        public string Description { get; set; }

        [DisplayName("Gambar")]
        public HttpPostedFileBase Image { get; set; }

        public string HiddenFileName { get; set; }
        public string OriginalFileName { get; set; }
    }
}