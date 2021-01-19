using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HappyFarmProjectWebAdmin.Models
{
    public class AddGoodsPriceRegionRequest
    {
        public int GoodsId { get; set; }
        [DisplayName("Wilayah")]
        [Required(ErrorMessage = "Wilayah harus diisi")]
        public int RegionId { get; set; }
        public int CreatedBy { get; set; }
        [DisplayName("Harga")]
        [Required(ErrorMessage = "Harga harus diisi")]
        public long Price { get; set; }
    }
}