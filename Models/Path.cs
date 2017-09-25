using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcoin_Notify.Models
{
    public class Path
    {
        public string label { get; set; }
        public int pathkey { get; set; }
        public List<int> path { get; set; }
        public MarketDifference marketdifference { get; set; }
        public DateTime lastupdate { get; set; }

        public List<int> pathnodes { get; set; }
        public decimal volume { get; set; }
    }
}