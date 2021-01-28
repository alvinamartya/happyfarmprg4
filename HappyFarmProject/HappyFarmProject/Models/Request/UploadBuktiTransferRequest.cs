using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class UploadBuktiTransferRequest
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int CreatedBy { get; set; }
    }
}