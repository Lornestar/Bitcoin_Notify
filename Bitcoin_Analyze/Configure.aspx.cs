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
    public partial class Configure : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewPathsAll().GetDataSet().Tables[0];
            RadGrid1.DataSource = dttemp;
        }

        protected void RadGrid2_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dttemp = Bitcoin_Notify_DB.SPs.ViewPathRouteSpecific(Convert.ToInt32(hdpathkey.Value)).GetDataSet().Tables[0];
            RadGrid2.DataSource = dttemp;
        }
        

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if ((e.CommandName == "PerformInsert") || (e.CommandName == "Update"))
            {                
                GridEditFormItem item = (GridEditFormItem)e.Item;
                int pathkey = 0;
                if (e.CommandName == "Update")
                {
                    pathkey = Convert.ToInt32(item.GetDataKeyValue("path_key"));
                }
                int order = Convert.ToInt32((item["page_order"].Controls[0] as TextBox).Text);

                Bitcoin_Notify_DB.SPs.UpdatePath(pathkey, order).Execute();

                RadGrid1.MasterTableView.ClearEditItems();
            }
            else if (e.CommandName == "Route")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int pathkey = Convert.ToInt32(item["path_key"].Text);
                string pathname = item["path_name"].Text;
                hdpathkey.Value = pathkey.ToString();
                lblpath.Text = pathkey.ToString() + " - " + pathname;
                RadGrid2.Rebind();
            }
            else if (e.CommandName == "Delete")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int pathkey = Convert.ToInt32(item.GetDataKeyValue("path_key"));

                Bitcoin_Notify_DB.SPs.DeletePath(pathkey).Execute();
                RadGrid1.MasterTableView.ClearEditItems();
            }
        }

        protected void RadGrid2_ItemCommand(object source, GridCommandEventArgs e)
        {
            if ((e.CommandName == "PerformInsert") || (e.CommandName == "Update"))
            {
                GridEditFormItem item = (GridEditFormItem)e.Item;
                int pathroutekey = 0;
                if (e.CommandName == "Update")
                {
                    pathroutekey = Convert.ToInt32(item.GetDataKeyValue("path_route_key"));
                }
                int pathkey = Convert.ToInt32(hdpathkey.Value);
                int marketkey = Convert.ToInt32((item["market_key2"].FindControl("ddlmarket") as RadComboBox).SelectedValue);

                Bitcoin_Notify_DB.SPs.UpdatePathRoute(pathroutekey, pathkey, marketkey).Execute();

                RadGrid2.MasterTableView.ClearEditItems();
                //string firstname = (item["first_name"].Controls[0] as TextBox).Text;
            }
            else if (e.CommandName == "Delete")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int pathroutekey = Convert.ToInt32(item.GetDataKeyValue("path_route_key"));

                Bitcoin_Notify_DB.SPs.DeletePathRoute(pathroutekey).Execute();

                RadGrid2.Rebind();
                //int userkey = Convert.ToInt32(item["user_key"].Text);
            }
        }

        protected void RadGrid2_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            
            if (e.Item is GridEditableItem && (e.Item as GridEditableItem).IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                RadComboBox ddlmarkets = editedItem.FindControl("ddlmarket") as RadComboBox;

                ddlmarkets.DataSource = Bitcoin_Notify_DB.SPs.ViewMarketsAll().GetDataSet().Tables[0];
                ddlmarkets.DataTextField = "market_shortname";
                ddlmarkets.DataValueField = "market_key";
                ddlmarkets.DataBind();

                //assign the card group
                if (editedItem is GridEditFormInsertItem) //is edit, and not insert
                {
                }
                else
                {
                    string strgroupkey = editedItem.GetDataKeyValue("market_key").ToString();
                    if (strgroupkey.Length > 0)
                    {
                        ddlmarkets.SelectedValue = strgroupkey;
                    }
                }
            }
        }
    }
}