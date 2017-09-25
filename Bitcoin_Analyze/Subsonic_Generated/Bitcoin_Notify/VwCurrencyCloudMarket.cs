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
    /// <summary>
    /// Strongly-typed collection for the VwCurrencyCloudMarket class.
    /// </summary>
    [Serializable]
    public partial class VwCurrencyCloudMarketCollection : ReadOnlyList<VwCurrencyCloudMarket, VwCurrencyCloudMarketCollection>
    {        
        public VwCurrencyCloudMarketCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the vw_CurrencyCloud_Markets view.
    /// </summary>
    [Serializable]
    public partial class VwCurrencyCloudMarket : ReadOnlyRecord<VwCurrencyCloudMarket>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("vw_CurrencyCloud_Markets", TableType.View, DataService.GetInstance("Bitcoin_Notify"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarCurrencycloudMarketKey = new TableSchema.TableColumn(schema);
                colvarCurrencycloudMarketKey.ColumnName = "currencycloud_market_key";
                colvarCurrencycloudMarketKey.DataType = DbType.Int32;
                colvarCurrencycloudMarketKey.MaxLength = 0;
                colvarCurrencycloudMarketKey.AutoIncrement = false;
                colvarCurrencycloudMarketKey.IsNullable = false;
                colvarCurrencycloudMarketKey.IsPrimaryKey = false;
                colvarCurrencycloudMarketKey.IsForeignKey = false;
                colvarCurrencycloudMarketKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarCurrencycloudMarketKey);
                
                TableSchema.TableColumn colvarSource = new TableSchema.TableColumn(schema);
                colvarSource.ColumnName = "source";
                colvarSource.DataType = DbType.Int32;
                colvarSource.MaxLength = 0;
                colvarSource.AutoIncrement = false;
                colvarSource.IsNullable = false;
                colvarSource.IsPrimaryKey = false;
                colvarSource.IsForeignKey = false;
                colvarSource.IsReadOnly = false;
                
                schema.Columns.Add(colvarSource);
                
                TableSchema.TableColumn colvarDestination = new TableSchema.TableColumn(schema);
                colvarDestination.ColumnName = "destination";
                colvarDestination.DataType = DbType.Int32;
                colvarDestination.MaxLength = 0;
                colvarDestination.AutoIncrement = false;
                colvarDestination.IsNullable = false;
                colvarDestination.IsPrimaryKey = false;
                colvarDestination.IsForeignKey = false;
                colvarDestination.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestination);
                
                TableSchema.TableColumn colvarFeePercentage = new TableSchema.TableColumn(schema);
                colvarFeePercentage.ColumnName = "fee_percentage";
                colvarFeePercentage.DataType = DbType.Decimal;
                colvarFeePercentage.MaxLength = 0;
                colvarFeePercentage.AutoIncrement = false;
                colvarFeePercentage.IsNullable = false;
                colvarFeePercentage.IsPrimaryKey = false;
                colvarFeePercentage.IsForeignKey = false;
                colvarFeePercentage.IsReadOnly = false;
                
                schema.Columns.Add(colvarFeePercentage);
                
                TableSchema.TableColumn colvarRate = new TableSchema.TableColumn(schema);
                colvarRate.ColumnName = "rate";
                colvarRate.DataType = DbType.Currency;
                colvarRate.MaxLength = 0;
                colvarRate.AutoIncrement = false;
                colvarRate.IsNullable = true;
                colvarRate.IsPrimaryKey = false;
                colvarRate.IsForeignKey = false;
                colvarRate.IsReadOnly = false;
                
                schema.Columns.Add(colvarRate);
                
                TableSchema.TableColumn colvarDatechanged = new TableSchema.TableColumn(schema);
                colvarDatechanged.ColumnName = "datechanged";
                colvarDatechanged.DataType = DbType.DateTime;
                colvarDatechanged.MaxLength = 0;
                colvarDatechanged.AutoIncrement = false;
                colvarDatechanged.IsNullable = true;
                colvarDatechanged.IsPrimaryKey = false;
                colvarDatechanged.IsForeignKey = false;
                colvarDatechanged.IsReadOnly = false;
                
                schema.Columns.Add(colvarDatechanged);
                
                TableSchema.TableColumn colvarFeeStatic = new TableSchema.TableColumn(schema);
                colvarFeeStatic.ColumnName = "fee_static";
                colvarFeeStatic.DataType = DbType.Decimal;
                colvarFeeStatic.MaxLength = 0;
                colvarFeeStatic.AutoIncrement = false;
                colvarFeeStatic.IsNullable = false;
                colvarFeeStatic.IsPrimaryKey = false;
                colvarFeeStatic.IsForeignKey = false;
                colvarFeeStatic.IsReadOnly = false;
                
                schema.Columns.Add(colvarFeeStatic);
                
                TableSchema.TableColumn colvarExchangetime = new TableSchema.TableColumn(schema);
                colvarExchangetime.ColumnName = "exchangetime";
                colvarExchangetime.DataType = DbType.Int32;
                colvarExchangetime.MaxLength = 0;
                colvarExchangetime.AutoIncrement = false;
                colvarExchangetime.IsNullable = false;
                colvarExchangetime.IsPrimaryKey = false;
                colvarExchangetime.IsForeignKey = false;
                colvarExchangetime.IsReadOnly = false;
                
                schema.Columns.Add(colvarExchangetime);
                
                TableSchema.TableColumn colvarSourcename = new TableSchema.TableColumn(schema);
                colvarSourcename.ColumnName = "sourcename";
                colvarSourcename.DataType = DbType.AnsiString;
                colvarSourcename.MaxLength = 4;
                colvarSourcename.AutoIncrement = false;
                colvarSourcename.IsNullable = true;
                colvarSourcename.IsPrimaryKey = false;
                colvarSourcename.IsForeignKey = false;
                colvarSourcename.IsReadOnly = false;
                
                schema.Columns.Add(colvarSourcename);
                
                TableSchema.TableColumn colvarDestinationname = new TableSchema.TableColumn(schema);
                colvarDestinationname.ColumnName = "destinationname";
                colvarDestinationname.DataType = DbType.AnsiString;
                colvarDestinationname.MaxLength = 4;
                colvarDestinationname.AutoIncrement = false;
                colvarDestinationname.IsNullable = true;
                colvarDestinationname.IsPrimaryKey = false;
                colvarDestinationname.IsForeignKey = false;
                colvarDestinationname.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestinationname);
                
                TableSchema.TableColumn colvarMarketKey = new TableSchema.TableColumn(schema);
                colvarMarketKey.ColumnName = "market_key";
                colvarMarketKey.DataType = DbType.Int32;
                colvarMarketKey.MaxLength = 0;
                colvarMarketKey.AutoIncrement = false;
                colvarMarketKey.IsNullable = false;
                colvarMarketKey.IsPrimaryKey = false;
                colvarMarketKey.IsForeignKey = false;
                colvarMarketKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarMarketKey);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["Bitcoin_Notify"].AddSchema("vw_CurrencyCloud_Markets",schema);
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
	    public VwCurrencyCloudMarket()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public VwCurrencyCloudMarket(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public VwCurrencyCloudMarket(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public VwCurrencyCloudMarket(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("CurrencycloudMarketKey")]
        [Bindable(true)]
        public int CurrencycloudMarketKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("currencycloud_market_key");
		    }
            set 
		    {
			    SetColumnValue("currencycloud_market_key", value);
            }
        }
	      
        [XmlAttribute("Source")]
        [Bindable(true)]
        public int Source 
	    {
		    get
		    {
			    return GetColumnValue<int>("source");
		    }
            set 
		    {
			    SetColumnValue("source", value);
            }
        }
	      
        [XmlAttribute("Destination")]
        [Bindable(true)]
        public int Destination 
	    {
		    get
		    {
			    return GetColumnValue<int>("destination");
		    }
            set 
		    {
			    SetColumnValue("destination", value);
            }
        }
	      
        [XmlAttribute("FeePercentage")]
        [Bindable(true)]
        public decimal FeePercentage 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("fee_percentage");
		    }
            set 
		    {
			    SetColumnValue("fee_percentage", value);
            }
        }
	      
        [XmlAttribute("Rate")]
        [Bindable(true)]
        public decimal? Rate 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("rate");
		    }
            set 
		    {
			    SetColumnValue("rate", value);
            }
        }
	      
        [XmlAttribute("Datechanged")]
        [Bindable(true)]
        public DateTime? Datechanged 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("datechanged");
		    }
            set 
		    {
			    SetColumnValue("datechanged", value);
            }
        }
	      
        [XmlAttribute("FeeStatic")]
        [Bindable(true)]
        public decimal FeeStatic 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("fee_static");
		    }
            set 
		    {
			    SetColumnValue("fee_static", value);
            }
        }
	      
        [XmlAttribute("Exchangetime")]
        [Bindable(true)]
        public int Exchangetime 
	    {
		    get
		    {
			    return GetColumnValue<int>("exchangetime");
		    }
            set 
		    {
			    SetColumnValue("exchangetime", value);
            }
        }
	      
        [XmlAttribute("Sourcename")]
        [Bindable(true)]
        public string Sourcename 
	    {
		    get
		    {
			    return GetColumnValue<string>("sourcename");
		    }
            set 
		    {
			    SetColumnValue("sourcename", value);
            }
        }
	      
        [XmlAttribute("Destinationname")]
        [Bindable(true)]
        public string Destinationname 
	    {
		    get
		    {
			    return GetColumnValue<string>("destinationname");
		    }
            set 
		    {
			    SetColumnValue("destinationname", value);
            }
        }
	      
        [XmlAttribute("MarketKey")]
        [Bindable(true)]
        public int MarketKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("market_key");
		    }
            set 
		    {
			    SetColumnValue("market_key", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string CurrencycloudMarketKey = @"currencycloud_market_key";
            
            public static string Source = @"source";
            
            public static string Destination = @"destination";
            
            public static string FeePercentage = @"fee_percentage";
            
            public static string Rate = @"rate";
            
            public static string Datechanged = @"datechanged";
            
            public static string FeeStatic = @"fee_static";
            
            public static string Exchangetime = @"exchangetime";
            
            public static string Sourcename = @"sourcename";
            
            public static string Destinationname = @"destinationname";
            
            public static string MarketKey = @"market_key";
            
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
