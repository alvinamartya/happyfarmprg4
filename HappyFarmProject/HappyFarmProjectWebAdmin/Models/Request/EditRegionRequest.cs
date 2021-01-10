using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HappyFarmProjectWebAdmin.Models
{
    public class EditRegionRequest
    {
        [Required(ErrorMessage = "Nama Wilayah harus diisi")]
        [DisplayName("Nama Wilayah")]
        public string Name { get; set; }
        public int ModifiedBy { get; set; }
    }
}