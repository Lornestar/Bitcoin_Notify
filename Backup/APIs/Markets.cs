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
        decimal buying = 0;
        decimal selling = 0;
        decimal lasttrade = 0;
        string strtemp = "";
        string coinmktapikey = ConfigurationSettings.AppSettings["Coinmkt_API_Key"];

        public void UpdateCoinbase0()
        {
            UpdateCoinbase(0);            
        }

        public void UpdateCoinbase1()
        {
            UpdateCoinbase(1);            
        }

        public void UpdateCoinbase(int lookup)
        {
            try
            {
                if (lookup == 0)
                {
                    string strresponse = sitetemp.Web_Request("https://coinbase.com/api/v1/prices/buy", null);

                    JObject o = JObject.Parse(strresponse);
                    JObject o2 = (JObject)o["subtotal"];
                    string newamount = (string)o2["amount"];
                    selling = decimal.Parse(newamount);
                    //selling += Convert.ToDecimal(0.3);
                    Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 800, 54).Execute();
                }
                else
                {
                    string strresponse = sitetemp.Web_Request("https://coinbase.com/api/v1/prices/sell", null);

                    JObject o = JObject.Parse(strresponse);
                    JObject o2 = (JObject)o["subtotal"];
                    string newamount = (string)o2["amount"];
                    buying = decimal.Parse(newamount);
                    //buying -= Convert.ToDecimal(0.3);

                    //update db

                    Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 800, 55).Execute();
                }                

                /*
                //update firebase
                Hashtable hstemp = new Hashtable();
                hstemp.Add("buying", buying);
                hstemp.Add("selling", selling);
                hstemp.Add("lasttrade", lasttrade);
                hstemp.Add("updated", DateTime.UtcNow.ToString("hh:mm:ss"));
                hstemp.Add("theindex", 0);

                Hashtable hstemp2 = new Hashtable();
                hstemp2.Add("coinbase", hstemp);            
            
                fb.UpdateMarket(hstemp2);*/
            }
            catch
            {
            }
        }

        public void UpdateKraken()
        {
            try
            {

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
            }
            catch
            {
            }
        }


        public void UpdateBtce()
        {
            try
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
            }
        }


        public void UpdateCoinmkt()
        {
            try
            {                

                //BTC USD
                string strresponse1 = sitetemp.Web_Request("https://api.coinmkt.com/v1/ticker/" + coinmktapikey + "/BTC_USD/1", null);

                JObject jo1 = JObject.Parse(strresponse1);

                selling = (decimal)jo1["Ask"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 28).Execute();

                buying = (decimal)jo1["Bid"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 29).Execute();
                               
            }
            catch
            {
            }
        }

        public void UpdateCoinmkt2()
        {
            try
            {
                //LTC USD
                string strresponse2 = sitetemp.Web_Request("https://api.coinmkt.com/v1/ticker/" + coinmktapikey + "/LTC_USD/1", null);

                JObject jo2 = JObject.Parse(strresponse2);

                selling = (decimal)jo2["Ask"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 30).Execute();

                buying = (decimal)jo2["Bid"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 31).Execute();
            }
            catch
            {
            }
        }

        public void UpdateCoinmkt3()
        {
            try
            {
                //LTC BTC
                string strresponse3 = sitetemp.Web_Request("https://api.coinmkt.com/v1/ticker/" + coinmktapikey + "/LTC_BTC/1", null);

                JObject jo3 = JObject.Parse(strresponse3);

                selling = (decimal)jo3["Ask"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(selling, 0, 32).Execute();

                buying = (decimal)jo3["Bid"];
                Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, 0, 33).Execute();
            }
            catch
            {
            }
        }



        public void UpdateBitstamp()
        {
            try
            {
                string strresponse = sitetemp.Web_Request("https://www.bitstamp.net/api/ticker/", null);

                JObject o = JObject.Parse(strresponse);
                strtemp = (string)o["bid"];
                buying = decimal.Parse(strtemp);
                strtemp = (string)o["ask"];
                selling = decimal.Parse(strtemp);
                strtemp = (string)o["last"];
                lasttrade = decimal.Parse(strtemp);

                //update db
                //Bitcoin_Notify_DB.SPs.UpdateMarketData(buying, selling, lasttrade, 4).Execute();

                //update firebase
                Hashtable hstemp = new Hashtable();
                hstemp.Add("buying", buying);
                hstemp.Add("selling", selling);
                hstemp.Add("lasttrade", lasttrade);
                hstemp.Add("updated", DateTime.UtcNow.ToString("hh:mm:ss"));
                hstemp.Add("theindex", 1);

                Hashtable hstemp2 = new Hashtable();
                hstemp2.Add("bitstamp", hstemp);

                fb.UpdateMarket(hstemp2);
            }
            catch
            {
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
            buying = (decimal)o3["buy"];            
            selling = (decimal)o3["sell"];
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