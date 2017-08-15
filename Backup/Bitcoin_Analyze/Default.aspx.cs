using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;

namespace Bitcoin_Analyze
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void RadListView1_NeedDataSource(object source, RadListViewNeedDataSourceEventArgs e)
        {
            APIs.Firebase fb = new APIs.Firebase();
            DataTable dttemp = fb.GetData();

            string v = Request.QueryString["altonly"];
            if (v == "true")
            {
                List<int> removallist = new List<int>();
                int theindex = 0;
                foreach (DataRow dr in dttemp.Rows)
                {
                    if (dr["name"].ToString().Contains("USD"))
                    {
                        removallist.Add(theindex);
                    }
                    theindex++;
                }

                DataTable dtpaths = Bitcoin_Notify_DB.SPs.ViewPathsAll().GetDataSet().Tables[0];

                for (int i = dttemp.Rows.Count - 1; i >= dtpaths.Rows.Count; i--)
                {
                    if (removallist.Contains(i))
                    {
                        dttemp.Rows.RemoveAt(i);
                    }
                }
            }

            RadListView1.DataSource = dttemp;

            //set javascript updates
            //checkthefirebase(1);
            string thescript = "";
            foreach (DataRow dr in dttemp.Rows)
            {
                thescript += "checkthefirebase("+ dr["pathkey"].ToString() +");";
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myKey", thescript, true);
        }
    }
}
