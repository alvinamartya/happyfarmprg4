using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class UploadBuktiTransfer
    {
        public string Id { get; set; }
        [DisplayName("Gambar")]
        public HttpPostedFileBase Image { get; set; }
        public string HiddenFileName { get; set; }
        public string OriginalFileName { get; set; }
    }
}