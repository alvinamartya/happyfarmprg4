using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class ProcessPromoModelView
    {
        [Required(ErrorMessage = "Nama harus diisi")]
        [DisplayName("Nama")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tanggal Mulai harus diisi")]
        [DisplayName("Tanggal Mulai")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Tanggal Berakhir harus diisi")]
        [DisplayName("Tanggal Berakhir")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Gratis Ongkos Kirim harus diisi")]
        [DisplayName("Gratis Ongkos Kirim")]
        public string IsFreeDelivery { get; set; }
        [Required(ErrorMessage = "Diskon harus diisi")]
        [DisplayName("Diskon")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float Discount { get; set; }
        [Required(ErrorMessage = "Minimal Transaksi harus diisi")]
        [DisplayName("Minimal Transaksi")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public long MinTransaction { get; set; }
        [Required(ErrorMessage = "Maksimal Diskon harus diisi")]
        [DisplayName("Maksimal Diskon")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public long MaxDiscount { get; set; }
        [DisplayName("Gambar")]
        public HttpPostedFileBase Image { get; set; }
        public string HiddenFileName { get; set; }
        public string OriginalFileName { get; set; }
    }
}