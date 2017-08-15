using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Bitcoin_Analyze.APIs
{
    public class Firebase
    {
        char chr34 = Convert.ToChar(34);

        public DataTable GetData(int type)
        {
            DataTable dttemp = new DataTable();


            string strresponse = Web_Request("https://lornestar.firebaseio.com/arbitrage/.json");
            if (type != 1)
            {
                JArray ja = JArray.Parse(strresponse);

                dttemp.Columns.Add("name", typeof(string));
                dttemp.Columns.Add("percentage", typeof(string));
                dttemp.Columns.Add("fee", typeof(string));
                dttemp.Columns.Add("time", typeof(int));
                dttemp.Columns.Add("pathkey", typeof(string));
                dttemp.Columns.Add("updated", typeof(string));
                dttemp.Columns.Add("starting", typeof(string));
                dttemp.Columns.Add("finish", typeof(string));

                foreach (var o in ja)
                {
                    try
                    {
                        JObject o2 = (JObject)o;

                        DataRow dr = dttemp.NewRow();
                        dr["name"] = (string)o2["name"];
                        dr["pathkey"] = (int)o2["pathkey"];
                        dr["updated"] = (string)o2["lastupdated"];

                        JObject o3 = (JObject)o2["marketdifference"];

                        dr["percentage"] = (decimal)o3["percentage"];

                        string strfee = "";
                        if (o3["feeUSD"] != null)
                        {
                            decimal currentfee = (decimal)o3["feeUSD"];
                            if (currentfee > 0)
                            {
                                strfee += currentfee.ToString() + "USD ";
                            }
                            currentfee = (decimal)o3["feeBTC"];
                            if (currentfee > 0)
                            {
                                strfee += currentfee.ToString() + "BTC ";
                            }
                            currentfee = (decimal)o3["feeLTC"];
                            if (currentfee > 0)
                            {
                                strfee += currentfee.ToString() + "LTC ";
                            }
                        }

                        dr["fee"] = strfee;
                        dr["time"] = (int)o3["time"];

                        //dr["starting"] = (decimal)o3["starting"];
                        //dr["finish"] = (decimal)o3["finish"];

                        dttemp.Rows.Add(dr);
                    }
                    catch
                    {
                    }
                }
            }
            if (type == 1)
            {
                strresponse = Web_Request("https://lornestar.firebaseio.com/rippletrade/.json") ;

                JObject ja = JObject.Parse(strresponse); 

                dttemp.Columns.Add("name", typeof(string));
                dttemp.Columns.Add("percentage", typeof(string));
                dttemp.Columns.Add("fee", typeof(string));
                dttemp.Columns.Add("time", typeof(int));
                dttemp.Columns.Add("pathkey", typeof(string));
                dttemp.Columns.Add("updated", typeof(string));
                dttemp.Columns.Add("starting", typeof(string));
                dttemp.Columns.Add("finish", typeof(string)); 

                DataTable dttemp2= RippleTrade_DB.SPs.ViewPathRoutesAll().GetDataSet().Tables[0];

                foreach (DataRow dr2 in dttemp2.Rows)
                {
                    try
                    {
                        int pathkey = Convert.ToInt32( dr2["path_key"]);
                        JObject o = (JObject)ja[pathkey.ToString()];

                        if (o!=null)
                        {
                            JObject o2 = (JObject)o;

                            DataRow dr = dttemp.NewRow();
                            dr["name"] = (string)o2["name"];
                            dr["pathkey"] = (int)o2["pathkey"];
                            dr["updated"] = (string)o2["lastupdated"];

                            JObject o3 = (JObject)o2["marketdifference"];

                            dr["percentage"] = (decimal)o3["percentage"];

                            string strfee = "";
                            if (o3["feeUSD"] != null)
                            {
                                decimal currentfee = (decimal)o3["feeUSD"];
                                if (currentfee > 0)
                                {
                                    strfee += currentfee.ToString() + "USD ";
                                }
                                currentfee = (decimal)o3["feeBTC"];
                                if (currentfee > 0)
                                {
                                    strfee += currentfee.ToString() + "BTC ";
                                }
                                currentfee = (decimal)o3["feeLTC"];
                                if (currentfee > 0)
                                {
                                    strfee += currentfee.ToString() + "LTC ";
                                }
                            }

                            dr["fee"] = strfee;
                            dr["time"] = (int)o3["time"];

                            dr["starting"] = (decimal)o3["starting"];
                            dr["finish"] = (decimal)o3["finish"];

                            dttemp.Rows.Add(dr);
                        }
                        
                    }
                    catch
                    {
                    }
                }
            }
            
            
            return dttemp;
        }


        private string Web_Request(string url)
        {

            HttpWebResponse webResponse;

            // Create request object
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";            

            string responseString = "";
            try
            {
                // Get the response from the request object and verify the status
                webResponse = webRequest.GetResponse() as HttpWebResponse;
                if (!webRequest.HaveResponse)
                {
                    throw new Exception();
                }
                if (webResponse.StatusCode != HttpStatusCode.OK && webResponse.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new Exception();
                }

                // Read the response string
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception e)
            {
            }

            return responseString;
        }
    }
}