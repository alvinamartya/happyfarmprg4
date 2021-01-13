﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class AddCategoryRequest
    {
        [Required(ErrorMessage = "Nama Category harus diisi")]
        [DisplayName("Nama Category")]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
    }
}