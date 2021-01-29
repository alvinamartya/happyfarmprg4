using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class EmployeeProfileModelView
    {
        [Required(ErrorMessage = "Id harus diisi")]
        [DisplayName("Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nama harus diisi")]
        [DisplayName("Nama")]
        public string Name { get; set; }
        [Required(ErrorMessage = "No telepon harus diisi")]
        [DisplayName("No Telepon")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email harus diisi")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Alamat harus diisi")]
        [DisplayName("Alamat")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Jenis Kelamin harus dipilih")]
        [DisplayName("Jenis Kelamin")]
        public string Gender { get; set; }
    }
}