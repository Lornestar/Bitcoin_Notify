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
            

            DataTable dtmarketdata = Bitcoin_Notify_DB.SPs.ViewMarketDataAll().GetDataSet().Tables[0];
            DataColumn[] keyColumns1 = new DataColumn[1];
            keyColumns1[0] = dtmarketdata.Columns["market_key"];
            dtmarketdata.PrimaryKey = keyColumns1;

            DataTable dtmarkets = Bitcoin_Notify_DB.SPs.ViewMarketsAll().GetDataSet().Tables[0];
            DataColumn[] keyColumns2 = new DataColumn[1];
            keyColumns2[0] = dtmarkets.Columns["market_key"];
            dtmarkets.PrimaryKey = keyColumns2;

            DataTable dtnodes = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesAll().GetDataSet().Tables[0];
            DataColumn[] keyColumns3 = new DataColumn[1];
            keyColumns3[0] = dtnodes.Columns["exchange_currency_key"];
            dtnodes.PrimaryKey = keyColumns3;
            
            //custom path from db
            df.Update_Paths(dtmarketdata, dtmarkets, dtnodes);

            //automated paths
            df.Update_Paths_All(dtmarketdata, dtmarkets, dtnodes);

            return "";
        }
        

        [OperationContract]
        [WebGet(UriTemplate = "execute_update", ResponseFormat = WebMessageFormat.Json)]
        string Execute_Update()
        {

            mk.UpdateExchange(1);
            mk.UpdateExchange(2);
            mk.UpdateExchange(3);
            mk.UpdateExchange(4);
            mk.UpdateExchange(5);
            mk.UpdateExchange(6);
            mk.UpdateExchange(7);
            mk.UpdateExchange(8);
            
            mk.UpdateCurrencyCloudRatesAll();

            return "";
        }

        [OperationContract]
        [WebGet(UriTemplate = "execute_update_markets", ResponseFormat = WebMessageFormat.Json)]
        string Execute_Update_Markets()
        {
            //delete all markets
            Bitcoin_Notify_DB.SPs.DeleteMarketsAll().Execute();

            int nextmarketkey = 1;

            //create markets within each exchange
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewExchangesAll().GetDataSet().Tables[0];

            foreach (DataRow dr in dttemp.Rows)
            {
                //each exchange
                Models.Exchange currentexchange = new Models.Exchange();
                currentexchange.exchange_key = Convert.ToInt32(dr["exchange_key"]);
                currentexchange.fee_percentage = Convert.ToDecimal(dr["fee_percentage"]);
                currentexchange.withdrawl_fee = Convert.ToDecimal(dr["withdrawl_fee"]);

                DataTable dttemp2 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(currentexchange.exchange_key).GetDataSet().Tables[0];

                foreach (DataRow drsource in dttemp2.Rows)
                {
                    //go through each currency in each exchange
                    int currentsourcekey = Convert.ToInt32(drsource["exchange_currency_key"]);
                    int currentsourcecurrency = Convert.ToInt32(drsource["currency"]);
                    bool isfiat = Convert.ToBoolean(drsource["isfiat"]);

                    //all the other markets in that exchange
                    foreach (DataRow drdestination in dttemp2.Rows)
                    {
                        int currentdestinationkey = Convert.ToInt32(drdestination["exchange_currency_key"]);
                        int currentdestinationcurrency = Convert.ToInt32(drdestination["currency"]);
                        bool isfiat2 = Convert.ToBoolean(drdestination["isfiat"]);

                        if (currentsourcekey != currentdestinationkey)
                        {
                            if ((!isfiat) || (!isfiat2))
                            {
                                //There is a market in that exchange, add it
                                Bitcoin_Notify_DB.SPs.UpdateMarket(nextmarketkey, currentsourcekey, currentdestinationkey, currentexchange.fee_percentage, 0, 10, true).Execute();
                                nextmarketkey += 1;
                            }
                            
                        }
                    }

                    //to all other markets of the same currency
                    DataTable dttemp3 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesAll().GetDataSet().Tables[0];
                    foreach(DataRow drdestination in dttemp3.Rows)
                    {
                        int currentdestinationkey = Convert.ToInt32(drdestination["exchange_currency_key"]);
                        int currentdestinationcurrency = Convert.ToInt32(drdestination["currency"]);

                        if ((currentsourcekey != currentdestinationkey) && (currentsourcecurrency == currentdestinationcurrency))
                        {
                            //There is a market in that exchange, add it
                            Bitcoin_Notify_DB.SPs.UpdateMarket(nextmarketkey, currentsourcekey, currentdestinationkey, 0, currentexchange.withdrawl_fee, 10,false).Execute();
                            nextmarketkey += 1;
                        }
                    }

                    //to all other markets of a different fiat currency - using currencycloud
                    if (isfiat)
                    {
                        //start connecting fiat to all other exchanges fiats
                        foreach (DataRow drcurrentsourceexchangecurrency2 in dttemp3.Rows)
                        {
                            int currentdestinationkey = Convert.ToInt32(drcurrentsourceexchangecurrency2["exchange_currency_key"]);
                            int currentdestinationcurrency = Convert.ToInt32(drcurrentsourceexchangecurrency2["currency"]);
                            bool isfiat2 = Convert.ToBoolean(drcurrentsourceexchangecurrency2["isfiat"]);
                            int exchange2 = Convert.ToInt32(drcurrentsourceexchangecurrency2["exchange_key"]);

                            if ((currentexchange.exchange_key != exchange2) && (isfiat2))
                            {
                                //different exchanges & destination exchange is fiat also, then add
                                //There is a market in that exchange, add it
                                Bitcoin_Notify_DB.SPs.UpdateMarket(nextmarketkey, currentsourcekey, currentdestinationkey, 0, currentexchange.withdrawl_fee, 10, true).Execute();

                                Bitcoin_Notify_DB.SPs.UpdateMarketData(1, 0, nextmarketkey).Execute();
                                nextmarketkey += 1;
                            }
                        }
                    }

                }
                
            }            

            //create markets foreach currencycloud currencies
            dttemp = Bitcoin_Notify_DB.SPs.ViewCurrenciesAllFiat().GetDataSet().Tables[0];

            nextmarketkey = 1;
            foreach (DataRow dr in dttemp.Rows)
            {
                foreach (DataRow dr2 in dttemp.Rows)
                {
                    int sourcecurrency = Convert.ToInt32(dr["currency_key"]);
                    int destinationcurrency = Convert.ToInt32(dr2["currency_key"]);
                    if (sourcecurrency != destinationcurrency)
                    {
                        Bitcoin_Notify_DB.SPs.UpdateCurrencyCloudMarket(nextmarketkey, sourcecurrency, destinationcurrency, 0, 0, 1440).Execute();
                        nextmarketkey += 1;
                    }
                    
                }                
            }


            APIs.Firebase fb = new APIs.Firebase();
            fb.ClearAllData();


            return "";
        }

        [OperationContract]
        [WebGet(UriTemplate = "execute_update_recurring", ResponseFormat = WebMessageFormat.Json)]
        string Execute_Update_Recurring()
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewUpdateRecurring().GetDataSet().Tables[0];
            bool keepupdating = false; 
            
            try
            {
                keepupdating = Convert.ToBoolean( dttemp.Rows[0]["continue"]);
            }
            catch{

            }

            do
            {
                Execute_Update();
                Execute_Update_Differences();

                dttemp = Bitcoin_Notify_DB.SPs.ViewUpdateRecurring().GetDataSet().Tables[0];
                keepupdating = Convert.ToBoolean(dttemp.Rows[0]["continue"]);

            } while (keepupdating);

            return "";
        }

    }
}
