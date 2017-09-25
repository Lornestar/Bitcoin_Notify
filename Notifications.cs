using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

namespace Bitcoin_Notify
{
    public class Notifications
    {

        public void CheckNotifications()
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewNotifications().GetDataSet().Tables[0];

            Hashtable hstemp = new Hashtable();

            foreach (DataRow dr in dttemp.Rows)
            {
                int notificationkey = Convert.ToInt32(dr["notification_key"]);
                int startingnode = Convert.ToInt32(dr["starting_node"]);
                decimal threashold = Convert.ToDecimal(dr["threashold"]);
                int threashold_direction = Convert.ToInt32(dr["threashold_direction"]);
                DateTime last_sent = DateTime.Now.AddYears(-1);
                if (dr["last_sent"] != DBNull.Value)
                {
                    last_sent = Convert.ToDateTime(dr["last_sent"]);
                }
                int sendwindow = Convert.ToInt32(dr["sendwindow"]);

                int currentpath = 1;
                DataTable dtarbresults = Bitcoin_Notify_DB.SPs.ViewArbResultsStartingnode(startingnode).GetDataSet().Tables[0];
                foreach (DataRow drarb in dtarbresults.Rows)
                {
                    
                    decimal percentage = Convert.ToDecimal(drarb["percentage"]);
                    if ((percentage >= threashold) && (last_sent.AddMinutes(Convert.ToDouble(sendwindow)) < DateTime.Now))
                    {
                        //add to hashtable
                        string arbresultlabel = drarb["label"].ToString() + " - " + drarb["percentage"].ToString();

                        hstemp.Add(currentpath, arbresultlabel);

                       currentpath =  currentpath+1;
                    }
                }

            }

            string thebody = "Arbitrage Bot - Notifications<br/>";
            foreach (DictionaryEntry entry in hstemp)
            {
                thebody += entry.Value + "<br/>";
                Bitcoin_Notify_DB.SPs.UpdateNotificationsLastSent(Convert.ToInt32( entry.Key)).Execute();
            }

            if (hstemp.Count > 0)
            {
                APIs.thesendgrid sg = new APIs.thesendgrid();
                sg.Send_Notification(thebody);
            }
            
        }

    }
}