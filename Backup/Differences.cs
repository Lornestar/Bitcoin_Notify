using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data;
using System.Collections;
using Bitcoin_Notify.Models;

namespace Bitcoin_Notify
{
    public class Differences
    {
        Site sitetemp = new Site();
        APIs.Firebase fb = new APIs.Firebase();

        private MarketDifference GetArbitrageRate(List<int> path)
        {
            MarketDifference arbitragerate = new MarketDifference();
            arbitragerate.isvoid = false;

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

            //set first buy data
            int startingmarket = path[0];                        
            int startingnode = Convert.ToInt32(dtmarkets.Rows.Find(startingmarket)["source"]);

            Value startingamount = new Value();
            startingamount.amount = 1;            
            startingamount.currency = Convert.ToInt32(dtnodes.Rows.Find(startingnode)["currency"]);

            Value currentamount = new Value();
            currentamount = startingamount;
            currentamount.isvoid = false;

            //calculate percentage rate
            foreach (int marketkey in path)
            {
                Market currentmarket = GetMarketInfo(dtmarkets,dtnodes,dtmarketdata, marketkey);
                currentamount = CalculateTransaction(currentamount, currentmarket,false);

                //calculate static fees
                if (currentmarket.fee_static > 0)
                {
                    switch (currentmarket.source.currency)
                    {
                        case 1: arbitragerate.staticfeeUSD += currentmarket.fee_static;
                            break;
                        case 2: arbitragerate.staticfeeBTC += currentmarket.fee_static;
                            break;
                        case 3: arbitragerate.staticfeeLTC += currentmarket.fee_static;
                            break;
                    }
                }

                //calculate time
                arbitragerate.time += currentmarket.exchangetime;
            }

            arbitragerate.percentage = (currentamount.amount - 1) * 100;
            arbitragerate.isvoid = currentamount.isvoid;            

            return arbitragerate;
        }


        private Value CalculateTransaction(Value currentamount, Market marketinfo, bool includefeestatic)
        {
            Value outputamount = currentamount;
            decimal percentagecalculate = 1 - marketinfo.fee_percentage;            
            bool doinverse = false;

            if ((marketinfo.source.currency == 1) && (marketinfo.destination.currency != 1)){
                doinverse = true;
            }
            else if ((marketinfo.source.currency == 2) && (marketinfo.destination.currency == 3)) //BTC -> LTC
            {
                marketinfo.price = 1 / marketinfo.price;
            }
            else if ((marketinfo.source.currency == 2) && (marketinfo.destination.currency == 4)) //BTC -> DOGE
            {
                marketinfo.price = 1 / marketinfo.price;
            }
            else if ((marketinfo.source.currency == 3) && (marketinfo.destination.currency == 4)) //LTC -> LTC
            {
                marketinfo.price = 1 / marketinfo.price;
            }
            

            //calculate amount
            outputamount.amount = currentamount.amount * marketinfo.price;
            if (doinverse)
            {
                outputamount.amount = 1 / outputamount.amount;
            }

            //add in fee percentage
            outputamount.amount = currentamount.amount * percentagecalculate;


            if (includefeestatic)
            {
                outputamount.amount = outputamount.amount - marketinfo.fee_static;
            }            

            outputamount.currency = marketinfo.destination.currency;

            if (marketinfo.price == 0)
            {
                outputamount.isvoid = true;
            }

            return outputamount;
        }

        public Market GetMarketInfo(DataTable dtmarkets, DataTable dtnodes, DataTable dtmarketdata, int marketkey)
        {
            Market tempmarket = new Market();
            tempmarket.market_key = marketkey;
            tempmarket.source = new MarketNode();
            tempmarket.destination = new MarketNode();

            DataRow dr = dtmarkets.Rows.Find(marketkey);
            
            int sourcenode = Convert.ToInt32(dr["source"]);
            DataRow drnode1 = dtnodes.Rows.Find(sourcenode);
            tempmarket.source.currency = Convert.ToInt32(drnode1["currency"]);
            tempmarket.source.exchange_key = Convert.ToInt32(drnode1["exchange_key"]);

            int destinationnode = Convert.ToInt32(dr["destination"]);
            DataRow drnode2 = dtnodes.Rows.Find(destinationnode);
            tempmarket.destination.currency = Convert.ToInt32(drnode2["currency"]);
            tempmarket.destination.exchange_key = Convert.ToInt32(drnode2["exchange_key"]);

            tempmarket.ratechanges = Convert.ToBoolean(dr["ratechanges"]);
            tempmarket.automatic = Convert.ToBoolean(dr["automatic"]);
            tempmarket.exchangetime = Convert.ToInt32(dr["exchangetime"]);
            tempmarket.fee_percentage = Convert.ToDecimal(dr["fee_percentage"]);
            tempmarket.fee_static = Convert.ToDecimal(dr["fee_static"]);

            if (tempmarket.ratechanges)
            {
                tempmarket.price = Convert.ToDecimal(dtmarketdata.Rows.Find(marketkey)["price"]);
            }
            else
            {
                tempmarket.price = 1;
            }

            return tempmarket;

        }

