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
namespace Bitcoin_Notify_DB{
    public partial class SPs{
        
        /// <summary>
        /// Creates an object wrapper for the Delete_Path Procedure
        /// </summary>
        public static StoredProcedure DeletePath(int? pathkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Delete_Path", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@path_key", pathkey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Delete_Path_Route Procedure
        /// </summary>
        public static StoredProcedure DeletePathRoute(int? pathroutekey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Delete_Path_Route", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@path_route_key", pathroutekey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_CurrencyCloud_Tokens Procedure
        /// </summary>
        public static StoredProcedure UpdateCurrencyCloudTokens(int? cctokenkey, string cctoken)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_CurrencyCloud_Tokens", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@cctokenkey", cctokenkey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@cctoken", cctoken, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Exchange_Rates Procedure
        /// </summary>
        public static StoredProcedure UpdateExchangeRates(int? exchangeratekey, decimal? exchangerate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Exchange_Rates", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_rate_key", exchangeratekey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@exchange_rate", exchangerate, DbType.Currency, 4, 19);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_MarketData Procedure
        /// </summary>
        public static StoredProcedure UpdateMarketData(decimal? price, decimal? volume, int? marketkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_MarketData", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@price", price, DbType.Decimal, 8, 18);
        	
            sp.Command.AddParameter("@volume", volume, DbType.Decimal, 8, 18);
        	
            sp.Command.AddParameter("@market_key", marketkey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Path Procedure
        /// </summary>
        public static StoredProcedure UpdatePath(int? pathkey, int? pageorder)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Path", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@path_key", pathkey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@page_order", pageorder, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Path_Route Procedure
        /// </summary>
        public static StoredProcedure UpdatePathRoute(int? pathroutekey, int? pathkey, int? marketkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Path_Route", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@path_route_key", pathroutekey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@path_key", pathkey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@market_key", marketkey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Exchange_Currencies_All Procedure
        /// </summary>
        public static StoredProcedure ViewExchangeCurrenciesAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Exchange_Currencies_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Exchange_Rates Procedure
        /// </summary>
        public static StoredProcedure ViewExchangeRates(int? exchangeratekey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Exchange_Rates", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_rate_key", exchangeratekey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Info_CurrencyCloud_Tokens Procedure
        /// </summary>
        public static StoredProcedure ViewInfoCurrencyCloudTokens()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Info_CurrencyCloud_Tokens", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Market_Data_All Procedure
        /// </summary>
        public static StoredProcedure ViewMarketDataAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Market_Data_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Market_Data_Specific Procedure
        /// </summary>
        public static StoredProcedure ViewMarketDataSpecific(int? marketkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Market_Data_Specific", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@market_key", marketkey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Markets_All Procedure
        /// </summary>
        public static StoredProcedure ViewMarketsAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Markets_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Path_Route_Specific Procedure
        /// </summary>
        public static StoredProcedure ViewPathRouteSpecific(int? pathkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Path_Route_Specific", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@path_key", pathkey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Paths_All Procedure
        /// </summary>
        public static StoredProcedure ViewPathsAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Paths_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
    }
    
}