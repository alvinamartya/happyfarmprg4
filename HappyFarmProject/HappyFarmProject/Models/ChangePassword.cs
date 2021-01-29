using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Kata sandi lama harus diisi")]
        [DisplayName("Kata sandi lama")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Password baru harus diisi")]
        [DisplayName("Kata sandi baru")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Konfirmasi kata sandi harus diisi")]
        [DisplayName("Kata sandi baru")]
        public string ConfirmPassword { get; set; }
    }
}