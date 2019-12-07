using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareeriaDevsApp.Models
{
    public class OpiskelijaInfinite
    {
        public List<OmaSisalto> Opiskelijat { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }

    }
}