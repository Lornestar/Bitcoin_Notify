using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using System.Data;

namespace Bitcoin_Notify
{    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract]
    public class Service1
    {
        APIs.Markets mk = new APIs.Markets();
        
        [OperationContract]
        [WebGet(UriTemplate = "execute_update_differences", ResponseFormat = WebMessageFormat.Json)]
        string Execute_Update_Differences()
        {
            Differences df = new Differences();
            //df.CaVirtexBTC_CoinbaseBTC();
            //df.CaVirtexBTC_KrakenBTC_KrakenLTC_CaVirtexBTC();

            APIs.Firebase fb = new APIs.Firebase();
            fb.ClearAllData();
            
            df.Update_Paths();

            df.Update_Paths_All();

            return "";
        }
        

        [OperationContract]
        [WebGet(UriTemplate = "execute_update", ResponseFormat = WebMessageFormat.Json)]
        string Execute_Update()
        {
            
            //update cavirtex
            Thread t = new Thread(mk.UpdateCaVirtex);
            t.Start();

            //update coinbase
            Thread t2 = new Thread(mk.UpdateCoinbase0);
            t2.Start();

            Thread t2b = new Thread(mk.UpdateCoinbase1);
            t2b.Start();

            //update kraken
            Thread t3 = new Thread(mk.UpdateKraken);
            t3.Start();

            //update btce
            Thread t4 = new Thread(mk.UpdateBtce);
            t4.Start();

            Thread t8 = new Thread(mk.UpdateCryptsy);
            t8.Start();

            //update coinmkt
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketDataSpecific(28).GetDataSet().Tables[0];
            DateTime lastupdate28 = Convert.ToDateTime(dttemp.Rows[0]["datechanged"]);
            dttemp = Bitcoin_Notify_DB.SPs.ViewMarketDataSpecific(30).GetDataSet().Tables[0];
            DateTime lastupdate30 = Convert.ToDateTime(dttemp.Rows[0]["datechanged"]);
            dttemp = Bitcoin_Notify_DB.SPs.ViewMarketDataSpecific(32).GetDataSet().Tables[0];
            DateTime lastupdate32 = Convert.ToDateTime(dttemp.Rows[0]["datechanged"]);
            if ((lastupdate28.AddSeconds(16) < DateTime.Now) && (lastupdate30.AddSeconds(16) < DateTime.Now) && (lastupdate32.AddSeconds(16) < DateTime.Now))
            {
                if ((DateTime.Now.Second >= 0) && (DateTime.Now.Second < 20))
                {
                    Thread t5 = new Thread(mk.UpdateCoinmkt);
                    t5.Start();
                }
                else if ((DateTime.Now.Second >= 20) && (DateTime.Now.Second < 40))
                {
                    Thread t6 = new Thread(mk.UpdateCoinmkt2);
                    t6.Start();
                }
                else
                {
                    Thread t7 = new Thread(mk.UpdateCoinmkt3);
                    t7.Start();
                }  
            }            

            return "";
        }
    }
}
