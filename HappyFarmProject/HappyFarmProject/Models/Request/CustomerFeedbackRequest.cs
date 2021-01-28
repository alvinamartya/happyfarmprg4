using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class CustomerFeedbackRequest
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
    }
}