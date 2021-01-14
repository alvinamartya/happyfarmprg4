using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class ProcessBannerModelView
    {
        [Required(ErrorMessage = "Nama harus diisi")]
        [DisplayName("Nama Banner")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Promo harus diisi")]
        [DisplayName("Promo")]
        public int? PromoId { get; set; }
        [DisplayName("Gambar")]
        public HttpPostedFileBase Image { get; set; }

        public string HiddenFileName { get; set; }
        public string OriginalFileName { get; set; }
    }
}