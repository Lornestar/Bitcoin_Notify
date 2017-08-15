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

        public DataTable GetData()
        {
            DataTable dttemp = new DataTable();

            string strresponse = Web_Request("https://lornestar.firebaseio.com/.json");
            JArray ja = JArray.Parse(strresponse);

            dttemp.Columns.Add("name", typeof(string));
            dttemp.Columns.Add("percentage", typeof(string));
            dttemp.Columns.Add("fee", typeof(string));
            dttemp.Columns.Add("time", typeof(int));
            dttemp.Columns.Add("pathkey", typeof(string));
            dttemp.Columns.Add("updated", typeof(string));

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
                    dr["fee"] = strfee;
                    dr["time"] = (int)o3["time"];
                    

                    dttemp.Rows.Add(dr);
                }
                catch
                {
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