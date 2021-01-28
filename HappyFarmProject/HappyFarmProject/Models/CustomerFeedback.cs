using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class CustomerFeedback
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Rating harus diisi")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Keterangan harus diisi")]
        [DisplayName("Keterangan")]
        public string Note { get; set; }
    }
}