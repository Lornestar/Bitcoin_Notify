using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendGrid.SmtpApi;
using SendGrid;
using System.Net;


namespace Bitcoin_Notify.APIs
{
    public class thesendgrid
    {

        Site sitetemp = new Site();

        public void SimpleEmail(string strTo, string strToEmail, string templateid, Hashtable hstemp, string subject)
        {
            try
            {
                // From
                string strFrom = "lorne@lornestar.com";
                string strFromName = "Arb bot";

                var myMessage = new SendGrid.SendGridMessage();
                myMessage.AddTo(strToEmail);
                myMessage.From = new MailAddress(strFrom, strFromName);
                myMessage.Subject = subject;
                myMessage.Text = " ";
                myMessage.Html = " ";

                foreach (DictionaryEntry entry in hstemp)
                {
                    List<string> templist = new List<string>();
                    templist.Add(entry.Value.ToString());
                    myMessage.AddSubstitution(entry.Key.ToString(), templist);
                }

                //Filters
                var filters = new Dictionary<string, dynamic>()
                {
                    {
                        "opentrack", new Dictionary<string, dynamic>()
                        {
                            {
                                "settings", new Dictionary<string, dynamic>()
                                {
                                    {
                                        "enable", 1
                                    }
                                }
                            }
                        }
                    },
                    {
                        "templates", new Dictionary<string, dynamic>()
                        {
                            {
                                "settings", new Dictionary<string, dynamic>()
                                {
                                    {
                                        "enable", 1
                                    },
                                    {
                                        "template_id", templateid
                                    }
                                }
                            }
                        }
                    }
                };
                foreach (var filter in filters.Keys)
                {
                    var settings = filters[filter]["settings"];
                    foreach (var setting in settings.Keys)
                    {
                        myMessage.Header.AddFilterSetting(filter, new List<string> { setting }, Convert.ToString(settings[setting]));
                    }
                }

                /* CREDENTIALS
                 * ===================================================*/
                string sgUsername = ConfigurationSettings.AppSettings["Sendgrid_username"];
                string sgPassword = ConfigurationSettings.AppSettings["Sendgrid_pwd"];

                /* SEND THE MESSAGE
                 * ===================================================*/
                var credentials2 = new NetworkCredential(sgUsername, sgPassword);
                // Create a Web transport for sending email.
                var transportWeb = new Web(credentials2);

                // Send the email.
                transportWeb.Deliver(myMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        protected string Get_Template_Info(string to, string templateid, Hashtable hssubs)
        {
            string strtemplate = "";

            string sub = "";
            foreach (DictionaryEntry entry in hssubs)
            {
                if (sub.Length > 0)
                {
                    sub += ",";
                }
                sub += "\":" + entry.Key + "\":[\"" + entry.Value + "\"]";
            }


            Hashtable hstemplates = new Hashtable();
            Hashtable hssettings = new Hashtable();

            hssettings.Add("enable", 1);
            hssettings.Add("template_id", templateid);
            hstemplates.Add("settings", hssettings);

            strtemplate = "{\"to\":[\"" + to + "\"],\"sub\":{" + sub + "},\"filters\":{\"templates\":" + JsonConvert.SerializeObject(hstemplates) + "}}";


            return strtemplate;

        }

        public void Send_Notification(string thebody)
        {
            Hashtable hstemp = new Hashtable();
            hstemp.Add("THEOPPORTUNITIES", thebody);
            SimpleEmail("Lorne", "lorne@lornestar.com", "e9a700f9-1623-4752-bce7-735a1c5153ee", hstemp, "Arb Bot Notification");

        }

    }
}