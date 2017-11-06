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
using SubSonic;

namespace Bitcoin_Notify.APIs
{
    public class Market_Updates
    {

        Site sitetemp = new Site();

        public void Update_Markets_Static(Models.Exchange currentexchange)
        {
            //Add Static markets

                
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewMarketsStaticByExchange(currentexchange.exchange_key).GetDataSet().Tables[0];

            foreach (DataRow dr in dttemp.Rows)
            {
                //each market static, gotta put into exchange currency & markets

                int sourcecurrency = Convert.ToInt32(dr["source_currency"]);
                int destinationcurrency = Convert.ToInt32(dr["destination_currency"]);
                decimal feepercentage = Convert.ToDecimal(dr["fee_percentage"]);
                decimal feestatic = Convert.ToDecimal(dr["fee_static"]);
                string apicall = dr["apicall"].ToString();

                Insert_New_Market(currentexchange.exchange_key, sourcecurrency, destinationcurrency, feepercentage, apicall);
                

            }
            

        }


        protected void Insert_New_Market(int exchangekey, int sourcecurrency, int destinationcurrency, decimal feepercentage, string apicall)
        {
            //Insert into Exchange_Currency
            StoredProcedure sp_UpdateExchangeCurrency = Bitcoin_Notify_DB.SPs.UpdateExchangeCurrency(exchangekey, sourcecurrency, 0);
            sp_UpdateExchangeCurrency.Execute();

            int exchangecurrency1 = Convert.ToInt32(sp_UpdateExchangeCurrency.Command.Parameters[2].ParameterValue);

            StoredProcedure sp_UpdateExchangeCurrency2 = Bitcoin_Notify_DB.SPs.UpdateExchangeCurrency(exchangekey, destinationcurrency, 0);
            sp_UpdateExchangeCurrency2.Execute();

            int exchangecurrency2 = Convert.ToInt32(sp_UpdateExchangeCurrency2.Command.Parameters[2].ParameterValue);

            Bitcoin_Notify_DB.SPs.UpdateMarket(0, exchangecurrency1, exchangecurrency2, feepercentage, 0, 0, true, apicall,0).Execute();
            Bitcoin_Notify_DB.SPs.UpdateMarket(0, exchangecurrency2, exchangecurrency1, feepercentage, 0, 0, true, "",0).Execute();

        }

        public void Update_Markets_Automated(int exchangekey)
        {
            //Add markets from API calls
            switch (exchangekey)
            {
                case 3: Update_Kraken_Markets(exchangekey);
                    break;
                case 7: Update_Bittrex_Markets(exchangekey);
                    break;
                case 8: Update_Poloniex_Markets(exchangekey);
                    break;
            }
            

        }

        protected void Update_Kraken_Markets(int exchangekey)
        {
            DataTable dtkrakeninfo = Bitcoin_Notify_DB.SPs.ViewExchangesBykey(exchangekey).GetDataSet().Tables[0];
            decimal kraken_fee_percentage = Convert.ToDecimal(dtkrakeninfo.Rows[0]["fee_percentage"]);

            //first update currencies
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewCurrenciesAll().GetDataSet().Tables[0];

            Hashtable hscurrencies = new Hashtable();
            foreach (DataRow dr in dttemp.Rows)
            {
                hscurrencies.Add( Convert.ToInt32(dr["currency_key"]), dr["currency_name"].ToString());
            }

            string strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/Assets", null);

            try
            {
                JObject o = JObject.Parse(strresponse);

                JObject result = (JObject)o["result"];

                foreach (var x in result)
                {
                    string assetname = x.Key;
                    JObject currencyinfo = (JObject)x.Value;
                    string currencyname = (string)currencyinfo["altname"];

                    Boolean isfiat = false;
                    if ((assetname[0].ToString() == "Z") || (assetname=="USDT"))
                    {
                        //it's fiat
                        isfiat = true;
                    }

                    if ((!hscurrencies.ContainsValue(currencyname)) & (currencyname != "XBT"))
                    {
                        int newcurrencykey = hscurrencies.Count + 1;
                        Bitcoin_Notify_DB.SPs.UpdateCurrencies(newcurrencykey, currencyname, isfiat).Execute();
                        hscurrencies.Add(newcurrencykey, currencyname);
                    }
                }

            }
            catch
            {

            }

            Hashtable hsdelisted = new Hashtable();
            hsdelisted.Add(1,"BTCGBP");
            hsdelisted.Add(2, "ETHGBP");
            hsdelisted.Add(3,"XRPCAD");
            hsdelisted.Add(4, "XRPJPY");
            hsdelisted.Add(5, "EOSEUR");
            hsdelisted.Add(6, "EOSUSD");
            hsdelisted.Add(7, "XLMEUR");
            hsdelisted.Add(8 , "XLMUSD");
            hsdelisted.Add(9, "GNOEUR");
            hsdelisted.Add(10, "GNOUSD");
            hsdelisted.Add(11, "REPUSD");

            strresponse = sitetemp.Web_Request("https://api.kraken.com/0/public/AssetPairs", null);

            try
            {
                JObject o = JObject.Parse(strresponse);

                JObject result = (JObject)o["result"];

                foreach (var x in result)
                {
                    //looping through each market
                    string marketname = x.Key;
                    JObject marketinfo = (JObject)x.Value;

                    string thebase = CleanKrakenCurrency( (string)marketinfo["base"]);
                    string quote = CleanKrakenCurrency( (string)marketinfo["quote"]);

                    DataTable dtcurrency = Bitcoin_Notify_DB.SPs.ViewCurrenciesByname(thebase).GetDataSet().Tables[0];
                    int sourcecurrency = Convert.ToInt32(dtcurrency.Rows[0]["currency_key"]);
                    dtcurrency = Bitcoin_Notify_DB.SPs.ViewCurrenciesByname(quote).GetDataSet().Tables[0];
                    int destinationcurrency = Convert.ToInt32(dtcurrency.Rows[0]["currency_key"]);

                    //filter out for delisted currencies
                    if (hsdelisted.ContainsValue(thebase + quote))
                    {

                    }
                    else
                    {
                        Insert_New_Market(exchangekey, sourcecurrency, destinationcurrency, kraken_fee_percentage, thebase + quote);
                    }


                    

                }

            }
            catch
            {

            }
        }


