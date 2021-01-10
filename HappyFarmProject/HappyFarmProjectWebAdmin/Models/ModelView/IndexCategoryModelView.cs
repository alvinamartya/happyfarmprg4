using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class IndexCategoryModelView
    {
        public IEnumerable<CategoryModelView> CategoryModelViews { get; set; }
        public GetListDataRequest DataPaging { get; set; }
    }
}