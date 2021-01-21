using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class CustomerFeedbackModelView
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double Rating { get; set; }
        public string Note { get; set; }
    }
}