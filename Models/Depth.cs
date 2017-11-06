using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcoin_Notify.Models
{
    public class Depth
    {
        public int orderbookdepth { get; set; }
        public decimal cumulativedepthusd { get; set; }
        public decimal currentdepthamountusd { get; set; }
        public int marketkey { get; set; }
        public decimal cumulativeprofitUSD { get; set; }
    }
}