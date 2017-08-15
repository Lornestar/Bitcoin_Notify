using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
// <auto-generated />
namespace RippleTrade_DB
{
    /// <summary>
    /// Controller class for CurrencyCloud_Token
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CurrencyCloudTokenController
    {
        // Preload our schema..
        CurrencyCloudToken thisSchemaLoad = new CurrencyCloudToken();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public CurrencyCloudTokenCollection FetchAll()
        {
            CurrencyCloudTokenCollection coll = new CurrencyCloudTokenCollection();
            Query qry = new Query(CurrencyCloudToken.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CurrencyCloudTokenCollection FetchByID(object CurrencyCloudTokensKey)
        {
            CurrencyCloudTokenCollection coll = new CurrencyCloudTokenCollection().Where("CurrencyCloud_Tokens_Key", CurrencyCloudTokensKey).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CurrencyCloudTokenCollection FetchByQuery(Query qry)
        {
            CurrencyCloudTokenCollection coll = new CurrencyCloudTokenCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CurrencyCloudTokensKey)
        {
            return (CurrencyCloudToken.Delete(CurrencyCloudTokensKey) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CurrencyCloudTokensKey)
        {
            return (CurrencyCloudToken.Destroy(CurrencyCloudTokensKey) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int CurrencyCloudTokensKey,string CurrencyCloudTokenX,DateTime? Lastchanged)
	    {
		    CurrencyCloudToken item = new CurrencyCloudToken();
		    
            item.CurrencyCloudTokensKey = CurrencyCloudTokensKey;
            
            item.CurrencyCloudTokenX = CurrencyCloudTokenX;
            
            item.Lastchanged = Lastchanged;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CurrencyCloudTokensKey,string CurrencyCloudTokenX,DateTime? Lastchanged)
	    {
		    CurrencyCloudToken item = new CurrencyCloudToken();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CurrencyCloudTokensKey = CurrencyCloudTokensKey;
				
			item.CurrencyCloudTokenX = CurrencyCloudTokenX;
				
			item.Lastchanged = Lastchanged;
				
	        item.Save(UserName);
	    }
    }
}