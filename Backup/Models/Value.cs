using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcoin_Notify.Models
{
    public class Value
    {
        public decimal amount { get; set; }
        public int currency { get; set; }
        public bool isvoid { get; set; }
    }
}