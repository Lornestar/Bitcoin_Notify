using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Hangfire;
using System.Configuration;
using System.Text;


namespace Bitcoin_Background
{
    public class Site
    {
        public string Web_Request(string url, Hashtable hstemp, int type, Hashtable header, string contenttype)
        {
            HttpWebResponse webResponse;
            string request = "";
            // Create request object
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "application/json";



            if (type == 0)
            {
                webRequest.Method = "GET";

                if (contenttype.Length > 0)
                {
                    webRequest.ContentType = contenttype;
                }

            }
            else if (type == 2) //PUT
            {
                webRequest.Method = "PUT";

                if (contenttype.Length > 0)
                {
                    webRequest.ContentType = contenttype;
                }

                //var data = Encoding.ASCII.GetBytes(hstemp["kycpic"].ToString());

                byte[] bytes = Convert.FromBase64String(hstemp["kycpic"].ToString());
                //request = ;
                // Write the request string to the request object
                /*StreamWriter writer = new StreamWriter(webRequest.GetRequestStream());
                writer.Write(data,0, data.Length);
                writer.Close();*/

                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            else
            {
                request = JsonConvert.SerializeObject(hstemp);

                if (contenttype.Length > 0)
                {
                    webRequest.ContentType = contenttype;
                    request = ConvertHashtabletoString(hstemp);
                }

                //request = "{\"address_city\":\"Calgary\",\"address_country\":\"CA\",\"address_postal_code\":\"T2P055\",\"address_state\":\"CA-AB\",\"address_street_1\":\"100 Twain Street\",\"address_street_2\":\"Suiste #300\",\"cell_phone_country\":\"1\",\"cell_phone\":\"6507840072\",\"ip\":\"1.1.1.1\",\"is_business\":\"false\",\"email\":\"tom@finn.com\",\"first_name\":\"Tom\",\"last_name\":\"Sawyer\",\"date_of_birth\":\"1950-01-01\"}";

                webRequest.Method = "POST";
                // Write the request string to the request object
                StreamWriter writer = new StreamWriter(webRequest.GetRequestStream());
                writer.Write(request);
                writer.Close();
            }

            if (header != null)
            {
                foreach (DictionaryEntry entry in header)
                {
                    webRequest.Headers.Add(entry.Key.ToString(), entry.Value.ToString());
                    //Console.WriteLine("{0}, {1}", entry.Key, entry.Value);
                }


            }

            webRequest.UserAgent = "InstantPadala";

            string responseString = "";
            try
            {

                // Get the response from the request object and verify the status
                webResponse = webRequest.GetResponse() as HttpWebResponse;
                if (!webRequest.HaveResponse)
                {
                    throw new Exception();
                }
                if ((webResponse.StatusCode != HttpStatusCode.OK) && (webResponse.StatusCode != HttpStatusCode.Accepted) && (webResponse.StatusCode != HttpStatusCode.Created))
                {
                    throw new Exception();
                }

                // Read the response string
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();


            }
            catch (WebException e)
            {
                string error = e.Message;
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                            //TODO: use JSON.net to parse this string and look at the error message
                        }
                    }
                }
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