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
    /// Controller class for MarketData
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MarketDatumController
    {
        // Preload our schema..
        MarketDatum thisSchemaLoad = new MarketDatum();
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
        public MarketDatumCollection FetchAll()
        {
            MarketDatumCollection coll = new MarketDatumCollection();
            Query qry = new Query(MarketDatum.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MarketDatumCollection FetchByID(object MarketKey)
        {
            MarketDatumCollection coll = new MarketDatumCollection().Where("market_key", MarketKey).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MarketDatumCollection FetchByQuery(Query qry)
        {
            MarketDatumCollection coll = new MarketDatumCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MarketKey)
        {
            return (MarketDatum.Delete(MarketKey) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MarketKey)
        {
            return (MarketDatum.Destroy(MarketKey) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Price,DateTime Datechanged,int MarketKey,string Volume,int? Level)
	    {
		    MarketDatum item = new MarketDatum();
		    
            item.Price = Price;
            
            item.Datechanged = Datechanged;
            
            item.MarketKey = MarketKey;
            
            item.Volume = Volume;
            
            item.Level = Level;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Price,DateTime Datechanged,int MarketKey,string Volume,int? Level)
	    {
		    MarketDatum item = new MarketDatum();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Price = Price;
				
			item.Datechanged = Datechanged;
				
			item.MarketKey = MarketKey;
				
			item.Volume = Volume;
				
			item.Level = Level;
				
	        item.Save(UserName);
	    }
    }
}
