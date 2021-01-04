using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HappyFarmProjectWebAdmin.Models
{
    public class AddEmployeeRequest
    {
        [DisplayName("Nama Pengguna")]
        public string Username { get; set; }
        [DisplayName("Hak Akses")]
        public int RoleId { get; set; }
        [DisplayName("Wilayah")]
        public int RegionId { get; set; }
        [DisplayName("Nama")]
        public string Name { get; set; }
        [DisplayName("Nomor Telepon")]
        public string PhoneNumber { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Alamat")]
        public string Address { get; set; }
        [DisplayName("Jenis Kelamin")]
        public string Gender { get; set; }
        public int CreatedBy { get; set; }
    }
}