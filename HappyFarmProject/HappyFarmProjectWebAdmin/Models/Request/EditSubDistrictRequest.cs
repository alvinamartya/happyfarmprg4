using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HappyFarmProjectWebAdmin.Models
{
    public class EditSubDistrictRequest
    {
        [Required(ErrorMessage = "Nama Wilayah harus diisi")]
        [DisplayName("Nama Wilayah")]
        public int RegionId { get; set; }
        [Required(ErrorMessage = "Nama Kecamatan harus diisi")]
        [DisplayName("Nama Kecamatan")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Biaya kirim harus diisi")]
        [DisplayName("Biaya Kirim")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Hanya Bisa Angka")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal ShippingCharges { get; set; }
        public int ModifiedBy { get; set; }
    }
}