        protected void Update_Bittrex_Markets(int exchangekey)
        {
            //first update currencies
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewCurrenciesAll().GetDataSet().Tables[0];

            Hashtable hscurrencies = new Hashtable();
            foreach (DataRow dr in dttemp.Rows)
            {
                hscurrencies.Add(Convert.ToInt32(dr["currency_key"]), dr["currency_name"].ToString());
            }

            string strresponse = sitetemp.Web_Request("https://bittrex.com/api/v1.1/public/getcurrencies", null);

            try
            {
                JObject o = JObject.Parse(strresponse);

                JArray result = (JArray)o["result"];

                foreach (JObject x in result.Children())
                {                    
                    //string assetname = (string)x["Currency"];                    
                    string currencyname = (string)x["Currency"];

                    Boolean isfiat = false;
                    if (currencyname == "USDT")
                    {
                        //it's fiat
                        isfiat = true;
                    }

                    if (!hscurrencies.ContainsValue(currencyname))
                    {
                        int newcurrencykey = hscurrencies.Count + 1;
                        Bitcoin_Notify_DB.SPs.UpdateCurrencies(newcurrencykey, currencyname, isfiat).Execute();
                        hscurrencies.Add(newcurrencykey, currencyname);
                    }
                }

            }
            catch
            {

            }


            strresponse = sitetemp.Web_Request("https://bittrex.com/api/v1.1/public/getmarkets", null);

            try
            {
                JObject o = JObject.Parse(strresponse);

                JArray result = (JArray)o["result"];

                foreach (JObject x in result.Children())
                {
                    //looping through each market
                    //string marketname = x.Key;                    

                    string thebase = CleanKrakenCurrency((string)x["MarketCurrency"]);
                    string quote = CleanKrakenCurrency((string)x["BaseCurrency"]);

                    if (thebase == "CLOAK")
                    {

                    }

                    DataTable dtcurrency = Bitcoin_Notify_DB.SPs.ViewCurrenciesByname(thebase).GetDataSet().Tables[0];
                    int sourcecurrency = Convert.ToInt32(dtcurrency.Rows[0]["currency_key"]);
                    dtcurrency = Bitcoin_Notify_DB.SPs.ViewCurrenciesByname(quote).GetDataSet().Tables[0];
                    int destinationcurrency = Convert.ToInt32(dtcurrency.Rows[0]["currency_key"]);

                    Insert_New_Market(exchangekey, sourcecurrency, destinationcurrency, decimal.Parse("0.0025"), thebase + quote);

                }

            }
            catch
            {

            }
        }


        protected void Update_Poloniex_Markets(int exchangekey)
        {
            //first update currencies
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewCurrenciesAll().GetDataSet().Tables[0];

            Hashtable hscurrencies = new Hashtable();
            foreach (DataRow dr in dttemp.Rows)
            {
                hscurrencies.Add(Convert.ToInt32(dr["currency_key"]), dr["currency_name"].ToString());
            }

            string strresponse = sitetemp.Web_Request("https://poloniex.com/public?command=return24hVolume", null);

            
            try
            {
                JObject o = JObject.Parse(strresponse);

                //JArray result = (JArray)o["result"];

                foreach (var x in o)
                {
                    string marketname = x.Key;

                    string[] words = marketname.Split('_');

                    string currency1 = words[0];
                    string currency2 = words[1];

                    try
                    {
                        DataTable dtcurrency = Bitcoin_Notify_DB.SPs.ViewCurrenciesByname(currency1).GetDataSet().Tables[0];
                        int sourcecurrency = Convert.ToInt32(dtcurrency.Rows[0]["currency_key"]);
                        dtcurrency = Bitcoin_Notify_DB.SPs.ViewCurrenciesByname(currency2).GetDataSet().Tables[0];
                        int destinationcurrency = Convert.ToInt32(dtcurrency.Rows[0]["currency_key"]);

                        Insert_New_Market(exchangekey, sourcecurrency, destinationcurrency, decimal.Parse("0.0025"), marketname);                    
                    }
                    catch
                    {

                    }
                    
                }

            }
            catch
            {

            }
        }

        protected string CleanKrakenCurrency(string currency)
        {
            if ((currency.Length ==4) && ((currency[0].ToString()=="X") || (currency[0].ToString() == "Z")))
            {
                currency = currency.Substring(1);
            }

            if (currency == "XBT")
            {
                currency = "BTC";
            }

            return currency;
        }

    }
}