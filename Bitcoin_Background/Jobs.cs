using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Hangfire;
using System.Configuration;
using System.Text;

namespace Bitcoin_Background
{
    public class Jobs
    {
        string baseurl = ConfigurationSettings.AppSettings["API_base_url"];
        Site sitetemp = new Site();

        [AutomaticRetry(Attempts = 0)]
        public void Init_Jobs()
        {
            RecurringJob.AddOrUpdate(() => doarb(), "*/1 * * * *");

        }

        public void doarb()
        {
            sitetemp.Web_Request(baseurl + "/execute_update", null, 0, null, "");
            sitetemp.Web_Request(baseurl + "/execute_update_differences", null, 0, null, "");
            //sitetemp.Web_Request("http://35.197.51.4/bitcoin_notify/service1.svc/execute_update", null, 0, null, "");
            //sitetemp.Web_Request("http://35.197.51.4/bitcoin_notify/service1.svc/execute_update_differences", null, 0, null, "");
        }
    }
}