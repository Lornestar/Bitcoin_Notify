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
        /// Creates an object wrapper for the Delete_Markets_All Procedure
        /// </summary>
        public static StoredProcedure DeleteMarketsAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Delete_Markets_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
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
        /// Creates an object wrapper for the Procedure Procedure
        /// </summary>
        public static StoredProcedure Procedure(int? param1, int? param2)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Procedure", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@param1", param1, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@param2", param2, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Arb_Results Procedure
        /// </summary>
        public static StoredProcedure UpdateArbResults(int? arbresultskey, int? startingmarket, int? endmarket, decimal? percentage, int? triptime, string label)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Arb_Results", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@arb_results_key", arbresultskey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@starting_market", startingmarket, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@end_market", endmarket, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@percentage", percentage, DbType.Decimal, 4, 18);
        	
            sp.Command.AddParameter("@triptime", triptime, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@label", label, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Currencies Procedure
        /// </summary>
        public static StoredProcedure UpdateCurrencies(int? currencykey, string currencyname, bool? isfiat)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Currencies", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@currency_key", currencykey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@currency_name", currencyname, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@isfiat", isfiat, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_CurrencyCloud_Market Procedure
        /// </summary>
        public static StoredProcedure UpdateCurrencyCloudMarket(int? marketkey, int? source, int? destination, decimal? feepercentage, decimal? feestatic, int? exchangetime)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_CurrencyCloud_Market", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@market_key", marketkey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@source", source, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@destination", destination, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@fee_percentage", feepercentage, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@fee_static", feestatic, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@exchangetime", exchangetime, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_CurrencyCloud_MarketData Procedure
        /// </summary>
        public static StoredProcedure UpdateCurrencyCloudMarketData(decimal? price, int? marketkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_CurrencyCloud_MarketData", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@price", price, DbType.Decimal, 8, 18);
        	
            sp.Command.AddParameter("@market_key", marketkey, DbType.Int32, 0, 10);
        	
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
        /// Creates an object wrapper for the Update_Exchange_Currency Procedure
        /// </summary>
        public static StoredProcedure UpdateExchangeCurrency(int? exchangekey, int? currency, int? exchangecurrencykeyreturn)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Exchange_Currency", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_key", exchangekey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@currency", currency, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@exchange_currency_key_return", DbType.Int32, 0, 10);
            
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
        /// Creates an object wrapper for the Update_Market Procedure
        /// </summary>
        public static StoredProcedure UpdateMarket(int? marketkey, int? source, int? destination, decimal? feepercentage, decimal? feestatic, int? exchangetime, bool? ratechanges, string apicall, int? marketkeyreturn)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Market", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@market_key", marketkey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@source", source, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@destination", destination, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@fee_percentage", feepercentage, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@fee_static", feestatic, DbType.Currency, 4, 19);
        	
            sp.Command.AddParameter("@exchangetime", exchangetime, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@ratechanges", ratechanges, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@apicall", apicall, DbType.String, null, null);
        	
            sp.Command.AddOutputParameter("@market_key_return", DbType.Int32, 0, 10);
            
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Market_APIErrorsCount Procedure
        /// </summary>
        public static StoredProcedure UpdateMarketAPIErrorsCount(int? marketkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Market_APIErrorsCount", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@market_key", marketkey, DbType.Int32, 0, 10);
        	
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
        /// Creates an object wrapper for the View_Arb_Results Procedure
        /// </summary>
        public static StoredProcedure ViewArbResults()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Arb_Results", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Currencies_All Procedure
        /// </summary>
        public static StoredProcedure ViewCurrenciesAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Currencies_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Currencies_AllFiat Procedure
        /// </summary>
        public static StoredProcedure ViewCurrenciesAllFiat()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Currencies_AllFiat", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Currencies_byname Procedure
        /// </summary>
        public static StoredProcedure ViewCurrenciesByname(string currencyname)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Currencies_byname", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@currency_name", currencyname, DbType.AnsiString, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_CurrencyCloud_Market_bySource_Destination Procedure
        /// </summary>
        public static StoredProcedure ViewCurrencyCloudMarketBySourceDestination(int? source, int? destination)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_CurrencyCloud_Market_bySource_Destination", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@source", source, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@destination", destination, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_CurrencyCloud_Markets Procedure
        /// </summary>
        public static StoredProcedure ViewCurrencyCloudMarkets()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_CurrencyCloud_Markets", DataService.GetInstance("Bitcoin_Notify"), "");
        	
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
        /// Creates an object wrapper for the View_Exchange_Currencies_SpecificExchange Procedure
        /// </summary>
        public static StoredProcedure ViewExchangeCurrenciesSpecificExchange(int? exchangekey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Exchange_Currencies_SpecificExchange", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_key", exchangekey, DbType.Int32, 0, 10);
        	
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
        /// Creates an object wrapper for the View_Exchanges_All Procedure
        /// </summary>
        public static StoredProcedure ViewExchangesAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Exchanges_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Exchanges_OnlyAlt_All Procedure
        /// </summary>
        public static StoredProcedure ViewExchangesOnlyAltAll()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Exchanges_OnlyAlt_All", DataService.GetInstance("Bitcoin_Notify"), "");
        	
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
        /// Creates an object wrapper for the View_Market_byExchange_Source_Destination Procedure
        /// </summary>
        public static StoredProcedure ViewMarketByExchangeSourceDestination(int? exchangekey1, int? exchangekey2, int? source, int? destination)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Market_byExchange_Source_Destination", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_key1", exchangekey1, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@exchange_key2", exchangekey2, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@source", source, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@destination", destination, DbType.Int32, 0, 10);
        	
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
        /// Creates an object wrapper for the View_Markets_byExchange_ForAPICall Procedure
        /// </summary>
        public static StoredProcedure ViewMarketsByExchangeForAPICall(int? exchangekey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Markets_byExchange_ForAPICall", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_key", exchangekey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Markets_byExchange_Internally Procedure
        /// </summary>
        public static StoredProcedure ViewMarketsByExchangeInternally(int? exchangekey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Markets_byExchange_Internally", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_key", exchangekey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Markets_Static_byExchange Procedure
        /// </summary>
        public static StoredProcedure ViewMarketsStaticByExchange(int? exchangekey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Markets_Static_byExchange", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange_key", exchangekey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Path_byExchanges_andCurrencies Procedure
        /// </summary>
        public static StoredProcedure ViewPathByExchangesAndCurrencies(int? exchange1, int? exchange2, int? currency1, int? currency2)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Path_byExchanges_andCurrencies", DataService.GetInstance("Bitcoin_Notify"), "dbo");
        	
            sp.Command.AddParameter("@exchange1", exchange1, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@exchange2", exchange2, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@currency1", currency1, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@currency2", currency2, DbType.Int32, 0, 10);
        	
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
        
        /// <summary>
        /// Creates an object wrapper for the View_Update_Recurring Procedure
        /// </summary>
        public static StoredProcedure ViewUpdateRecurring()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Update_Recurring", DataService.GetInstance("Bitcoin_Notify"), "");
        	
            return sp;
        }
        
    }
    
}
