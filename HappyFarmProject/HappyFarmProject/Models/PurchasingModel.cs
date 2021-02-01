using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class PurchasingModel
    {
        [Required(ErrorMessage = "Nama Penerima harus diisi")]
        public string RecipientName { get; set; }
        [Required(ErrorMessage = "No telepon Penerima harus diisi")]
        public string RecipientPhone { get; set; }
        [Required(ErrorMessage = "Alamat harus diisi")]
        public string RecipientAddress { get; set; }
        public string PromoCode { get; set; }
        public int RegionId { get; set; }
        public int SubdistrictId { get; set; }
        public int TotalPurchase { get; set; }
        public int Discount { get; set; }
        public int ShippingCharges { get; set; }
    }
}