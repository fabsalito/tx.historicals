using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

namespace sga.Filters
{
    public class callParameterAlarmsGrid
    {
        public Int32 idLinea { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string system { get; set; }
        public string node { get; set; }
        public DateTime inicio { get; set; }
        public DateTime fin { get; set; }
        public string code { get; set; }
        public bool showEvents { get; set; }
        public bool showAusNoRec { get; set; }
        public bool showPreNoRec { get; set; }
        public bool showPreRec { get; set; }
        public bool outside { get; set; }
        public int iDisplayLength { get; set; }
    }
}