using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class PurchasingDetailRequest
    {
        [Required(ErrorMessage = "Kategori harus dipilih")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Produk harus dipilih")]
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        [Required(ErrorMessage = "Jumlah harus diisi")]
        public int Qty { get; set; }
        [Required(ErrorMessage = "Harga harus diisi")]
        public long Price { get; set; }
    }
}