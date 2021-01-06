using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class IndexEmployeeModelView
    {
        public IEnumerable<EmployeeModelView> EmployeeModelViews { get; set; }
        public GetListDataRequest DataPaging { get; set; }
    }
}