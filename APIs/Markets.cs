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

namespace Bitcoin_Notify.APIs
{
    public class Markets
    {
        Site sitetemp = new Site();
        Firebase fb = new Firebase();
        decimal lasttrade = 0;
        string strtemp = "";
        string coinmktapikey = ConfigurationSettings.AppSettings["Coinmkt_API_Key"];
        
        int btccurrencykey = 1;
        int ltccurrencykey = 2;
        int ethcurrencykey = 3;
        int xrpcurrencykey = 4;
        int usdcurrencykey = 5;
        int cadcurrencykey = 6;
        decimal btcmultiplier = 2500;
        decimal ltcmultiplier = 40;
        decimal xrpmultiplier = decimal.Parse("0.25");
        decimal ethmultiplier = 295;
        int apierrorcountmax = 100;
        decimal orderbookdepthpercentage = decimal.Parse(" 0.08");

        public void UpdateExchange(int exchangekey)
        {            
            switch (exchangekey)
            {
                case 1: //quadriga
                    UpdateQuadriga(exchangekey);
                    break;
                case 2: //Coinbase
                    UpdateGDax(exchangekey);
                    break;
                case 3: //Kraken
                    UpdateKraken(exchangekey);
                    break;
                case 4: //Bitstamp
                    UpdateBitstamp(exchangekey);
                    break;
                case 5: //Bitflyer
                    UpdateBitflyer(exchangekey);
                    break;
                case 6: //coinsph
                    UpdateCoinsph(exchangekey);
                    break;
                case 8: //Poloniex
                    UpdatePoloniex(exchangekey);
                    break;
                case 7: //Bitrex
                    UpdateBittrex(exchangekey);
                    break;
                case 9: //Gemnini
                    UpdateGemini(exchangekey);
                    break;
            }
        }

        public void UpdateCurrencyCloudRatesAll()
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewCurrencyCloudMarkets().GetDataSet().Tables[0];
            CurrencyCloud cc = new CurrencyCloud();
            

            foreach (DataRow dr in dttemp.Rows)
            {
                string sourcecurrency = dr["sourcename"].ToString();
                string destinationcurrency = dr["destinationname"].ToString();
                decimal exchangerate = 1;

                try
                {
                    //exchangerate = cc.getexchangerate(sourcecurrency + destinationcurrency);
                    exchangerate = sitetemp.Exchange_Rate_ccy_pair_Doupdate(sourcecurrency, destinationcurrency);

                    if (exchangerate <= decimal.Parse("0.0001"))
                    {
                        //gotta find inverse of it
                        exchangerate = sitetemp.Exchange_Rate_ccy_pair_Doupdate(destinationcurrency, sourcecurrency);
                        exchangerate = 1 / exchangerate;
                    }

                    Bitcoin_Notify_DB.SPs.UpdateCurrencyCloudMarketData(exchangerate, Convert.ToInt32(dr["currencycloud_market_key"])).Execute();
                }
                catch{
                    
                }

                
            }
        }

        public void UpdateCoinsph(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                int apierrorcount = 0;
                string apicall = dr["apicall"].ToString();

                if (apicall.Length > 0)
                {
                    string strresponse = sitetemp.Web_Request("https://quote.coins.ph/v1/markets/" + currentsourcecurrency.ToUpper() + "-" + currentdestinationcurrency.ToUpper(), null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JObject market = (JObject)o["market"];


                        decimal selling = Convert.ToDecimal( (string)market["bid"]);
                        decimal sellingvolume = 100000000;
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        decimal buying = 1 / Convert.ToDecimal( (string)market["ask"]);
                        decimal buyingvolume = 100000000;
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }
        }

        public void UpdateKorbit(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                int apierrorcount = 0;

                if (dr["apierrorcount"] != DBNull.Value)
                {
                    apierrorcount = Convert.ToInt32(dr["apierrorcount"]);
                }

                if (apierrorcount < apierrorcountmax)
                {
                    string strresponse = sitetemp.Web_Request("https://api.korbit.co.kr/v1/orderbook?currency_pair=" + currentsourcecurrency.ToLower() + "_" + currentdestinationcurrency.ToLower(), null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JArray o4 = (JArray)asks[0];
                        decimal selling = decimal.Parse((string)o4[0]);
                        decimal sellingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JArray)bids[0];
                        decimal buying = 1 / decimal.Parse((string)o4[0]);
                        decimal buyingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }
        }

        public void UpdateBitflyer(int currentexchangekey)
        {
           DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                int apierrorcount = 0;
                string apicall = dr["apicall"].ToString();
                

                if (apicall.Length > 0) 
                {
                    string strresponse = sitetemp.Web_Request("http://api.bitflyer.jp/v1/getboard?product_code=" + apicall, null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JObject o4 = (JObject)asks[0];
                        decimal selling = (decimal)o4["price"];
                        decimal sellingvolume = (decimal)o4["size"];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JObject)bids[0];
                        decimal buying = 1 / (decimal)o4["price"];
                        decimal buyingvolume = (decimal)o4["size"];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }
            

            }
        }


