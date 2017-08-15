using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;

namespace Bitcoin_Notify
{
    public class Site
    {

        public string Web_Request(string url, Hashtable hstemp)
        {


            HttpWebResponse webResponse;

            // Create request object
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "application/json";
            if ((url.Contains("?")) || (hstemp == null))
            {
                webRequest.Method = "GET";
            }
            else
            {
                string request = JsonConvert.SerializeObject(hstemp);

                webRequest.Method = "POST";
                // Write the request string to the request object
                StreamWriter writer = new StreamWriter(webRequest.GetRequestStream());
                writer.Write(request);
                writer.Close();
            }

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
                //string Toemail = System.Configuration.ConfigurationSettings.AppSettings.Get("ErrorToEmail").ToString();
                //SendGrid sg = new SendGrid();
                //sg.SimpleEmail("Lorne", "Passportfx API Error", Toemail, "Error@passportfx.com", e.Message, "CurrencyCloud Error");                
            }

            return responseString;
        }

        public string ConvertHashtabletoString(Hashtable hstemp)
        {
            StringBuilder requestString = new StringBuilder();

            if (hstemp != null)
            {
                foreach (DictionaryEntry Item in hstemp)
                {
                    if (requestString.Length == 0)
                    {
                        requestString.Append(Item.Key + "=" + Item.Value);
                    }
                    else
                    {
                        requestString.Append("&" + Item.Key + "=" + Item.Value);
                    }
                }
            }

            return requestString.ToString();
        }
    }
}