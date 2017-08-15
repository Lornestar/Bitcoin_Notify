using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcoin_Notify.Models
{
    public class MarketDifference
    {        
        public decimal percentage { get; set; }
        public decimal staticfeeUSD { get; set; }
        public decimal staticfeeBTC { get; set; }
        public decimal staticfeeLTC { get; set; }
        public int time { get; set; }
        public bool isvoid { get; set; }
    }
}