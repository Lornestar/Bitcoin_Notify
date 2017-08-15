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

namespace Bitcoin_Notify.APIs
{
    public class CurrencyCloud
    {

        Site sitetemp = new Site();

        public string Authentication_New()
        {
            string strstatus = "error";
            Hashtable hstemp = new Hashtable();
            hstemp.Add("login_id", ConfigurationSettings.AppSettings["CurrencyCloud_login_id"]);
            hstemp.Add("api_key", getapikey());
            strstatus = Web_Request("/authentication/token/new", hstemp);

            JObject o = JObject.Parse(strstatus);
            string newtoken = (string)o["data"];

            return newtoken;
        }

        public Hashtable Exchange_Rate_ccy_pair(string currencypair)
        {
            Hashtable hstemp = new Hashtable();

            string strcurrencycodes = currencypair;
            string url = "/prices/market/" + strcurrencycodes + "?accept_stale=true";

            string responseString = CallWeb_Request(url, hstemp);

            hstemp = JsonConvert.DeserializeObject<Hashtable>(responseString);
            hstemp = JsonConvert.DeserializeObject<Hashtable>(hstemp["data"].ToString());

            return hstemp;
        }

        public decimal getexchangerate(string currencypair)
        {
            decimal dcrate = 0;
            Hashtable hstemp = Exchange_Rate_ccy_pair(currencypair);
            dcrate = Convert.ToDecimal(hstemp["market_price"]);

            return dcrate;
        }

        private string geturl()
        {
            string url = "";
            int playground = Convert.ToInt32(ConfigurationSettings.AppSettings["CurrencyCloud_Environment"]);
            switch (playground)
            {
                case 0: url = ConfigurationSettings.AppSettings["CurrencyCloud_api_url_reference"];
                    break;
                case 1: url = ConfigurationSettings.AppSettings["CurrencyCloud_api_url_demo"];
                    break;
                case 2: url = ConfigurationSettings.AppSettings["CurrencyCloud_api_url_live"];
                    break;
            }
            return url;
        }

        private string getapikey()
        {
            string key = "";
            int playground = Convert.ToInt32(ConfigurationSettings.AppSettings["CurrencyCloud_Environment"]);
            switch (playground)
            {
                case 0: key = ConfigurationSettings.AppSettings["CurrencyCloud_api_key_reference"];
                    break;
                case 1: key = ConfigurationSettings.AppSettings["CurrencyCloud_api_key_demo"];
                    break;
                case 2: key = ConfigurationSettings.AppSettings["CurrencyCloud_api_key_live"];
                    break;
            }
            return key;
        }


        private string Web_Request(string callurl, Hashtable hstemp)
        {
            string url = geturl();

            string token = view_info_currencycloud_token();
            if (callurl.Contains("authentication/token/new"))
            {
                url += "/api/en/v1.0";
            }
            else
            {
                url += "/api/en/v1.0/" + token;
            }
            url += callurl;

            HttpWebResponse webResponse;

            string request = sitetemp.ConvertHashtabletoString(hstemp);

            // Create request object
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.MediaType = "application/x-www-form-urlencoded";
            if (url.Contains("?"))
            {
                webRequest.Method = "GET";
            }
            else
            {
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


        //Get token for reading exchange rates
        private string view_info_currencycloud_token()
        {


            DataSet dstemp = Bitcoin_Notify_DB.SPs.ViewInfoCurrencyCloudTokens().GetDataSet();
            return dstemp.Tables[0].Rows[0]["CurrencyCloud_Token"].ToString();
        }

        //Renew token for reading exchange rates
        private string update_info_currencycloud_token()
        {
            string newtoken = Authentication_New();
            Bitcoin_Notify_DB.SPs.UpdateCurrencyCloudTokens(1, newtoken).Execute();
            return newtoken;
        }

        private string CallWeb_Request(string callurl, Hashtable hstemp)
        {
            string strreturn;
            strreturn = Web_Request(callurl, hstemp);
            if ((strreturn.Contains("error")) || (strreturn == ""))
            {
                string token = update_info_currencycloud_token();
                strreturn = Web_Request(callurl, hstemp);
            }
            return strreturn;
        }

    }
}