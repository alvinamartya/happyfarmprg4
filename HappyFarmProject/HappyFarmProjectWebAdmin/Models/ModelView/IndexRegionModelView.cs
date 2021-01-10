using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class IndexRegionModelView
    {
        public IEnumerable<RegionModelView> RegionModelViews { get; set; }
        public GetListDataRequest DataPaging { get; set; }
    }
}