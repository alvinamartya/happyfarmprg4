using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HappyFarmProjectWebAdmin.Models
{
    public class ProcessBannerModelView
    {
        [Required(ErrorMessage ="Nama harus diisi")]
        [DisplayName("Nama")]
        public string Name { get; set; }
        [DisplayName("Promo")]
        public int? PromoId { get; set; }
        [DisplayName("Gambar")]
        public HttpPostedFileBase Image { get; set; }
        public string HiddenFileName { get; set; }
        public string OriginalFileName { get; set; }
    }
}