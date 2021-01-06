using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HappyFarmProjectWebAdmin.Models
{
    public class EditEmployeeRequest
    {
        [Required(ErrorMessage = "Wilayah harus diisi")]
        [DisplayName("Wilayah")]
        public int RegionId { get; set; }
        [Required(ErrorMessage = "Nama harus diisi")]
        [DisplayName("Nama")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Nomor Telepon harus diisi")]
        [DisplayName("Nomor Telepon")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email harus diisi")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Alamat harus diisi")]
        [DisplayName("Alamat")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Jenis Kelamin harus diisi")]
        [DisplayName("Jenis Kelamin")]
        public string Gender { get; set; }
        public int ModifiedBy { get; set; }
    }
}