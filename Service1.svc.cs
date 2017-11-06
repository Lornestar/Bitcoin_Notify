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
using System.Collections;
using SubSonic;

namespace Bitcoin_Notify
{    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract]
    public class Service1
    {
        APIs.Markets mk = new APIs.Markets();
        Site sitetemp = new Site();
        public delegate void AsyncMethodCaller(int exchangekey, out int threadId);
        int numberofexchanges = 9;
        Hashtable hsnumberofthreads = new Hashtable();

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

            DataTable dtmarketorderbooks = Bitcoin_Notify_DB.SPs.ViewMarketOrderbooksAll().GetDataSet().Tables[0];
            DataColumn[] keyColumns4 = new DataColumn[1];
            keyColumns4[0] = dtnodes.Columns["market_key"];
            dtmarketorderbooks.PrimaryKey = keyColumns4;

            Hashtable hsnodes = new Hashtable();
            foreach (DataRow dr in dtnodes.Rows)
            {
                hsnodes.Add(Convert.ToInt32(dr["exchange_currency_key"]), sitetemp.MappingMarketNode(dr));
            }

            //setup list of nodes by exchange
            List<Hashtable> lshsnodesbyexchange = new List<Hashtable>();

            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewExchangesAll().GetDataSet().Tables[0];

            foreach (DataRow dr in dttemp.Rows)
            {
                //each exchange
                Hashtable hsnodesbyexchange = new Hashtable();
                lshsnodesbyexchange.Add(hsnodesbyexchange);
            }
            
            foreach (DictionaryEntry entry in hsnodes)
            {
                Models.MarketNode node = (Models.MarketNode)entry.Value;
                Hashtable hsnodesbyexchange = lshsnodesbyexchange[node.exchange_key - 1];
                hsnodesbyexchange.Add(node.exchange_currency_key, node);
                lshsnodesbyexchange[node.exchange_key - 1] = hsnodesbyexchange;
            }
            
            //custom path from db
            //df.Update_Paths(dtmarketdata, dtmarkets, dtnodes);

            //automated paths
            df.Update_Paths_All(dtmarketdata, dtmarkets, hsnodes, dtnodes, lshsnodesbyexchange, dtmarketorderbooks);
            hsnumberofthreads.Clear();

            return "";
        }
        

        [OperationContract]
        [WebGet(UriTemplate = "execute_update", ResponseFormat = WebMessageFormat.Json)]
        string Execute_Update()
        {
            
            for (int i = 1; i <= numberofexchanges; i++ )
            {
                //mk.UpdateExchange(i); 
                int threadId;
                AsyncMethodCaller caller = new AsyncMethodCaller(CallMarkets);
                
                // Initiate the asychronous call.
                IAsyncResult result = caller.BeginInvoke(i, out threadId, Callback, null);
                
                hsnumberofthreads.Add(i, result);
            }

            /*
            while (hsnumberofthreads.Count > 0)
            {

            }*/
            
            WaitHandle[] waitHandles = new WaitHandle[numberofexchanges];
            foreach (DictionaryEntry entry in hsnumberofthreads)
            {
                IAsyncResult tempresult = (IAsyncResult)entry.Value;
                waitHandles[Convert.ToInt32( entry.Key)-1] = tempresult.AsyncWaitHandle;
            }
            WaitHandle.WaitAll(waitHandles);

            hsnumberofthreads.Clear();

            return "";
        }

        public void CallMarkets(int exchangekey, out int threadId)
        {
            
                mk.UpdateExchange(exchangekey);
            
            threadId = Thread.CurrentThread.ManagedThreadId;
            //hsnumberofthreads.Add(threadId, null);
        }

        public void Callback(IAsyncResult result)
        {
            hsnumberofthreads.Remove(hsnumberofthreads[hsnumberofthreads.Count]);
            //hsnumberofthreads.Remove(Thread.CurrentThread.ManagedThreadId);
            //Console.WriteLine("..Climbing is completed...");
        }

        [OperationContract]
        [WebGet(UriTemplate = "execute_update_round", ResponseFormat = WebMessageFormat.Json)]
        void Execute_Update_Round()
        {
            Execute_Update();
            Execute_Update_Differences();

            Notifications nt = new Notifications();
            nt.CheckNotifications();
        }

        [OperationContract]
        [WebGet(UriTemplate = "execute_notification", ResponseFormat = WebMessageFormat.Json)]
        void Execute_Notification()
        {

            Notifications nt = new Notifications();
            nt.CheckNotifications();
        }

        [OperationContract]
        [WebGet(UriTemplate = "execute_update_fiat_rates", ResponseFormat = WebMessageFormat.Json)]
        void Execute_Update_Fiat_Rates()
        {
            mk.UpdateCurrencyCloudRatesAll();
        }

        [OperationContract]
        [WebGet(UriTemplate = "execute_update_firebase", ResponseFormat = WebMessageFormat.Json)]
        void Execute_Update_Firebase()
        {
            APIs.Firebase fb = new APIs.Firebase();

            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewArbResults().GetDataSet().Tables[0];
            List<Models.Path> allpaths = new List<Models.Path>();
            foreach(DataRow dr in dttemp.Rows)
            {
                Models.Path currentpath = new Models.Path();
                currentpath.label = dr["label"].ToString();
                currentpath.marketdifference = new Models.MarketDifference();
                currentpath.marketdifference.percentage = Convert.ToDecimal(dr["percentage"]);
                currentpath.pathkey = Convert.ToInt32(dr["arb_results_key"]);
                allpaths.Add(currentpath);
            }
            fb.UpdateAllPaths(allpaths);
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
                currentexchange.isstatic = Convert.ToBoolean(dr["isstatic"]);

                
                APIs.Market_Updates mu = new APIs.Market_Updates();

                if (currentexchange.isstatic)
                {
                    //Setup static markets
                    mu.Update_Markets_Static(currentexchange);
                }
                else
                {
                    //Setup automatic exchange
                    mu.Update_Markets_Automated(currentexchange.exchange_key);
                }

                

                DataTable dttemp2 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(currentexchange.exchange_key).GetDataSet().Tables[0];

                foreach (DataRow drsource in dttemp2.Rows)
                {
                    //go through each currency in each exchange
                    int currentsourcekey = Convert.ToInt32(drsource["exchange_currency_key"]);
                    int currentsourcecurrency = Convert.ToInt32(drsource["currency"]);
                    bool isfiat = Convert.ToBoolean(drsource["isfiat"]);

                    /*//all the other markets in that exchange
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
                                Bitcoin_Notify_DB.SPs.UpdateMarket(nextmarketkey, currentsourcekey, currentdestinationkey, currentexchange.fee_percentage, 0, 10, true,"").Execute();
                                nextmarketkey += 1;
                            }
                            
                        }
                    }*/

                }

                
            }

            //Now matching all same currencies with eachother
            foreach (DataRow dr in dttemp.Rows)
            {
                //each exchange
                Models.Exchange currentexchange = new Models.Exchange();
                currentexchange.exchange_key = Convert.ToInt32(dr["exchange_key"]);
                currentexchange.fee_percentage = Convert.ToDecimal(dr["fee_percentage"]);
                currentexchange.withdrawl_fee = Convert.ToDecimal(dr["withdrawl_fee"]);
                currentexchange.isstatic = Convert.ToBoolean(dr["isstatic"]);

                DataTable dttemp2 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(currentexchange.exchange_key).GetDataSet().Tables[0];
                foreach (DataRow drsource in dttemp2.Rows)
                {
                    //go through each currency in each exchange
                    int currentsourcekey = Convert.ToInt32(drsource["exchange_currency_key"]);
                    int currentsourcecurrency = Convert.ToInt32(drsource["currency"]);
                    bool isfiat = Convert.ToBoolean(drsource["isfiat"]);


                    //to all other markets of the same currency
                    DataTable dttemp3 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesAll().GetDataSet().Tables[0];
                    foreach (DataRow drdestination in dttemp3.Rows)
                    {
                        int currentdestinationkey = Convert.ToInt32(drdestination["exchange_currency_key"]);
                        int currentdestinationcurrency = Convert.ToInt32(drdestination["currency"]);

                        if ((currentsourcekey != currentdestinationkey) && (currentsourcecurrency == currentdestinationcurrency))
                        {
                            //There is a market in that exchange, add it
                            Bitcoin_Notify_DB.SPs.UpdateMarket(0, currentsourcekey, currentdestinationkey, 0, currentexchange.withdrawl_fee, 10, false, "", 0).Execute();



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

                            if (isfiat2)
                            {

                                //different exchanges & destination exchange is fiat also, then add
                                //There is a market in that exchange, add it
                                StoredProcedure sp_updatemarket = Bitcoin_Notify_DB.SPs.UpdateMarket(0, currentsourcekey, currentdestinationkey, 0, currentexchange.withdrawl_fee, 10, true, "", 0);
                                sp_updatemarket.Execute();
                                nextmarketkey = Convert.ToInt32(sp_updatemarket.Command.Parameters[8].ParameterValue);

                                

                                Bitcoin_Notify_DB.SPs.UpdateMarketData(1, 0, nextmarketkey).Execute();

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


            //Setup all possible paths
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

            Hashtable hsnodes = new Hashtable();
            foreach (DataRow dr in dtnodes.Rows)
            {
                hsnodes.Add(Convert.ToInt32(dr["exchange_currency_key"]), sitetemp.MappingMarketNode(dr));
            }

            //setup list of nodes by exchange
            List<Hashtable> lshsnodesbyexchange = new List<Hashtable>();

            DataTable dtexchangesall = Bitcoin_Notify_DB.SPs.ViewExchangesAll().GetDataSet().Tables[0];

            foreach (DataRow dr in dtexchangesall.Rows)
            {
                //each exchange
                Hashtable hsnodesbyexchange = new Hashtable();
                lshsnodesbyexchange.Add(hsnodesbyexchange);
            }
            
            foreach (DictionaryEntry entry in hsnodes)
            {
                Models.MarketNode node = (Models.MarketNode)entry.Value;
                Hashtable hsnodesbyexchange = lshsnodesbyexchange[node.exchange_key - 1];
                hsnodesbyexchange.Add(node.exchange_currency_key, node);
                lshsnodesbyexchange[node.exchange_key - 1] = hsnodesbyexchange;
            }

            dtmarkets = Bitcoin_Notify_DB.SPs.ViewMarketsAll().GetDataSet().Tables[0];
            DataColumn[] keyColumns4 = new DataColumn[1];
            keyColumns4[0] = dtmarkets.Columns["market_key"];
            dtmarkets.PrimaryKey = keyColumns4;

            Differences df = new Differences();
            df.Create_Paths_All(dtmarkets, hsnodes, dtnodes, lshsnodesbyexchange);

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
                Execute_Update_Firebase();

                dttemp = Bitcoin_Notify_DB.SPs.ViewUpdateRecurring().GetDataSet().Tables[0];
                keepupdating = Convert.ToBoolean(dttemp.Rows[0]["continue"]);

            } while (keepupdating);

            return "";
        }

    }
}