        public void Update_Paths()
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewPathsAll().GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                try
                {
                    Models.Path currentpath = new Models.Path();
                    currentpath.pathkey = Convert.ToInt32(dr["path_key"]);
                    currentpath.label = dr["path_name"].ToString();
                    DataTable dttemproutes = Bitcoin_Notify_DB.SPs.ViewPathRouteSpecific(currentpath.pathkey).GetDataSet().Tables[0];
                    currentpath.path = new List<int>();
                    foreach (DataRow dr2 in dttemproutes.Rows)
                    {
                        currentpath.path.Add(Convert.ToInt32(dr2["market_key"]));
                    }

                    currentpath.marketdifference = GetArbitrageRate(currentpath.path);

                    if (currentpath.marketdifference.isvoid == false)
                    {
                        fb.UpdatePaths(currentpath);
                    }                    
                }
                catch
                {
                }
            }
        }

        public void Update_Paths_All()
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewExchangesAll().GetDataSet().Tables[0];            

            DataTable dttemppathkey = Bitcoin_Notify_DB.SPs.ViewPathsAll().GetDataSet().Tables[0];
            int pathkey = Convert.ToInt32(dttemppathkey.Rows[dttemppathkey.Rows.Count - 1]["path_key"]);

            foreach (DataRow dr in dttemp.Rows)  //looping through exchanges 1
            {
                int exchange1 = Convert.ToInt32(dr["exchange_key"]);

                List<Tuple<int, int>> hsexchangecurrencypairs = new List<Tuple<int, int>>();
                
                DataTable dtexchangecurrencies = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(exchange1).GetDataSet().Tables[0];
                foreach (DataRow drec in dtexchangecurrencies.Rows)
                {
                    int currency1 = Convert.ToInt32(drec["currency"]);
                    foreach (DataRow drec2 in dtexchangecurrencies.Rows)
                    {    
                        int currency2 = Convert.ToInt32(drec2["currency"]);
                        if (currency1 < currency2)
                        {
                            hsexchangecurrencypairs.Add(new Tuple<int, int>(currency1, currency2));
                        }
                    }                    
                }

                foreach (DataRow dr2 in dttemp.Rows) //looping through exchanges 2
                {
                    int exchange2 = Convert.ToInt32(dr2["exchange_key"]);
                    

                    if (exchange1 != exchange2)  //must be different exchanges
                    {
                        
                        foreach (Tuple<int,int> currencypair in hsexchangecurrencypairs)
                        {
                            //go through all currency pairs exchange1 has

                            //check if exchange 2 has both currencies
                            DataTable dtexchangecurrency2 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(exchange2).GetDataSet().Tables[0];

                            DataColumn[] keyColumns1 = new DataColumn[1];
                            keyColumns1[0] = dtexchangecurrency2.Columns["currency"];
                            dtexchangecurrency2.PrimaryKey = keyColumns1;

                            bool candopairing = false;
                            if ((dtexchangecurrency2.Rows.Contains(currencypair.Item1)) && (dtexchangecurrency2.Rows.Contains(currencypair.Item2)))
                            {
                                candopairing = true;
                            }

                            if (candopairing) //can do currency pairing
                            {
                                DataTable dttemppath = Bitcoin_Notify_DB.SPs.ViewPathByExchangesAndCurrencies(exchange1, exchange2, currencypair.Item1, currencypair.Item2).GetDataSet().Tables[0];

                                Models.Path currentpath = new Models.Path();
                                currentpath.pathkey = 0;
                                currentpath.label = "";
                                currentpath.path = new List<int>();

                                foreach (DataRow drpath in dttemppath.Rows)
                                {
                                    currentpath.path.Add(Convert.ToInt32(drpath["market_key"]));
                                    currentpath.label += drpath["Source_Shortname"].ToString() + "_" + drpath["Source_Currency"].ToString() + "->";
                                }

                                if (currentpath.label.Length > 3)
                                {
                                    currentpath.label = currentpath.label.Substring(0, currentpath.label.Length - 2);
                                }

                                currentpath.marketdifference = GetArbitrageRate(currentpath.path);

                                if (currentpath.marketdifference.isvoid == false)
                                {
                                    pathkey += 1;
                                    currentpath.pathkey = pathkey;
                                    fb.UpdatePaths(currentpath);
                                }
                            }
                            
                        }                        
                    }                    
                }
            }
        }

        public void CaVirtexBTC_CoinbaseBTC()
        {
            //buy CaVirtex BTC -> Sell Coinbase BTC -> Loop money back to CaVirtex

            //market_keys 1 -> 10 -> 55 -> 56
            List<int> path = new List<int>();
            path.Add(1);
            path.Add(10);
            path.Add(55);
            path.Add(56);

            MarketDifference therate = GetArbitrageRate(path);
            

            //update firebase
            /*Hashtable hstemp = new Hashtable();
            hstemp.Add("buying", buying);
            hstemp.Add("selling", selling);
            hstemp.Add("lasttrade", lasttrade);
            hstemp.Add("updated", DateTime.UtcNow.ToString("hh:mm:ss"));
            hstemp.Add("theindex", 0);

            Hashtable hstemp2 = new Hashtable();
            hstemp2.Add("coinbase", hstemp);

            fb.UpdateMarket(hstemp2);*/

            //buy Coinbase BTC -> Sell CaVirtex BTC -> Loop money back to CaVirtex
        }

        public void CaVirtexBTC_KrakenBTC_KrakenLTC_CaVirtexBTC()
        {
            //buy CaVirtex BTC -> Sell Coinbase BTC -> Loop money back to CaVirtex

            //market_keys 7 -> 16 -> 24 -> 6
            List<int> path = new List<int>();
            path.Add(7);
            path.Add(16);
            path.Add(24);
            path.Add(6);

            MarketDifference therate = GetArbitrageRate(path);
            

        }

    }
}