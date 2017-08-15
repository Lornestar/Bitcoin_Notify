using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcoin_Notify.Models
{
    public class Exchange
    {
        public int exchange_key { get; set; }
        public string exchange_name { get; set; }
        public string exchange_shortname { get; set; }
        public decimal fee_percentage { get; set; }
        public decimal withdrawl_fee { get; set; }
    }
}