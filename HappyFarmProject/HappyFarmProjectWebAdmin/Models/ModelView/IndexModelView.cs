using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class IndexModelView<T>
    {
        public T ModelViews { get; set; }
        public GetListDataRequest DataPaging { get; set; }
    }
}