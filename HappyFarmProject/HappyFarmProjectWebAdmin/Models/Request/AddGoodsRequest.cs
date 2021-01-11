using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class AddGoodsRequest
    {
        [Required(ErrorMessage = "Kategori harus diisi")]
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Nama harus diisi")]
        [DisplayName("Nama")]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [Required(ErrorMessage = "Deskripsi harus diisi")]
        [DisplayName("Deskripsi")]
        public string Description { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }
}