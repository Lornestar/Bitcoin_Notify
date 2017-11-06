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
using System.Threading;
using Bitcoin_Notify.Models;

namespace Bitcoin_Notify
{
    public class Differences
    {
        Site sitetemp = new Site();
        APIs.Firebase fb = new APIs.Firebase();
        Hashtable hsnumberofthreads = new Hashtable();
        public delegate Models.Path AsyncMethodCaller(List<int> path, DataTable dtmarketdata, DataTable dtmarkets, DataTable dtnodes, Models.Path currentpath, DataTable dtmarketorderbooks);
        List<Models.Path> allthepathsglobal = new List<Models.Path>();
        decimal percentagethreashold = 0;
        
        

        private PathDepthInfo GetMinVolumeAlongPath(List<int> path, DataTable dtmarketdata, DataTable dtmarkets, DataTable dtnodes, DataTable dtmarketorderbooks, Boolean startingmaker)
        {
            PathDepthInfo pathdepthinfo = new PathDepthInfo();
            decimal minvolumeforalldepths = 0;
            decimal minprofitforalldepths = 0;
            decimal minvolume = 0;

            //set first buy data
            int startingmarket = path[0];
            int startingnode = Convert.ToInt32(dtmarkets.Rows.Find(startingmarket)["source"]);

            Boolean donecalculatingdepth = false;
            int counter = 0;
            Hashtable hscumulativedepths = new Hashtable();
            foreach (int marketkey in path)
            {
                Depth dp = new Depth();
                dp.orderbookdepth = 1;
                dp.marketkey = marketkey;
                DataRow[] result = dtmarketorderbooks.Select("market_key = " + marketkey.ToString() + " AND depth = 1");
                try
                {
                    dp.currentdepthamountusd = Convert.ToDecimal(result[0]["volumeusd"]);
                    
                }
                catch {
                    dp.currentdepthamountusd = 0;
                }
                hscumulativedepths.Add(counter, dp);
                counter++;
            }

            int lastpathtomove = 0;
            while (donecalculatingdepth==false)
            {
                decimal currentlowestpoint = 0;
                int pathnumbertomove = 0;
                

                foreach (DictionaryEntry entry in hscumulativedepths)
                {
                    Depth dp = (Depth)entry.Value;
                    decimal amounttoconsider = dp.cumulativedepthusd;
                    if (dp.orderbookdepth == 1)
                    {
                        amounttoconsider = dp.currentdepthamountusd;
                    }
                    else if (dp.orderbookdepth > 1)
                    {
                        amounttoconsider += dp.currentdepthamountusd;
                    }
                    
                    if ((currentlowestpoint == 0) || ((currentlowestpoint > amounttoconsider) && (amounttoconsider > 0)))
                    {                        
                        pathnumbertomove = (int)entry.Key;
                        currentlowestpoint = amounttoconsider;
                    }
                }

                //now check if this change in depth in one of the markets pushes us past profitability
                Value startingamount = new Value();
                startingamount.amount = 1;
                startingamount.currency = Convert.ToInt32(dtnodes.Rows.Find(startingnode)["currency"]);

                Value currentamount = new Value();
                currentamount = startingamount;
                currentamount.isvoid = false;
                //Check at this depth is it profitable?
                foreach (DictionaryEntry entry in hscumulativedepths)
                {
                    Depth dp = (Depth)entry.Value;
                    Market currentmarket = GetMarketInfo(dtmarkets, dtnodes, dtmarketdata, dp.marketkey, dtmarketorderbooks, dp.orderbookdepth);
                    currentamount = CalculateTransaction(currentamount, currentmarket, false);
                }

                if (currentamount.amount - 1 > percentagethreashold)
                {
                    Depth currentdepthtoupdate = (Depth)hscumulativedepths[pathnumbertomove];
                    try
                    {
                        DataRow[] result = dtmarketorderbooks.Select("market_key = " + currentdepthtoupdate.marketkey.ToString() + " AND depth = " + currentdepthtoupdate.orderbookdepth);
                        currentdepthtoupdate.orderbookdepth += 1;
                        currentdepthtoupdate.cumulativedepthusd += currentdepthtoupdate.currentdepthamountusd;
                        currentdepthtoupdate.currentdepthamountusd = Convert.ToDecimal(result[0]["volumeusd"]);
                        currentdepthtoupdate.cumulativeprofitUSD += ((currentamount.amount - 1) * currentdepthtoupdate.currentdepthamountusd);
                        lastpathtomove = pathnumbertomove;
                    }
                    catch(Exception e)
                    {
                        //it's not profitable anymore, so roll back last market you added depth to, and calculate the cumulative depth amount in usd                        
                        minvolumeforalldepths = currentdepthtoupdate.cumulativedepthusd;
                        minprofitforalldepths = currentdepthtoupdate.cumulativeprofitUSD;

                        // Get stack trace for the exception with source file information
                        var st = new System.Diagnostics.StackTrace(e, true);
                        // Get the top stack frame
                        var frame = st.GetFrame(0);
                        // Get the line number from the stack frame
                        var line = frame.GetFileLineNumber();

                        Bitcoin_Notify_DB.SPs.UpdateError("Market Key: " + currentdepthtoupdate.marketkey.ToString() + "/ Depth: " + currentdepthtoupdate.orderbookdepth + "/ " + e.Message, line, frame.GetFileName()).Execute();

                        break;
                    }
                    
                }
                else
                {
                    //it's not profitable anymore, so roll back last market you added depth to, and calculate the cumulative depth amount in usd
                    Depth currentdepthtoupdate = (Depth)hscumulativedepths[pathnumbertomove];
                    minvolumeforalldepths = currentdepthtoupdate.cumulativedepthusd;
                    minprofitforalldepths = currentdepthtoupdate.cumulativeprofitUSD;
                    break;
                }

                /*
                //now this dp is the lowest depth, so add another depth layer to this market
                Depth currentdepthtoupdate = (Depth)hscumulativedepths[pathnumbertomove];
                currentdepthtoupdate.orderbookdepth += 1;
                decimal nextdepthlevelamount = 0;
                DataRow[] result = dtmarketorderbooks.Select("market_key = " + currentdepthtoupdate.marketkey.ToString() + " AND depth = " + currentdepthtoupdate.orderbookdepth);
                try
                {
                    nextdepthlevelamount = Convert.ToDecimal(result[0]["volumeusd"]);
                }
                catch
                {
                    minvolumeforalldepths = currentlowestpoint.cumulativedepthusd;
                    minprofitforalldepths = currentlowestpoint.cumulativeprofitUSD;
                    break;
                }
                currentdepthtoupdate.cumulativedepthusd += nextdepthlevelamount;

                //calculating profit
                decimal profitatthisdepth = (currentamount.amount * nextdepthlevelamount) - nextdepthlevelamount;
                currentdepthtoupdate.cumulativeprofitUSD += profitatthisdepth;
                */


                //if it's above 1, then rinse & repeat

            }

            pathdepthinfo.minvolumeforalldepthsUSD = minvolumeforalldepths;
            pathdepthinfo.profitforalldepthsUSD = minprofitforalldepths;

            return pathdepthinfo;
        }

