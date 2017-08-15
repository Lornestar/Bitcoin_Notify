using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections;

namespace Bitcoin_Notify.APIs
{
    public class Firebase
    {
        char chr34 = Convert.ToChar(34);

        public void UpdateMarket(Hashtable hstemp)
        {
            
            string strurl = "https://lornestar.firebaseio.com/.json";

            Web_Request(strurl, hstemp, 0);

        }

        public void UpdatePaths(Models.Path thepath)
        {
            string strurl = "https://lornestar.firebaseio.com/.json";
            
            Hashtable hstemp = new Hashtable();
            hstemp.Add("percentage", Math.Round(thepath.marketdifference.percentage,2));
            hstemp.Add("feeBTC", thepath.marketdifference.staticfeeBTC);
            hstemp.Add("feeLTC", thepath.marketdifference.staticfeeLTC);
            hstemp.Add("feeUSD", thepath.marketdifference.staticfeeUSD);
            hstemp.Add("time", thepath.marketdifference.time);

            Hashtable hstemp2 = new Hashtable();
            hstemp2.Add("pathkey", thepath.pathkey);
            hstemp2.Add("name", thepath.label);
            hstemp2.Add("marketdifference", hstemp);
            hstemp2.Add("lastupdated", DateTime.UtcNow.ToString("hh:mm:ss"));

            Hashtable hstemp3 = new Hashtable();
            hstemp3.Add(thepath.pathkey, hstemp2);
            
            Web_Request(strurl, hstemp3, 0);
            
        }

        public void ClearAllData()
        {
            string strurl = "https://lornestar.firebaseio.com/.json";

            Web_Request(strurl, null, 1);
        }

        private string Web_Request(string url, Hashtable hstemp, int method)
        {

            HttpWebResponse webResponse;

            // Create request object
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "application/json";
            if (method == 0)
            {
                webRequest.Method = "PATCH";            
            }
            else
            {
                webRequest.Method = "DELETE";            
            }

            string jsontxt = "";
            if (hstemp != null)
            {
                jsontxt = JsonConvert.SerializeObject(hstemp);
            }            

            // Write the request string to the request object
            StreamWriter writer = new StreamWriter(webRequest.GetRequestStream());

            writer.Write(jsontxt);
            writer.Close();

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