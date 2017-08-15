using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcoin_Notify.Models
{
    public class Market
    {
        public int market_key { get; set; }
        public MarketNode source { get; set; }
        public MarketNode destination { get; set; }
        public decimal price { get; set; }
        public decimal fee_percentage { get; set; }
        public decimal fee_static { get; set; }
        public bool automatic { get; set; }
        public bool ratechanges { get; set; }
        public int exchangetime { get; set; } //time takes to transfer the funds

        public decimal volume { get; set; }
    }
}