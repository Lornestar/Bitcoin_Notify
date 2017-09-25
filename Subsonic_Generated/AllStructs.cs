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
	#region Tables Struct
	public partial struct Tables
	{
		
		public static readonly string ArbResult = @"Arb_Results";
        
		public static readonly string Currency = @"Currencies";
        
		public static readonly string CurrencyCloudMarket = @"CurrencyCloud_Markets";
        
		public static readonly string CurrencyCloudToken = @"CurrencyCloud_Token";
        
		public static readonly string ExchangeCurrency = @"Exchange_Currency";
        
		public static readonly string ExchangeRate = @"Exchange_Rates";
        
		public static readonly string Exchange = @"Exchanges";
        
		public static readonly string MarketOrderbook = @"Market_Orderbooks";
        
		public static readonly string MarketDatum = @"MarketData";
        
		public static readonly string Market = @"Markets";
        
		public static readonly string MarketsStatic = @"Markets_Static";
        
		public static readonly string Notification = @"Notifications";
        
		public static readonly string PathRoute = @"Path_Routes";
        
		public static readonly string Path = @"Paths";
        
		public static readonly string UpdateRecurring = @"Update_Recurring";
        
	}
	#endregion
    #region Schemas
    public partial class Schemas {
		
		public static TableSchema.Table ArbResult
		{
            get { return DataService.GetSchema("Arb_Results", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table Currency
		{
            get { return DataService.GetSchema("Currencies", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table CurrencyCloudMarket
		{
            get { return DataService.GetSchema("CurrencyCloud_Markets", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table CurrencyCloudToken
		{
            get { return DataService.GetSchema("CurrencyCloud_Token", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table ExchangeCurrency
		{
            get { return DataService.GetSchema("Exchange_Currency", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table ExchangeRate
		{
            get { return DataService.GetSchema("Exchange_Rates", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table Exchange
		{
            get { return DataService.GetSchema("Exchanges", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table MarketOrderbook
		{
            get { return DataService.GetSchema("Market_Orderbooks", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table MarketDatum
		{
            get { return DataService.GetSchema("MarketData", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table Market
		{
            get { return DataService.GetSchema("Markets", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table MarketsStatic
		{
            get { return DataService.GetSchema("Markets_Static", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table Notification
		{
            get { return DataService.GetSchema("Notifications", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table PathRoute
		{
            get { return DataService.GetSchema("Path_Routes", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table Path
		{
            get { return DataService.GetSchema("Paths", "Bitcoin_Notify"); }
		}
        
		public static TableSchema.Table UpdateRecurring
		{
            get { return DataService.GetSchema("Update_Recurring", "Bitcoin_Notify"); }
		}
        
	
    }
    #endregion
    #region View Struct
    public partial struct Views 
    {
		
		public static readonly string VwArbResult = @"vw_Arb_Results";
        
		public static readonly string VwCurrencyCloudMarket = @"vw_CurrencyCloud_Markets";
        
		public static readonly string VwExchangeCurrency = @"vw_Exchange_Currency";
        
		public static readonly string VwMarketDatum = @"vw_MarketData";
        
		public static readonly string VwMarket = @"vw_Markets";
        
		public static readonly string VwPathRoute = @"vw_Path_Routes";
        
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["Bitcoin_Notify"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository 
        {
            get 
            {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository; 
            }
            set { _repository = value; }
        }
        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {
            return Repository.SelectAllColumnsFrom<T>();
	    }
	    public static Select Select()
	    {
            return Repository.Select();
	    }
	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }
	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }
   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }
	    
	    public static Delete Delete()
	    {
            return Repository.Delete();
	    }
	    
	    public static InlineQuery Query()
	    {
            return Repository.Query();
	    }
	    	    
	    
	}
    #endregion
    
}
#region Databases
public partial struct Databases 
{
	
	public static readonly string Bitcoin_Notify = @"Bitcoin_Notify";
    
}
#endregion