        /*
        private decimal GetArbitrageDepth(List<int> path, DataTable dtmarketdata, DataTable dtmarkets, DataTable dtnodes, Models.Path currentpath, DataTable dtmarketorderbooks)
        {
            List<int> pathwithdynamicmarkets = new List<int>();
            //filter out markets that have unlimited volume and are not the same currency
            foreach(int marketkey in path)
            {
                Market currentmarket = GetMarketInfo(dtmarkets, dtnodes, dtmarketdata, marketkey,);
                Boolean isvalidmarket = true;
                if (currentmarket.source.currency == currentmarket.destination.currency)
                {
                    isvalidmarket = false;
                }
                if ((currentmarket.source.isfiat) && (currentmarket.destination.isfiat))
                {
                    isvalidmarket = false;
                }

                if (isvalidmarket)
                {
                    pathwithdynamicmarkets.Add(marketkey);
                }
            }

            decimal depthamountusd = 0;
            Boolean differenceabovezero = true;

            while(differenceabovezero)
            {
                Value startingamount = new Value();
                startingamount.amount = 1;
                startingamount.currency = 5; //USD Starting currency



            }

            

            return depthamountusd;
        }*/


            private Models.Path GetArbitrageRate(List<int> path, DataTable dtmarketdata, DataTable dtmarkets, DataTable dtnodes, Models.Path currentpath, DataTable dtmarketorderbooks)
        {
            MarketDifference arbitragerate = new MarketDifference();
            arbitragerate.isvoid = false;

            //set first buy data
            int startingmarket = path[0];                        
            int startingnode = Convert.ToInt32(dtmarkets.Rows.Find(startingmarket)["source"]);
            
            Value startingamount = new Value();
            startingamount.amount = 1;            
            startingamount.currency = Convert.ToInt32(dtnodes.Rows.Find(startingnode)["currency"]);

            Value currentamount = new Value();
            currentamount = startingamount;
            currentamount.isvoid = false;

            int startingmarketopposite = Convert.ToInt32(dtmarkets.Rows.Find(startingmarket)["market_key_opposite"]);            
            Value currentamount_asmaker = new Value();
            currentamount_asmaker = new Value();
            currentamount_asmaker.amount = 1;
            currentamount_asmaker.currency = startingamount.currency;
            currentamount_asmaker.isvoid = false;

            int market_key_firstmarket_opposite = 0;

            //calculate percentage rate
            int counter = 0;
            foreach (int marketkey in path)
            {
                try
                { 
                    Market currentmarket = GetMarketInfo(dtmarkets, dtnodes, dtmarketdata, marketkey, dtmarketorderbooks, 1);
                    currentamount = CalculateTransaction(currentamount, currentmarket, false);

                    if (counter == 0)
                    {
                        //first market, calculate marketopposite
                        Market currentmarket_opposite = GetMarketInfo(dtmarkets, dtnodes, dtmarketdata, startingmarketopposite, dtmarketorderbooks, 1);
                        currentmarket_opposite.price = 1 / currentmarket_opposite.price;
                        currentamount_asmaker = CalculateTransaction(currentamount_asmaker, currentmarket_opposite, false);
                        market_key_firstmarket_opposite = currentmarket_opposite.market_key;
                        //currentamount_asmaker.amount = 1 / currentamount_asmaker.amount ;
                    }
                    else
                    {
                        currentamount_asmaker = CalculateTransaction(currentamount_asmaker, currentmarket, false);
                    }

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

                    counter++;
                }
                catch (Exception e)
                {
                    // Get stack trace for the exception with source file information
                    var st = new System.Diagnostics.StackTrace(e, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();

                    Bitcoin_Notify_DB.SPs.UpdateError("Path Key: " + currentpath.pathkey + "/ " + e.Message, line, frame.GetFileName()).Execute();
                }
            }
            
            

            arbitragerate.percentage = (currentamount.amount - 1) * 100;
            arbitragerate.isvoid = currentamount.isvoid;
            arbitragerate.percentage_asmaker = (currentamount_asmaker.amount - 1) * 100;

            
            if (arbitragerate.percentage > percentagethreashold)
            {
                try
                {
                    PathDepthInfo tempdepthinfo = GetMinVolumeAlongPath(path, dtmarketdata, dtmarkets, dtnodes, dtmarketorderbooks, false);
                    arbitragerate.depthusd = tempdepthinfo.minvolumeforalldepthsUSD;
                    arbitragerate.depthprofit = tempdepthinfo.profitforalldepthsUSD;
                }
                catch (Exception e)
                {
                    // Get stack trace for the exception with source file information
                    var st = new System.Diagnostics.StackTrace(e, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();

                    Bitcoin_Notify_DB.SPs.UpdateError("Path Key: " +  currentpath.pathkey + "/ " + e.Message, line, frame.GetFileName()).Execute();
                }
            }
            if (arbitragerate.percentage_asmaker > percentagethreashold)
            {
                PathDepthInfo tempdepthinfo = GetMinVolumeAlongPath(path, dtmarketdata, dtmarkets, dtnodes, dtmarketorderbooks, true);
                arbitragerate.depthusd_asmaker = tempdepthinfo.minvolumeforalldepthsUSD;
                arbitragerate.depthprofit_asmaker = tempdepthinfo.profitforalldepthsUSD;
            }
            

            currentpath.marketdifference = arbitragerate;

            return currentpath;
        }


        private Value CalculateTransaction(Value currentamount, Market marketinfo, bool includefeestatic)
        {
            Value outputamount = currentamount;
            decimal percentagecalculate = 1 - marketinfo.fee_percentage;            
            bool doinverse = false;

            /*if ((marketinfo.source.currency == 1) && (marketinfo.destination.currency != 1)){
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
            }*/
            

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

        public Market GetMarketInfo(DataTable dtmarkets, DataTable dtnodes, DataTable dtmarketdata, int marketkey, DataTable dtmarketorderbooks, int depth)
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
                if (depth ==1)
                {
                    tempmarket.price = Convert.ToDecimal(dtmarketdata.Rows.Find(marketkey)["price"]);
                }
                else
                {
                    //not at surface price
                    DataRow[] result = dtmarketorderbooks.Select("market_key = " + marketkey.ToString() + " AND depth = " + depth.ToString());
                    try
                    {
                        tempmarket.price = Convert.ToDecimal(result[0]["price"]);
                    }
                    catch
                    {
                        //there was no result
                        tempmarket.price = 1;
                    }
                }
            }
            else
            {
                tempmarket.price = 1;
            }

            tempmarket.volume = 0;
            if (tempmarket.ratechanges)
            {
                
                    DataRow[] result = dtmarketorderbooks.Select("market_key = " + marketkey.ToString() + " AND depth = " + depth.ToString());
                    try
                    {
                        tempmarket.volume = Convert.ToDecimal(result[0]["volumeusd"]);
                    }
                    catch
                    {
                        //there was no result
                        tempmarket.volume = 0;
                    }
                
                
            }
            
            
                

            return tempmarket;

        }

        /*public void Update_Paths(DataTable dtmarketdata, DataTable dtmarkets, DataTable dtnodes)
        {
            //custom paths shown in db
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewPathsAll().GetDataSet().Tables[0];
            if (dttemp.Rows.Count > 0)
            {
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

                        currentpath.marketdifference = GetArbitrageRate(currentpath.path, dtmarketdata, dtmarkets, dtnodes);

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
            
        }*/

        protected Models.Path ResetPath()
        {
            Models.Path currentpath = new Models.Path();
            currentpath.pathkey = 0;
            currentpath.label = "";
            currentpath.path = new List<int>();

            return currentpath;
        }

        protected Models.Path InsertPath(Models.Path currentpath, int exchange1, int exchange2, int currency1, int currency2, int pathnumber)
        {
            int newmarketkey = -1;
            DataTable dtcurrentmarket = Bitcoin_Notify_DB.SPs.ViewMarketByExchangeSourceDestination(exchange1, exchange2, currency1, currency2).GetDataSet().Tables[0]; 
            if (dtcurrentmarket.Rows.Count > 0)
            {
                newmarketkey = Convert.ToInt32(dtcurrentmarket.Rows[0]["market_key"]);
            }
            
            if (currentpath.path.Count < pathnumber)
            {
                currentpath.path.Add(newmarketkey);
            }
            else
            {
                currentpath.path[pathnumber - 1] = newmarketkey;
                //clear all other paths above it                
                for (int i = currentpath.path.Count - 1; i > pathnumber -1 ; i--)
                {
                    currentpath.path.RemoveAt(i);
                }
            }
            
            return currentpath;
        }


        protected List<Models.Path> CreateThePath(DataTable dtmarkets, DataTable dtnodes, List<Models.Path> allthepaths, Models.Path currentpath)
        {
            Models.Path currentpath2 = ResetPath();
            //currentpath2.path = currentpath.path;
            currentpath2.path = new List<int>(currentpath.path);

            currentpath2.label = "";
            int lastmarketkey = 0;
            currentpath2.pathkey = allthepaths.Count + 1;

            //check to make sure each market has marketdata
            foreach (int marketkey in currentpath2.path)
            {
                currentpath2.label += dtmarkets.Rows.Find(marketkey)["Source_Shortname"].ToString() + "_" + dtmarkets.Rows.Find(marketkey)["Source_Currency"].ToString() + "->";
                lastmarketkey = marketkey;

                //add path info into db
                Bitcoin_Notify_DB.SPs.UpdatePathRoute(0, currentpath2.pathkey, marketkey).Execute();
            }
            currentpath2.label = currentpath2.label.Substring(0, currentpath2.label.Length - 2);

            if (currentpath2.label.Length > 3)
            {
                currentpath2.label = currentpath2.label.Substring(0, currentpath2.label.Length);
            }

            
            allthepaths.Add(currentpath2);

            //add path info into db
            Bitcoin_Notify_DB.SPs.UpdatePath(currentpath2.pathkey, currentpath2.pathkey, currentpath2.label).Execute();

            return allthepaths;
        }

        /*protected List<Models.Path> UpdateAllPaths(DataTable dtmarketdata, DataTable dtmarkets, DataTable dtnodes, List<Models.Path> allthepaths, Models.Path currentpath)
        {
            Models.Path currentpath2 = ResetPath();
            //currentpath2.path = currentpath.path;
            currentpath2.path = new List<int>(currentpath.path);

            Boolean candopairing = true;
            currentpath2.label = "";
            int lastmarketkey = 0;
            //check to make sure each market has marketdata
            foreach (int marketkey in currentpath2.path)
            {
                int currency1 = Convert.ToInt32(dtmarkets.Rows.Find(marketkey)["source_currency_key"]);
                int currency2 = Convert.ToInt32(dtmarkets.Rows.Find(marketkey)["destination_currency_key"]);
                string exch1 = dtmarkets.Rows.Find(marketkey)["source_exchange"].ToString();
                string exch2 = dtmarkets.Rows.Find(marketkey)["destination_exchange"].ToString();
                if ((exch1 == exch2) && (!dtmarketdata.Rows.Contains(marketkey)))
                {
                    candopairing = false;
                }

                currentpath2.label += dtmarkets.Rows.Find(marketkey)["Source_Shortname"].ToString() + "_" + dtmarkets.Rows.Find(marketkey)["Source_Currency"].ToString() + "->";
                lastmarketkey = marketkey;
            }
            currentpath2.label = currentpath2.label.Substring(0, currentpath2.label.Length - 2);

            //if still valid
            if (candopairing)
            {
                if (currentpath2.label.Length > 3)
                {
                    currentpath2.label = currentpath2.label.Substring(0, currentpath2.label.Length);
                }

                currentpath2.marketdifference = GetArbitrageRate(currentpath2.path, dtmarketdata, dtmarkets, dtnodes);
                currentpath2.volume = GetMinVolumeAlongPath(currentpath2.path, dtmarketdata, dtmarkets, dtnodes);

                if (currentpath2.marketdifference.isvoid == false)
                {
                    
                    currentpath2.pathkey = allthepaths.Count + 1;                    
                    allthepaths.Add(currentpath2);

                    int startingnode = currentpath2.path[0];
                    int endnode = currentpath2.path[currentpath.path.Count - 1];
                    Bitcoin_Notify_DB.SPs.UpdateArbResults(currentpath2.pathkey, startingnode, endnode, currentpath2.marketdifference.percentage, currentpath2.marketdifference.time, currentpath2.label).Execute();            
                }
            }

            return allthepaths;
        }*/




        public void Create_Paths_All(DataTable dtmarkets, Hashtable hsnodes, DataTable dtnodes, List<Hashtable> lshsnodesbyexchange)
        {
            List<Models.Path> allthepaths = new List<Models.Path>();

            foreach (DictionaryEntry entry1 in hsnodes)
            {
                Models.MarketNode node1 = (Models.MarketNode)entry1.Value;

                //looping through all exchange nodes.  Only start from nodes that are fiat
                if (node1.isfiat) //then proceed , this is the first node
                {
                    foreach (DictionaryEntry entry2 in lshsnodesbyexchange[node1.exchange_key - 1])
                    {
                        Models.MarketNode node2 = (Models.MarketNode)entry2.Value;
                        //looking for node #2
                        if ((node1.exchange_key == node2.exchange_key) && (!node2.isfiat))
                        {
                            //if same exchange & digital currency
                            //path #1 is legit, so start process

                            Models.Path currentpath = ResetPath();
                            currentpath = InsertPath(currentpath, node1.exchange_key, node2.exchange_key, node1.currency, node2.currency, 1);
                            if (currentpath.path[currentpath.path.Count - 1] == -1)
                            {
                                continue;
                            }


                            //Path #1 - starts from node1 to node2

                            //now look for node #3
                            foreach (DictionaryEntry entry3 in hsnodes)
                            {
                                Models.MarketNode node3 = (Models.MarketNode)entry3.Value;

                                if ((node2.currency == node3.currency) || ((!node3.isfiat) && (node2.exchange_key == node3.exchange_key)))
                                {
                                    //have to be same digital currency here

                                    if ((node2.exchange_currency_key == node3.exchange_currency_key) && (node2.currency == node3.currency))
                                    {
                                        //if it's the same node, just look for closing fiat node in same exchange
                                        foreach (DictionaryEntry entry4 in lshsnodesbyexchange[node3.exchange_key - 1])
                                        {
                                            Models.MarketNode node4 = (Models.MarketNode)entry4.Value;
                                            if ((node4.isfiat) && (node3.exchange_key == node4.exchange_key) && (node1.currency != node4.currency))
                                            {
                                                //it's just 2 markets inside same exchange

                                                //Path #2 - node2 to node4
                                                currentpath = InsertPath(currentpath, node2.exchange_key, node3.exchange_key, node2.currency, node4.currency, 2);
                                                if (currentpath.path[currentpath.path.Count - 1] == -1)
                                                {
                                                    continue;
                                                }

                                                //Path #3 - node4 to node1
                                                currentpath = InsertPath(currentpath, node4.exchange_key, node1.exchange_key, node4.currency, node1.currency, 3);
                                                if (currentpath.path[currentpath.path.Count - 1] == -1)
                                                {
                                                    continue;
                                                }

                                                allthepaths = CreateThePath(dtmarkets, dtnodes, allthepaths, currentpath);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Path #2 - node2 to node3
                                        currentpath = InsertPath(currentpath, node2.exchange_key, node3.exchange_key, node2.currency, node3.currency, 2);
                                        if (currentpath.path[currentpath.path.Count - 1] == -1)
                                        {
                                            continue;
                                        }

                                        //now look for node #4
                                        foreach (DictionaryEntry entry4 in lshsnodesbyexchange[node3.exchange_key - 1])
                                        {
                                            Models.MarketNode node4 = (Models.MarketNode)entry4.Value;

                                            if (node3.currency != node4.currency)
                                            {
                                                //Path #3 - node3 to node4
                                                currentpath = InsertPath(currentpath, node3.exchange_key, node4.exchange_key, node3.currency, node4.currency, 3);
                                                if (currentpath.path[currentpath.path.Count - 1] == -1)
                                                {
                                                    continue;
                                                }

                                                if (node4.isfiat)
                                                {
                                                    //Path #4 - node4 to node1
                                                    currentpath = InsertPath(currentpath, node4.exchange_key, node1.exchange_key, node4.currency, node1.currency, 4);
                                                    if (currentpath.path[currentpath.path.Count - 1] == -1)
                                                    {
                                                        continue;
                                                    }

                                                    allthepaths = CreateThePath(dtmarkets, dtnodes, allthepaths, currentpath);
                                                }
                                               /* else//it's coinbase, eg btc --> eth, node 4 is digital currency, node 5 has to be same currency
                                                {
                                                    //now look for node #4
                                                    foreach (DictionaryEntry entry5 in hsnodes)
                                                    {
                                                        Models.MarketNode node5 = (Models.MarketNode)entry5.Value;

                                                        if ((node4.currency == node5.currency) && (node4.exchange_currency_key != node5.exchange_currency_key))
                                                        {
                                                            //Path #4 - node4 to node5
                                                            currentpath = InsertPath(currentpath, node4.exchange_key, node5.exchange_key, node4.currency, node5.currency, 4);
                                                            if (currentpath.path[currentpath.path.Count - 1] == -1)
                                                            {
                                                                continue;
                                                            }

                                                            //now find node 6 & complete trip, will be in same exchange as node 5
                                                            foreach (DictionaryEntry entry6 in lshsnodesbyexchange[node5.exchange_key - 1])
                                                            {
                                                                Models.MarketNode node6 = (Models.MarketNode)entry6.Value;
                                                                if (node6.isfiat)
                                                                {
                                                                    //Path #5 - node5 to node6
                                                                    currentpath = InsertPath(currentpath, node5.exchange_key, node6.exchange_key, node5.currency, node6.currency, 5);
                                                                    if (currentpath.path[currentpath.path.Count - 1] == -1)
                                                                    {
                                                                        continue;
                                                                    }

                                                                    //Path #6 - node6 to node1
                                                                    currentpath = InsertPath(currentpath, node6.exchange_key, node1.exchange_key, node6.currency, node1.currency, 6);
                                                                    if (currentpath.path[currentpath.path.Count - 1] == -1)
                                                                    {
                                                                        continue;
                                                                    }

                                                                    allthepaths = CreateThePath(dtmarkets, dtnodes, allthepaths, currentpath);
                                                                }
                                                            }

                                                        }
                                                    }
                                                }*/

                                            }


                                        }
                                    }
                                }



                            }


                        }
                    }

                }
            }
        }


        private void CallbackMethodArbitrageRate(IAsyncResult ar)
        {
            // Retrieve the delegate.
            System.Runtime.Remoting.Messaging.AsyncResult result = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
            AsyncMethodCaller caller = (AsyncMethodCaller)result.AsyncDelegate;

            Models.Path currentpath = caller.EndInvoke(result);

            Bitcoin_Notify_DB.SPs.UpdateArbResults(currentpath.pathkey, currentpath.path[0], currentpath.path[currentpath.path.Count - 1], currentpath.marketdifference.percentage, currentpath.marketdifference.time, currentpath.label, currentpath.pathkey * -1, currentpath.marketdifference.percentage_asmaker, currentpath.path.Count, currentpath.marketdifference.depthusd, currentpath.marketdifference.depthusd_asmaker, currentpath.marketdifference.depthprofit, currentpath.marketdifference.depthprofit_asmaker).Execute();
            
        }
        
        public List<Models.Path> Update_Paths_All(DataTable dtmarketdata, DataTable dtmarkets, Hashtable hsnodes, DataTable dtnodes, List<Hashtable> lshsnodesbyexchange, DataTable dtmarketorderbooks)
        {
            DataTable dtsettings = Bitcoin_Notify_DB.SPs.ViewSettings().GetDataSet().Tables[0];
            percentagethreashold = (decimal)dtsettings.Rows[0]["percentage_threashold"];

            List<Models.Path> allthepaths = new List<Models.Path>();
            Models.Path currentpath = ResetPath();

            DataTable dtpathroutes = Bitcoin_Notify_DB.SPs.ViewPathRoutesAll().GetDataSet().Tables[0];

            foreach (DataRow dr in dtpathroutes.Rows)
            {
                //Loop through each path route, and organize 
                int currentpathroutekey = Convert.ToInt32(dr["path_route_key"]);
                int currentpathkey = Convert.ToInt32(dr["path_key"]);
                int currentmarketkey = Convert.ToInt32(dr["market_key"]);
                string pathname = dr["path_name"].ToString();

                if (currentpath.pathkey != currentpathkey)
                {
                    if (currentpath.pathkey > 0)
                    {
                        allthepaths.Add(currentpath);
                    }
                    
                    currentpath = ResetPath();
                    currentpath.pathkey = currentpathkey;
                    currentpath.label = pathname;
                }
                currentpath.path.Add(currentmarketkey);

            }

            
            foreach (var thecurrentpath in allthepaths)
            {


                //Seek_Paths_From_Node1(node1, dtmarketdata, dtmarkets, hsnodes, dtnodes, lshsnodesbyexchange);

                //if (thecurrentpath.pathkey == 11749)
                //{
                    AsyncMethodCaller caller = new AsyncMethodCaller(GetArbitrageRate);
                    IAsyncResult result = caller.BeginInvoke(thecurrentpath.path, dtmarketdata, dtmarkets, dtnodes, thecurrentpath, dtmarketorderbooks, new AsyncCallback(CallbackMethodArbitrageRate), null);
                    hsnumberofthreads.Add(thecurrentpath.pathkey, result);
                //}
                    

                

                if (hsnumberofthreads.Count ==64)
                {
                    int i = 0;
                    WaitHandle[] waitHandles = new WaitHandle[64];
                    foreach (DictionaryEntry entry in hsnumberofthreads)
                    {
                        IAsyncResult tempresult = (IAsyncResult)entry.Value;
                        waitHandles[i] = tempresult.AsyncWaitHandle;
                        i++;
                    }
                    WaitHandle.WaitAll(waitHandles);
                    hsnumberofthreads.Clear();
                }
            }



            //List<Models.Path> tempallpaths = new List<Models.Path>(allthepathsglobal);
            //fb.UpdateAllPaths(allthepaths);

            return allthepaths;

            /*DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewExchangesAll().GetDataSet().Tables[0];

            DataTable dttemppathkey = Bitcoin_Notify_DB.SPs.ViewPathsAll().GetDataSet().Tables[0];

            int pathkey = 0;
            if (dttemppathkey.Rows.Count > 0)
            {
                pathkey = Convert.ToInt32(dttemppathkey.Rows[dttemppathkey.Rows.Count - 1]["path_key"]);
            }


            foreach (DataRow dr in dttemp.Rows)  //looping through exchanges 1
            {
                int exchange1 = Convert.ToInt32(dr["exchange_key"]);

                List<int> hsexchangedigitalcurrencies = new List<int>();
                List<int> hsexchangefiatcurrencies = new List<int>();
                

                DataTable dtexchangecurrencies = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(exchange1).GetDataSet().Tables[0];
                foreach (DataRow drec in dtexchangecurrencies.Rows)
                {
                    int currency1 = Convert.ToInt32(drec["currency"]);

                    if (! Convert.ToBoolean( drec["isfiat"]))
                    {   
                        hsexchangedigitalcurrencies.Add(currency1);
                    }
                    else
                    {
                        hsexchangefiatcurrencies.Add(currency1);
                    }
                    
                }

                foreach (DataRow dr2 in dttemp.Rows) //looping through exchanges 2
                {
                    int exchange2 = Convert.ToInt32(dr2["exchange_key"]);

                        foreach (int currentdigitalcurrency in hsexchangedigitalcurrencies)
                        {
                            //go through all currency pairs exchange1 has

                            //check if exchange 2 has both currencies
                            DataTable dtexchangecurrency2 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(exchange2).GetDataSet().Tables[0];

                            DataColumn[] keyColumns1 = new DataColumn[1];
                            keyColumns1[0] = dtexchangecurrency2.Columns["currency"];
                            dtexchangecurrency2.PrimaryKey = keyColumns1;

                            bool candopairing = false;
                            if (dtexchangecurrency2.Rows.Contains(currentdigitalcurrency))  //exchange 2 has the same digital currency
                            {
                                candopairing = true;
                            }


                            if (candopairing) //can do currency pairing
                            {
                                //get all fiat currencies for exchange #2
                                List<int> hsexchangefiatcurrencies2 = new List<int>();
                                DataTable dtexchangecurrencies2 = Bitcoin_Notify_DB.SPs.ViewExchangeCurrenciesSpecificExchange(exchange2).GetDataSet().Tables[0];
                                foreach (DataRow drec in dtexchangecurrencies2.Rows)
                                {
                                    int currency1 = Convert.ToInt32(drec["currency"]);

                                    if (Convert.ToBoolean( drec["isfiat"]))
                                    {   
                                        hsexchangefiatcurrencies2.Add(currency1);
                                    }
                                }

                                //DataTable dttemppath = Bitcoin_Notify_DB.SPs.ViewPathByExchangesAndCurrencies(exchange1, exchange2, currencypair.Item1, currencypair.Item2).GetDataSet().Tables[0];

                                

                                foreach (int currentfiatcurrency in hsexchangefiatcurrencies)
                                {
                                    Models.Path currentpath = new Models.Path();
                                    currentpath.pathkey = 0;
                                    currentpath.label = "";
                                    currentpath.path = new List<int>();

                                    DataTable dtcurrentmarket = Bitcoin_Notify_DB.SPs.ViewMarketByExchangeSourceDestination(exchange1, exchange1,currentfiatcurrency, currentdigitalcurrency).GetDataSet().Tables[0];
                                    
                                    if (dtcurrentmarket.Rows.Count == 0)
                                    {
                                        break;
                                    }

                                    //Path #1 - starts from fiat to crypto
                                    currentpath.path.Add(Convert.ToInt32(dtcurrentmarket.Rows[0]["market_key"]));
                                    currentpath.label = dtcurrentmarket.Rows[0]["Source_Shortname"].ToString() + "_" + dtcurrentmarket.Rows[0]["Source_Currency"].ToString() + "->";

                                    //Path #2 - crypto in exchange 1 to crypto in exchange 2
                                    if (exchange1 != exchange2)
                                    {
                                        //skip this path, because it's going to the same node
                                        dtcurrentmarket = Bitcoin_Notify_DB.SPs.ViewMarketByExchangeSourceDestination(exchange1, exchange2, currentdigitalcurrency, currentdigitalcurrency).GetDataSet().Tables[0];
                                        currentpath.path.Add(Convert.ToInt32(dtcurrentmarket.Rows[0]["market_key"]));
                                        currentpath.label += dtcurrentmarket.Rows[0]["Source_Shortname"].ToString() + "_" + dtcurrentmarket.Rows[0]["Source_Currency"].ToString() + "->";
                                    }
                                    

                                    //Loop through this, as many different fiats for some exchanges
                                    foreach (int currentfiatcurrency2 in hsexchangefiatcurrencies2)
                                    {

                                        if ((exchange1 == exchange2) && (currentfiatcurrency == currentfiatcurrency2))
                                        {
                                            break; 
                                        }

                                        Models.Path currentpath2 = new Models.Path();
                                        currentpath2.pathkey = 0;
                                        currentpath2.label = currentpath.label;
                                        currentpath2.path = new List<int>();
                                        foreach (int marketkey in currentpath.path)
                                        {
                                            currentpath2.path.Add(marketkey);
                                        }

                                        //Path #3 - crypto in exchange 2 to fiat in exchange 2 - 
                                        dtcurrentmarket = Bitcoin_Notify_DB.SPs.ViewMarketByExchangeSourceDestination(exchange2, exchange2, currentdigitalcurrency, currentfiatcurrency2).GetDataSet().Tables[0];
                                        if (dtcurrentmarket.Rows.Count == 0)
                                        {                                       
                                            break;
                                        }
                                        currentpath2.path.Add(Convert.ToInt32(dtcurrentmarket.Rows[0]["market_key"]));
                                        currentpath2.label += dtcurrentmarket.Rows[0]["Source_Shortname"].ToString() + "_" + dtcurrentmarket.Rows[0]["Source_Currency"].ToString() + "->";

                                        //Path #4 - fiat in exchange 2 to fiat in exchange 1 
                                        dtcurrentmarket = Bitcoin_Notify_DB.SPs.ViewMarketByExchangeSourceDestination(exchange2, exchange1, currentfiatcurrency2, currentfiatcurrency).GetDataSet().Tables[0];
                                        if (dtcurrentmarket.Rows.Count == 0)
                                        {
                                            break;
                                        }
                                        currentpath2.path.Add(Convert.ToInt32(dtcurrentmarket.Rows[0]["market_key"]));
                                        currentpath2.label += dtcurrentmarket.Rows[0]["Source_Shortname"].ToString() + "_" + dtcurrentmarket.Rows[0]["Source_Currency"].ToString();

                                        //check to make sure each market has marketdata
                                        foreach (int marketkey in currentpath2.path)
                                        {
                                            int currency1 = Convert.ToInt32(dtmarkets.Rows.Find(marketkey)["source_currency_key"]);
                                            int currency2 = Convert.ToInt32(dtmarkets.Rows.Find(marketkey)["destination_currency_key"]);
                                            string exch1 = dtmarkets.Rows.Find(marketkey)["source_exchange"].ToString();
                                            string exch2 = dtmarkets.Rows.Find(marketkey)["destination_exchange"].ToString();
                                            if ((exch1 == exch2) && (!dtmarketdata.Rows.Contains(marketkey)))
                                            {
                                                candopairing = false;
                                            }

                                        }

                                        

                                        //if still valid
                                        if (candopairing)
                                        {
                                            if (currentpath2.label.Length > 3)
                                            {
                                                currentpath2.label = currentpath2.label.Substring(0, currentpath2.label.Length);
                                            }

                                            currentpath2.marketdifference = GetArbitrageRate(currentpath2.path, dtmarketdata, dtmarkets, dtnodes);
                                            currentpath2.volume = GetMinVolumeAlongPath(currentpath2.path, dtmarketdata, dtmarkets, dtnodes);

                                            if (currentpath2.marketdifference.isvoid == false)
                                            {                                                

                                                pathkey += 1;
                                                currentpath2.pathkey = pathkey;
                                                //fb.UpdatePaths(currentpath2);                    
                                                allthepaths.Add(currentpath2);
                                            }
                                        }

                                    }

                                }
                                    

                                    

                                


                            }

                        }
                    

                }

            }*/

            

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

            //MarketDifference therate = GetArbitrageRate(path);
            

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

            //MarketDifference therate = GetArbitrageRate(path);
            

        }

    }
}