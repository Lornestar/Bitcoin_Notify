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
                GridSortExpression expression = new GridSortExpression();
                expression.FieldName = "percentage";
                expression.SortOrder = GridSortOrder.Descending;
                RadGrid1.MasterTableView.SortExpressions.AddSortExpression(expression);
            }
        }

        protected void RadListView1_NeedDataSource(object source, RadListViewNeedDataSourceEventArgs e)
        { 
            /*APIs.Firebase fb = new APIs.Firebase();
            DataTable dttemp = fb.GetData(0);


            RadListView1.DataSource = dttemp;

            RadGrid1.DataSource = dttemp;

            //set javascript updates
            //checkthefirebase(1);
            string thescript = "";
            foreach (DataRow dr in dttemp.Rows)
            {
                thescript += "checkthefirebase("+ dr["pathkey"].ToString() +");";
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myKey", thescript, true);*/
        }

        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewArbResults().GetDataSet().Tables[0];
            RadGrid1.DataSource = dttemp;

            string thescript = "";
            foreach (DataRow dr in dttemp.Rows)
            {
                thescript += "checkthefirebase(" + dr["arb_results_key"].ToString() + ");";
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myKey", thescript, true);
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {

        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView row = (DataRowView)e.Item.DataItem;
                int pathkey = Convert.ToInt32(row["arb_results_key"]);
                item["percentage"].Text = "<span id=" + pathkey + "_percentage>" + item["percentage"].Text + "</span>";
                item["label"].Text = "<span id=" + pathkey + "_label>" + item["label"].Text + "</span>";
                item["last_changed"].Text = "<span id=" + pathkey + "_updated>" + item["last_changed"].Text + "</span>";
            }
        }
    }
}
