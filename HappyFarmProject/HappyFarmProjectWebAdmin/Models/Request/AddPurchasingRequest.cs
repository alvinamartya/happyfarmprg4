using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class AddPurchasingRequest
    {
        public int EmployeeId { get; set; }
        [DisplayName("Nama Petani")]
        [Required(ErrorMessage = "Nama petani harus diisi")]
        public string FarmerName { get; set; }
        [DisplayName("No Telepon Petani")]
        [Required(ErrorMessage = "No telepon petani harus diisi")]
        public string FarmerPhone { get; set; }
        [DisplayName("Alamat Petani")]
        [Required(ErrorMessage = "Alamat petani harus diisi")]
        public string FarmerAddress { get; set; }
        public List<PurchasingDetailRequest> PurchasingDetails { get; set; }
    }
}