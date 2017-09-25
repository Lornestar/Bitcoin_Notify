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
namespace Bitcoin_Notify_DB
{
    /// <summary>
    /// Controller class for Markets
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MarketController
    {
        // Preload our schema..
        Market thisSchemaLoad = new Market();
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
        public MarketCollection FetchAll()
        {
            MarketCollection coll = new MarketCollection();
            Query qry = new Query(Market.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MarketCollection FetchByID(object MarketKey)
        {
            MarketCollection coll = new MarketCollection().Where("market_key", MarketKey).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MarketCollection FetchByQuery(Query qry)
        {
            MarketCollection coll = new MarketCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MarketKey)
        {
            return (Market.Delete(MarketKey) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MarketKey)
        {
            return (Market.Destroy(MarketKey) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int MarketKey,int Source,int Destination,bool Automatic,decimal? FeePercentage,decimal? FeeStatic,bool? Ratechanges,int? Exchangetime,int? Apierrorcount,string Apicall)
	    {
		    Market item = new Market();
		    
            item.MarketKey = MarketKey;
            
            item.Source = Source;
            
            item.Destination = Destination;
            
            item.Automatic = Automatic;
            
            item.FeePercentage = FeePercentage;
            
            item.FeeStatic = FeeStatic;
            
            item.Ratechanges = Ratechanges;
            
            item.Exchangetime = Exchangetime;
            
            item.Apierrorcount = Apierrorcount;
            
            item.Apicall = Apicall;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int MarketKey,int Source,int Destination,bool Automatic,decimal? FeePercentage,decimal? FeeStatic,bool? Ratechanges,int? Exchangetime,int? Apierrorcount,string Apicall)
	    {
		    Market item = new Market();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.MarketKey = MarketKey;
				
			item.Source = Source;
				
			item.Destination = Destination;
				
			item.Automatic = Automatic;
				
			item.FeePercentage = FeePercentage;
				
			item.FeeStatic = FeeStatic;
				
			item.Ratechanges = Ratechanges;
				
			item.Exchangetime = Exchangetime;
				
			item.Apierrorcount = Apierrorcount;
				
			item.Apicall = Apicall;
				
	        item.Save(UserName);
	    }
    }
}