        public void UpdateLuno(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                int apierrorcount = 0;

                if (currentsourcecurrency == "BTC")
                {
                    currentsourcecurrency = "XBT";
                }
                if (currentdestinationcurrency == "BTC")
                {
                    currentdestinationcurrency = "XBT";
                }

                if (dr["apierrorcount"] != DBNull.Value)
                {
                    apierrorcount = Convert.ToInt32(dr["apierrorcount"]);
                }

                if (apierrorcount < apierrorcountmax)
                {
                    //https://api.mybitx.com/api/1/ticker?pair=XBTZAR
                    string strresponse = sitetemp.Web_Request("https://api.mybitx.com/api/1/orderbook?pair=" + currentsourcecurrency.ToUpper() + currentdestinationcurrency.ToUpper(), null);

                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JObject o4 = (JObject)bids[0];
                        decimal selling = decimal.Parse((string)o4["price"]);
                        decimal sellingvolume = decimal.Parse((string)o4["volume"]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JObject)asks[0];
                        decimal buying = 1 / decimal.Parse((string)o4["price"]);
                        decimal buyingvolume = decimal.Parse((string)o4["volume"]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }

                
            }
        }



        public void UpdateCoinone(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                int apierrorcount = 0;

                if (dr["apierrorcount"] != DBNull.Value)
                {
                    apierrorcount = Convert.ToInt32(dr["apierrorcount"]);
                }

                if ((apierrorcount < apierrorcountmax) && (currentsourcecurrency == "KRW"))
                {
                    string strresponse = sitetemp.Web_Request("https://api.coinone.co.kr/orderbook?currency=" + currentdestinationcurrency.ToLower(), null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JArray bids = (JArray)o["bid"];
                        JArray asks = (JArray)o["ask"];

                        JObject o4 = (JObject) asks[0];
                        decimal selling = 1/ decimal.Parse((string)o4["price"]);
                        decimal sellingvolume = decimal.Parse((string)o4["qty"]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JObject)bids[0];
                        decimal buying = decimal.Parse((string)o4["price"]);
                        decimal buyingvolume = decimal.Parse((string)o4["qty"]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }
        }

        public void UpdateQuadriga(int currentexchangekey)
        {
            
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                string apicall = dr["apicall"].ToString();
                
                decimal currentmultiplier = 0;
                if (currentsourcecurrency == "BTC")
                {
                    currentmultiplier = btcmultiplier;
                }
                else
                {
                    currentmultiplier = ethmultiplier;
                }

                if (apicall.Length > 0)
                {
                    string strresponse = sitetemp.Web_Request("https://api.quadrigacx.com/v2/order_book?book=" + apicall, null);

                    string strresponse2 = sitetemp.Web_Request("https://api.quadrigacx.com/v2/ticker?book=" + apicall, null);

                    try
                    {
                        JObject o = JObject.Parse(strresponse);
                        JObject oticker = JObject.Parse(strresponse2);
                        decimal marketvolume = Convert.ToDecimal((string)oticker["volume"]) * currentmultiplier;

                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JArray o4 = (JArray)bids[0];
                        decimal selling = decimal.Parse((string)o4[0]);
                        decimal sellingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, marketvolume, marketkey).Execute();

                        o4 = (JArray)asks[0];
                        decimal buying = 1/decimal.Parse((string)o4[0]);
                        decimal buyingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, marketvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }
                

            }
                
        }

        public void UpdateGDax(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                string apicall = dr["apicall"].ToString();

                
                if (apicall.Length > 0)
                {
                    string strresponse = sitetemp.Web_Request("https://api.gdax.com/products/" + apicall + "/book?level=25", null);

                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JArray o4 = (JArray)bids[0];
                        decimal selling = decimal.Parse((string)o4[0]);
                        decimal sellingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JArray)asks[0];
                        decimal buying = 1 / decimal.Parse((string)o4[0]);
                        decimal buyingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();


                        Bitcoin_Notify_DB.SPs.DeleteMarketOrderbooks(marketkey).Execute();
                        Bitcoin_Notify_DB.SPs.DeleteMarketOrderbooks(marketkeyopposite).Execute();
                        //insert orderbook into db - max 8% diff
                        foreach (JArray item in bids.Children())
                        {
                            o4 = item;
                            decimal currentprice = decimal.Parse((string)o4[0]);
                            decimal currentvolume = decimal.Parse((string)o4[1]);

                            decimal currentdifference = (selling / currentprice) - 1;
                            if (currentdifference > orderbookdepthpercentage)
                            {
                                continue;
                            }

                            Bitcoin_Notify_DB.SPs.UpdateMarketOrderbooks(marketkey, true, currentprice, currentvolume).Execute();
                        }

                        foreach (JArray item in asks.Children())
                        {
                            o4 = item;
                            decimal currentprice = 1 / decimal.Parse((string)o4[0]);
                            decimal currentvolume = decimal.Parse((string)o4[1]);

                            decimal currentdifference = (buying / currentprice) - 1;
                            if (currentdifference > orderbookdepthpercentage)
                            {
                                continue;
                            }

                            Bitcoin_Notify_DB.SPs.UpdateMarketOrderbooks(marketkey, true, currentprice, currentvolume).Execute();
                        }


                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }

                
            }
        }


        public void UpdateGemini(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                string apicall = dr["apicall"].ToString();


                if (apicall.Length > 0)
                {
                    string strresponse = sitetemp.Web_Request("https://api.gemini.com/v1/book/" + apicall, null);

                    try
                    {
                        JObject o = JObject.Parse(strresponse);


                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JObject o4 = (JObject)bids[0];
                        decimal selling = decimal.Parse((string)o4["price"]);
                        decimal sellingvolume = decimal.Parse((string)o4["amount"]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JObject)asks[0];
                        decimal buying = 1 / decimal.Parse((string)o4["price"]);
                        decimal buyingvolume = decimal.Parse((string)o4["amount"]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }
        }



        public void UpdateKraken(int currentexchangekey)
        {
            try
            {
                DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeForAPICall(currentexchangekey).GetDataSet().Tables[0];
                foreach (DataRow dr in dttemp.Rows)
                {                    
                    string currentsourcecurrency = dr["source_currency"].ToString();
                    string currentdestinationcurrency = dr["destination_currency"].ToString();
                    int marketkey = Convert.ToInt32(dr["market_key"]);
                    int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                    string apicall = dr["apicall"].ToString();

                    //figure out multiplier to convert volume to USD
                    decimal currentmultiplier = 0;
                    switch (currentsourcecurrency)
                    {
                        case "BTC": currentmultiplier = btcmultiplier;
                            break;
                        case "LTC": currentmultiplier = ltcmultiplier;
                            break;
                        case "XRP": currentmultiplier = xrpmultiplier;
                            break;
                        case "ETH": currentmultiplier = ethmultiplier;
                            break;
                    }

                    if (currentsourcecurrency == "BTC")
                    {
                        currentsourcecurrency = "XBT";
                    }
                    if (currentdestinationcurrency == "BTC")
                    {
                        currentdestinationcurrency = "XBT";
                    }

                    int apierrorcount = 0;

                    if (dr["apierrorcount"] != DBNull.Value)
                    {
                        apierrorcount = Convert.ToInt32(dr["apierrorcount"]);
                    }

                    if (apicall.Length > 0)
                    {                        

                        string strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/Ticker?pair=" + currentsourcecurrency + currentdestinationcurrency, null);

                        try
                        {
                            JObject o = JObject.Parse(strresponse);

                            JArray error = (JArray)o["error"];
                            if (error.Count == 0)
                            {
                                string firstone = "X";
                                string secondone = "X";
                                if (Convert.ToBoolean(dr["source_currency_isfiat"]))
                                {
                                    firstone = "Z";
                                }
                                if (Convert.ToBoolean(dr["destination_currency_isfiat"]))
                                {
                                    secondone = "Z";
                                }



                                JObject o2 = (JObject)o["result"];
                                JObject o3 = (JObject)o2[firstone + currentsourcecurrency + secondone + currentdestinationcurrency];

                                if (o3 == null)
                                {
                                    o3 = (JObject)o2[currentsourcecurrency + currentdestinationcurrency];
                                }

                                JArray o4 = (JArray)o3["b"];
                                JArray o5 = (JArray)o3["v"];
                                decimal selling = decimal.Parse((string)o4[0]);
                                decimal sellingvolume = decimal.Parse((string)o5[1]) * currentmultiplier;
                                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                                o4 = (JArray)o3["a"];
                                decimal buying = 1 / decimal.Parse((string)o4[0]);
                                decimal buyingvolume = decimal.Parse((string)o5[1]) * currentmultiplier;
                                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();
                            }
                            else
                            {
                                Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                            }
                        }
                        catch
                        {

                        }
                        
                    }
                    

                }


                /*
                //XBT USD
                string strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/Ticker?pair=XBTUSD", null);

                JObject o = JObject.Parse(strresponse);
                JObject o2 = (JObject)o["result"];
                JObject o3 = (JObject)o2["XXBTZUSD"];

                JArray o4 = (JArray)o3["a"];
                selling = decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 12).Execute();

                o4 = (JArray)o3["b"];
                buying = decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 13).Execute();

                //LTC USD
                strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/Ticker?pair=LTCUSD", null);

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["result"];
                o3 = (JObject)o2["XLTCZUSD"];

                o4 = (JArray)o3["a"];
                selling = decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 14).Execute();

                o4 = (JArray)o3["b"];
                buying = decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 15).Execute();


                //LTC XBT
                strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/Ticker?pair=XBTLTC", null);

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["result"];
                o3 = (JObject)o2["XXBTXLTC"];

                o4 = (JArray)o3["b"];
                buying = 1 / decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 16).Execute();

                o4 = (JArray)o3["a"];
                selling = 1 / decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 17).Execute();

                //XBT XDG
                strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/Ticker?pair=XBTXDG", null);

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["result"];
                o3 = (JObject)o2["XXBTXXDG"];

                o4 = (JArray)o3["b"];
                buying = 1 / decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 103).Execute();

                o4 = (JArray)o3["a"];
                selling = 1 / decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 104).Execute();

                //LTC XDG
                strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/Ticker?pair=LTCXDG", null);

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["result"];
                o3 = (JObject)o2["XLTCXXDG"];

                o4 = (JArray)o3["b"];
                buying = 1 / decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 105).Execute();

                o4 = (JArray)o3["a"];
                selling = 1 / decimal.Parse((string)o4[0]);
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 106).Execute();
                 * */
            }
            catch
            {
            }
        }


        public void UpdateBtce(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                int apierrorcount = 0;

                if (dr["apierrorcount"] != DBNull.Value)
                {
                    apierrorcount = Convert.ToInt32(dr["apierrorcount"]);
                }

                if (apierrorcount == 0)
                {
                    string strresponse = sitetemp.Web_Request("https://btc-e.com/api/3/depth/" + currentsourcecurrency.ToLower() + "_" + currentdestinationcurrency.ToLower(), null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JObject o2 = (JObject)o[currentsourcecurrency.ToLower() + "_" + currentdestinationcurrency.ToLower()];

                        JArray bids = (JArray)o2["bids"];
                        JArray asks = (JArray)o2["asks"];

                        JArray o4 = (JArray)bids[0];
                        decimal selling = (decimal)o4[0];
                        decimal sellingvolume = (decimal)o4[1];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JArray)asks[0];
                        decimal buying = 1 / (decimal)o4[0];
                        decimal buyingvolume = (decimal)o4[1];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }


            

            /*try
            {

                //BTC USD
                string strresponse = sitetemp.Web_Request("https://btc-e.com/api/2/btc_usd/ticker", null);

                JObject o = JObject.Parse(strresponse);
                JObject o2 = (JObject)o["ticker"];

                selling = (decimal)o2["buy"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 41).Execute();

                buying = (decimal)o2["sell"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 42).Execute();

                //LTC USD
                strresponse = sitetemp.Web_Request("https://btc-e.com/api/2/ltc_usd/ticker", null);

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["ticker"];

                selling = (decimal)o2["buy"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 43).Execute();

                buying = (decimal)o2["sell"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 44).Execute();

                //LTC BTC
                strresponse = sitetemp.Web_Request("https://btc-e.com/api/2/ltc_btc/ticker", null);

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["ticker"];

                selling = (decimal)o2["buy"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 45).Execute();

                buying = (decimal)o2["sell"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 46).Execute();
            }
            catch
            {
            }*/
        }




        public void UpdateBitstamp(int currentexchangekey)
        {

            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                string apicall = dr["apicall"].ToString();

                if (apicall.Length > 0)
                {
                    string strresponse = sitetemp.Web_Request("https://www.bitstamp.net/api/v2/order_book/" + apicall, null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JArray o4 = (JArray)bids[0];
                        decimal selling = decimal.Parse((string)o4[0]);
                        decimal sellingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JArray)asks[0];
                        decimal buying = 1 / decimal.Parse((string)o4[0]);
                        decimal buyingvolume = decimal.Parse((string)o4[1]);
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }

            
        }
        

        public void UpdateMtGox()
        {
            /*decimal buying = 0;
            decimal selling = 0;
            decimal lasttrade = 0;
            string strtemp = "";

            string strresponse = sitetemp.Web_Request("http://data.mtgox.com/api/2/BTCUSD/money/ticker_fast", null);

            JObject o = JObject.Parse(strresponse);
            JObject o2 = (JObject)o["data"];
            
            JObject o3 = (JObject)o2["buy"];
            strtemp = (string)o3["value"];
            buying = decimal.Parse(strtemp);

            o3 = (JObject)o2["last"];
            strtemp = (string)o3["value"];
            lasttrade = decimal.Parse(strtemp); 

            o3 = (JObject)o2["sell"];
            strtemp = (string)o3["value"];
            selling = decimal.Parse(strtemp);

            //update db
            Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, selling, lasttrade, 3).Execute();

            //update firebase
            Hashtable hstemp = new Hashtable();
            hstemp.Add("buying", buying);
            hstemp.Add("selling", selling);
            hstemp.Add("lasttrade", lasttrade);
            hstemp.Add("updated", DateTime.UtcNow.ToString("hh:mm:ss"));
            hstemp.Add("theindex", 2);

            Hashtable hstemp2 = new Hashtable();
            hstemp2.Add("mtgox", hstemp);
            
            fb.UpdateMarket(hstemp2);*/
        }


        public void UpdateCryptsy()
        {
            try
            {
                string strresponse = sitetemp.Web_Request("http://pubapi.cryptsy.com/api.php?method=singlemarketdata&marketid=3", null); //LTC-BTC

                JObject o = JObject.Parse(strresponse);
                JObject o2 = (JObject)o["return"];
                JObject o3 = (JObject)o2["markets"];
                JObject o4 = (JObject)o3["LTC"];

                JArray sellorders = (JArray)o4["sellorders"];
                JObject s1 = (JObject)sellorders[0];
                decimal sellprice = decimal.Parse((string)s1["price"]);

                JArray buyorders = (JArray)o4["buyorders"];
                JObject b1 = (JObject)buyorders[0];
                decimal buyprice = decimal.Parse((string)b1["price"]);

                Bitcoin_Notify_DB.SPs.UpdateMarketData(sellprice, 0, 79).Execute();
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buyprice, 0, 80).Execute();

                strresponse = sitetemp.Web_Request("http://pubapi.cryptsy.com/api.php?method=singlemarketdata&marketid=135", null); //DOGE-LTC

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["return"];
                o3 = (JObject)o2["markets"];
                o4 = (JObject)o3["DOGE"];

                sellorders = (JArray)o4["sellorders"];
                s1 = (JObject)sellorders[0];
                sellprice = decimal.Parse((string)s1["price"]);

                buyorders = (JArray)o4["buyorders"];
                b1 = (JObject)buyorders[0];
                buyprice = decimal.Parse((string)b1["price"]);

                Bitcoin_Notify_DB.SPs.UpdateMarketData(sellprice, 0, 101).Execute();
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buyprice, 0, 102).Execute();

                strresponse = sitetemp.Web_Request("http://pubapi.cryptsy.com/api.php?method=singlemarketdata&marketid=132", null); //DOGE-BTC

                o = JObject.Parse(strresponse);
                o2 = (JObject)o["return"];
                o3 = (JObject)o2["markets"];
                o4 = (JObject)o3["DOGE"];

                sellorders = (JArray)o4["sellorders"];
                s1 = (JObject)sellorders[0];
                sellprice = decimal.Parse((string)s1["price"]);

                buyorders = (JArray)o4["buyorders"];
                b1 = (JObject)buyorders[0];
                buyprice = decimal.Parse((string)b1["price"]);

                Bitcoin_Notify_DB.SPs.UpdateMarketData(sellprice, 0, 99).Execute();
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buyprice, 0, 100).Execute();
            }
            catch
            {

            }
        }

        public void UpdatePoloniex(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                string apicall = dr["apicall"].ToString();

                if (currentsourcecurrency.ToUpper() == "USD")
                {
                    currentsourcecurrency = "USDT";
                }

                if (currentdestinationcurrency.ToUpper() == "USD")
                {
                    currentdestinationcurrency = "USDT";
                }

                if (apicall.Length > 0)
                {
                    string strresponse = sitetemp.Web_Request("https://poloniex.com/public?command=returnOrderBook&depth=10&currencyPair=" + currentsourcecurrency.ToUpper() + "_" + currentdestinationcurrency.ToUpper(), null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JArray bids = (JArray)o["bids"];
                        JArray asks = (JArray)o["asks"];

                        JArray o4 = (JArray)asks[0];
                        decimal selling = 1 / decimal.Parse((string)o4[0]);
                        decimal sellingvolume = (decimal)o4[1];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JArray)bids[0];
                        decimal buying =  decimal.Parse((string)o4[0]);
                        decimal buyingvolume = (decimal)o4[1];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }
        }

        public void UpdateBittrex(int currentexchangekey)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsByExchangeInternally(currentexchangekey).GetDataSet().Tables[0];
            foreach (DataRow dr in dttemp.Rows)
            {
                string currentsourcecurrency = dr["source_currency"].ToString();
                string currentdestinationcurrency = dr["destination_currency"].ToString();
                int marketkey = Convert.ToInt32(dr["market_key"]);
                int marketkeyopposite = Convert.ToInt32(dr["market_key_opposite"]);
                string apicall = dr["apicall"].ToString();

                if (currentsourcecurrency.ToUpper() == "USD")
                {
                    currentsourcecurrency = "USDT";
                }

                if (currentdestinationcurrency.ToUpper() == "USD")
                {
                    currentdestinationcurrency = "USDT";
                }

                if (apicall.Length > 0)
                {
                    string strresponse = sitetemp.Web_Request("https://bittrex.com/api/v1.1/public/getorderbook?type=both&market=" + currentdestinationcurrency.ToUpper() + "-" + currentsourcecurrency.ToUpper(), null);


                    try
                    {
                        JObject o = JObject.Parse(strresponse);

                        JObject o2 = (JObject)o["result"];

                        JArray bids = (JArray)o2["buy"];
                        JArray asks = (JArray)o2["sell"];

                        JObject o4 = (JObject)bids[0];
                        decimal selling = (decimal)o4["Rate"];
                        decimal sellingvolume = (decimal)o4["Quantity"];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, sellingvolume, marketkey).Execute();

                        o4 = (JObject)asks[0];
                        decimal buying = 1/ (decimal)o4["Rate"];
                        decimal buyingvolume = (decimal)o4["Quantity"];
                        Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, buyingvolume, marketkeyopposite).Execute();

                    }
                    catch
                    {
                        Bitcoin_Notify_DB.SPs.UpdateMarketAPIErrorsCount(marketkey).Execute();
                    }
                }


            }
        }

        public void UpdateCaVirtex()
        {
            try
            {
            

            /*string strresponse = sitetemp.Web_Request("https://www.cavirtex.com/api/CAD/orderbook.json",null);

            List<Cavirtexobject> asklist = new List<Cavirtexobject>();

            JObject o = JObject.Parse(strresponse);
            JArray asks = (JArray)o["asks"];
            foreach (var item in asks)
            {
                //    KeyValuePair<decimal, decimal> kv = new KeyValuePair<decimal, decimal>(Convert.ToDecimal(item.First.ToString()), Convert.ToDecimal(item.Last.ToString()));
                Cavirtexobject kv = new Cavirtexobject();
                kv.btc = Convert.ToDecimal(item.Last.ToString());
                kv.price = Convert.ToDecimal(item.First.ToString());

                asklist.Add(kv);
            }

            // Acquire keys and sort them.
            var list = asklist.ToList();
            list.Sort();
            selling = list.Last().price;

            //now find buying amount
            asklist.Clear();
            JArray bids = (JArray)o["bids"];
            foreach (var item in bids)
            {
                //    KeyValuePair<decimal, decimal> kv = new KeyValuePair<decimal, decimal>(Convert.ToDecimal(item.First.ToString()), Convert.ToDecimal(item.Last.ToString()));
                Cavirtexobject kv = new Cavirtexobject();
                kv.btc = Convert.ToDecimal(item.Last.ToString());
                kv.price = Convert.ToDecimal(item.First.ToString());

                asklist.Add(kv);
            }

            // Acquire keys and sort them.
            list = asklist.ToList();
            list.Sort();
            buying = list.First().price;

            strresponse = sitetemp.Web_Request("https://www.cavirtex.com/api/CAD/trades.json", null);
            JArray jlasttrade = JArray.Parse(strresponse);
            lasttrade = (decimal)jlasttrade.Last["price"];*/
            
            string strresponse = sitetemp.Web_Request("http://cavirtex.com/api2/ticker.json", null);

            JObject o = JObject.Parse(strresponse);
            JObject o2 = (JObject)o["ticker"];

            //get exchange rate
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewExchangeRates(1).GetDataSet().Tables[0];
            DateTime datetemp = Convert.ToDateTime(dttemp.Rows[0]["lastchanged"]);
            decimal exchangerate = 0;
            if (DateTime.Now > datetemp.AddHours(1))
            {
                //after hour update the exchange rate
                CurrencyCloud cc = new CurrencyCloud();
                exchangerate = cc.getexchangerate("CADUSD");
                Bitcoin_Notify_DB.SPs.UpdateExchangeRates(1, exchangerate).Execute();
            }
            else
            {
                exchangerate = Convert.ToDecimal(dttemp.Rows[0]["exchange_rate"]);
            }
            
            
            //BTC CAD
            JObject o3 = (JObject)o2["BTCCAD"];
            decimal buying = (decimal)o3["buy"];
            decimal selling = (decimal)o3["sell"];
            lasttrade = (decimal)o3["last"];

            buying = buying * exchangerate;
            selling = selling * exchangerate;

            //update db
            Bitcoin_Notify_DB.SPs.UpdateMarketData(selling,0,1).Execute();
            Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 2).Execute();

            //LTC CAD
            o3 = (JObject)o2["LTCCAD"];
            buying = (decimal)o3["buy"];
            selling = (decimal)o3["sell"];
            lasttrade = (decimal)o3["last"];

            buying = buying * exchangerate;
            selling = selling * exchangerate;

            //update db
            Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 3).Execute();
            Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 4).Execute();

            //BTC LTC
            o3 = (JObject)o2["BTCLTC"];
            buying = 1/(decimal)o3["buy"];            
            selling = 1/(decimal)o3["sell"];
            lasttrade = (decimal)o3["last"];
            //update db
            Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 5).Execute();
            Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 6).Execute();


            /*
            //update firebase
            Hashtable hstemp = new Hashtable();
            hstemp.Add("buying",buying);
            hstemp.Add("selling",selling);
            hstemp.Add("lasttrade",lasttrade);
            hstemp.Add("updated", DateTime.UtcNow.ToString("hh:mm:ss"));
            hstemp.Add("theindex", 3);

            Hashtable hstemp2 = new Hashtable();
            hstemp2.Add("cavirtex",hstemp);
            
            fb.UpdateMarket(hstemp2);*/
            }
            catch
            {
            }
        }

    }

    class Cavirtexobject : IComparable<Cavirtexobject>
    {
        public decimal price { get; set; }
        public decimal btc { get; set; }

        public int CompareTo(Cavirtexobject other)
        {
            // Alphabetic sort if salary is equal. [A to Z]
            if (this.price == other.price)
            {
                return this.price.CompareTo(other.price);
            }
            // Default to salary sort. [High to low]
            return other.price.CompareTo(this.price);
        }

        public override string ToString()
        {
            // String representation.
            return this.price.ToString() + "," + this.btc;
        }
    }

}