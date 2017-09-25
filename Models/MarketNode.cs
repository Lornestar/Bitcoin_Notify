using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcoin_Notify.Models
{
    public class MarketNode
    {
        public int exchange_currency_key { get; set; }
        public int exchange_key { get; set; }
        public int currency { get; set; }
        public Boolean isfiat { get; set; }
    }
}