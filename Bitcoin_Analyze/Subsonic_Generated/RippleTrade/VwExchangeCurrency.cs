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
namespace RippleTrade_DB{
    /// <summary>
    /// Strongly-typed collection for the VwExchangeCurrency class.
    /// </summary>
    [Serializable]
    public partial class VwExchangeCurrencyCollection : ReadOnlyList<VwExchangeCurrency, VwExchangeCurrencyCollection>
    {        
        public VwExchangeCurrencyCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the vw_Exchange_Currency view.
    /// </summary>
    [Serializable]
    public partial class VwExchangeCurrency : ReadOnlyRecord<VwExchangeCurrency>, IReadOnlyRecord
    {
    
	    #region Default Settings
	    protected static void SetSQLProps() 
	    {
		    GetTableSchema();
	    }
	    #endregion
        #region Schema Accessor
	    public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                {
                    SetSQLProps();
                }
                return BaseSchema;
            }
        }
    	
        private static void GetTableSchema() 
        {
            if(!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("vw_Exchange_Currency", TableType.View, DataService.GetInstance("RippleTrade"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarExchangeCurrencyKey = new TableSchema.TableColumn(schema);
                colvarExchangeCurrencyKey.ColumnName = "exchange_currency_key";
                colvarExchangeCurrencyKey.DataType = DbType.Int32;
                colvarExchangeCurrencyKey.MaxLength = 0;
                colvarExchangeCurrencyKey.AutoIncrement = false;
                colvarExchangeCurrencyKey.IsNullable = false;
                colvarExchangeCurrencyKey.IsPrimaryKey = false;
                colvarExchangeCurrencyKey.IsForeignKey = false;
                colvarExchangeCurrencyKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarExchangeCurrencyKey);
                
                TableSchema.TableColumn colvarCurrencyName = new TableSchema.TableColumn(schema);
                colvarCurrencyName.ColumnName = "currency_name";
                colvarCurrencyName.DataType = DbType.AnsiString;
                colvarCurrencyName.MaxLength = 4;
                colvarCurrencyName.AutoIncrement = false;
                colvarCurrencyName.IsNullable = true;
                colvarCurrencyName.IsPrimaryKey = false;
                colvarCurrencyName.IsForeignKey = false;
                colvarCurrencyName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCurrencyName);
                
                TableSchema.TableColumn colvarExchangeName = new TableSchema.TableColumn(schema);
                colvarExchangeName.ColumnName = "exchange_name";
                colvarExchangeName.DataType = DbType.String;
                colvarExchangeName.MaxLength = 50;
                colvarExchangeName.AutoIncrement = false;
                colvarExchangeName.IsNullable = false;
                colvarExchangeName.IsPrimaryKey = false;
                colvarExchangeName.IsForeignKey = false;
                colvarExchangeName.IsReadOnly = false;
                
                schema.Columns.Add(colvarExchangeName);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["RippleTrade"].AddSchema("vw_Exchange_Currency",schema);
            }
        }
        #endregion
        
        #region Query Accessor
	    public static Query CreateQuery()
	    {
		    return new Query(Schema);
	    }
	    #endregion
	    
	    #region .ctors
	    public VwExchangeCurrency()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public VwExchangeCurrency(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public VwExchangeCurrency(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public VwExchangeCurrency(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("ExchangeCurrencyKey")]
        [Bindable(true)]
        public int ExchangeCurrencyKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("exchange_currency_key");
		    }
            set 
		    {
			    SetColumnValue("exchange_currency_key", value);
            }
        }
	      
        [XmlAttribute("CurrencyName")]
        [Bindable(true)]
        public string CurrencyName 
	    {
		    get
		    {
			    return GetColumnValue<string>("currency_name");
		    }
            set 
		    {
			    SetColumnValue("currency_name", value);
            }
        }
	      
        [XmlAttribute("ExchangeName")]
        [Bindable(true)]
        public string ExchangeName 
	    {
		    get
		    {
			    return GetColumnValue<string>("exchange_name");
		    }
            set 
		    {
			    SetColumnValue("exchange_name", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ExchangeCurrencyKey = @"exchange_currency_key";
            
            public static string CurrencyName = @"currency_name";
            
            public static string ExchangeName = @"exchange_name";
            
	    }
	    #endregion
	    
	    
	    #region IAbstractRecord Members
        public new CT GetColumnValue<CT>(string columnName) {
            return base.GetColumnValue<CT>(columnName);
        }
        public object GetColumnValue(string columnName) {
            return base.GetColumnValue<object>(columnName);
        }
        #endregion
	    
    }
}
