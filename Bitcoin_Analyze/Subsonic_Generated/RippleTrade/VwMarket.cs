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
    /// Strongly-typed collection for the VwMarket class.
    /// </summary>
    [Serializable]
    public partial class VwMarketCollection : ReadOnlyList<VwMarket, VwMarketCollection>
    {        
        public VwMarketCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the vw_Markets view.
    /// </summary>
    [Serializable]
    public partial class VwMarket : ReadOnlyRecord<VwMarket>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("vw_Markets", TableType.View, DataService.GetInstance("RippleTrade"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
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
                
                TableSchema.TableColumn colvarAutomatic = new TableSchema.TableColumn(schema);
                colvarAutomatic.ColumnName = "automatic";
                colvarAutomatic.DataType = DbType.Boolean;
                colvarAutomatic.MaxLength = 0;
                colvarAutomatic.AutoIncrement = false;
                colvarAutomatic.IsNullable = false;
                colvarAutomatic.IsPrimaryKey = false;
                colvarAutomatic.IsForeignKey = false;
                colvarAutomatic.IsReadOnly = false;
                
                schema.Columns.Add(colvarAutomatic);
                
                TableSchema.TableColumn colvarFeePercentage = new TableSchema.TableColumn(schema);
                colvarFeePercentage.ColumnName = "fee_percentage";
                colvarFeePercentage.DataType = DbType.Currency;
                colvarFeePercentage.MaxLength = 0;
                colvarFeePercentage.AutoIncrement = false;
                colvarFeePercentage.IsNullable = true;
                colvarFeePercentage.IsPrimaryKey = false;
                colvarFeePercentage.IsForeignKey = false;
                colvarFeePercentage.IsReadOnly = false;
                
                schema.Columns.Add(colvarFeePercentage);
                
                TableSchema.TableColumn colvarFeeStatic = new TableSchema.TableColumn(schema);
                colvarFeeStatic.ColumnName = "fee_static";
                colvarFeeStatic.DataType = DbType.Currency;
                colvarFeeStatic.MaxLength = 0;
                colvarFeeStatic.AutoIncrement = false;
                colvarFeeStatic.IsNullable = true;
                colvarFeeStatic.IsPrimaryKey = false;
                colvarFeeStatic.IsForeignKey = false;
                colvarFeeStatic.IsReadOnly = false;
                
                schema.Columns.Add(colvarFeeStatic);
                
                TableSchema.TableColumn colvarRatechanges = new TableSchema.TableColumn(schema);
                colvarRatechanges.ColumnName = "ratechanges";
                colvarRatechanges.DataType = DbType.Boolean;
                colvarRatechanges.MaxLength = 0;
                colvarRatechanges.AutoIncrement = false;
                colvarRatechanges.IsNullable = true;
                colvarRatechanges.IsPrimaryKey = false;
                colvarRatechanges.IsForeignKey = false;
                colvarRatechanges.IsReadOnly = false;
                
                schema.Columns.Add(colvarRatechanges);
                
                TableSchema.TableColumn colvarExchangetime = new TableSchema.TableColumn(schema);
                colvarExchangetime.ColumnName = "exchangetime";
                colvarExchangetime.DataType = DbType.Int32;
                colvarExchangetime.MaxLength = 0;
                colvarExchangetime.AutoIncrement = false;
                colvarExchangetime.IsNullable = true;
                colvarExchangetime.IsPrimaryKey = false;
                colvarExchangetime.IsForeignKey = false;
                colvarExchangetime.IsReadOnly = false;
                
                schema.Columns.Add(colvarExchangetime);
                
                TableSchema.TableColumn colvarSourceExchange = new TableSchema.TableColumn(schema);
                colvarSourceExchange.ColumnName = "Source_Exchange";
                colvarSourceExchange.DataType = DbType.String;
                colvarSourceExchange.MaxLength = 50;
                colvarSourceExchange.AutoIncrement = false;
                colvarSourceExchange.IsNullable = false;
                colvarSourceExchange.IsPrimaryKey = false;
                colvarSourceExchange.IsForeignKey = false;
                colvarSourceExchange.IsReadOnly = false;
                
                schema.Columns.Add(colvarSourceExchange);
                
                TableSchema.TableColumn colvarSourceCurrency = new TableSchema.TableColumn(schema);
                colvarSourceCurrency.ColumnName = "Source_Currency";
                colvarSourceCurrency.DataType = DbType.AnsiString;
                colvarSourceCurrency.MaxLength = 4;
                colvarSourceCurrency.AutoIncrement = false;
                colvarSourceCurrency.IsNullable = true;
                colvarSourceCurrency.IsPrimaryKey = false;
                colvarSourceCurrency.IsForeignKey = false;
                colvarSourceCurrency.IsReadOnly = false;
                
                schema.Columns.Add(colvarSourceCurrency);
                
                TableSchema.TableColumn colvarDestinationExchange = new TableSchema.TableColumn(schema);
                colvarDestinationExchange.ColumnName = "Destination_Exchange";
                colvarDestinationExchange.DataType = DbType.String;
                colvarDestinationExchange.MaxLength = 50;
                colvarDestinationExchange.AutoIncrement = false;
                colvarDestinationExchange.IsNullable = false;
                colvarDestinationExchange.IsPrimaryKey = false;
                colvarDestinationExchange.IsForeignKey = false;
                colvarDestinationExchange.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestinationExchange);
                
                TableSchema.TableColumn colvarDestinationCurrency = new TableSchema.TableColumn(schema);
                colvarDestinationCurrency.ColumnName = "Destination_Currency";
                colvarDestinationCurrency.DataType = DbType.AnsiString;
                colvarDestinationCurrency.MaxLength = 4;
                colvarDestinationCurrency.AutoIncrement = false;
                colvarDestinationCurrency.IsNullable = true;
                colvarDestinationCurrency.IsPrimaryKey = false;
                colvarDestinationCurrency.IsForeignKey = false;
                colvarDestinationCurrency.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestinationCurrency);
                
                TableSchema.TableColumn colvarSourceShortname = new TableSchema.TableColumn(schema);
                colvarSourceShortname.ColumnName = "Source_Shortname";
                colvarSourceShortname.DataType = DbType.AnsiString;
                colvarSourceShortname.MaxLength = 4;
                colvarSourceShortname.AutoIncrement = false;
                colvarSourceShortname.IsNullable = true;
                colvarSourceShortname.IsPrimaryKey = false;
                colvarSourceShortname.IsForeignKey = false;
                colvarSourceShortname.IsReadOnly = false;
                
                schema.Columns.Add(colvarSourceShortname);
                
                TableSchema.TableColumn colvarDestinationShortname = new TableSchema.TableColumn(schema);
                colvarDestinationShortname.ColumnName = "Destination_Shortname";
                colvarDestinationShortname.DataType = DbType.AnsiString;
                colvarDestinationShortname.MaxLength = 4;
                colvarDestinationShortname.AutoIncrement = false;
                colvarDestinationShortname.IsNullable = true;
                colvarDestinationShortname.IsPrimaryKey = false;
                colvarDestinationShortname.IsForeignKey = false;
                colvarDestinationShortname.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestinationShortname);
                
                TableSchema.TableColumn colvarMarketShortname = new TableSchema.TableColumn(schema);
                colvarMarketShortname.ColumnName = "market_shortname";
                colvarMarketShortname.DataType = DbType.AnsiString;
                colvarMarketShortname.MaxLength = 20;
                colvarMarketShortname.AutoIncrement = false;
                colvarMarketShortname.IsNullable = true;
                colvarMarketShortname.IsPrimaryKey = false;
                colvarMarketShortname.IsForeignKey = false;
                colvarMarketShortname.IsReadOnly = false;
                
                schema.Columns.Add(colvarMarketShortname);
                
                TableSchema.TableColumn colvarSourceExchangeKey = new TableSchema.TableColumn(schema);
                colvarSourceExchangeKey.ColumnName = "Source_Exchange_Key";
                colvarSourceExchangeKey.DataType = DbType.Int32;
                colvarSourceExchangeKey.MaxLength = 0;
                colvarSourceExchangeKey.AutoIncrement = false;
                colvarSourceExchangeKey.IsNullable = false;
                colvarSourceExchangeKey.IsPrimaryKey = false;
                colvarSourceExchangeKey.IsForeignKey = false;
                colvarSourceExchangeKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarSourceExchangeKey);
                
                TableSchema.TableColumn colvarDestinationExchangeKey = new TableSchema.TableColumn(schema);
                colvarDestinationExchangeKey.ColumnName = "Destination_Exchange_Key";
                colvarDestinationExchangeKey.DataType = DbType.Int32;
                colvarDestinationExchangeKey.MaxLength = 0;
                colvarDestinationExchangeKey.AutoIncrement = false;
                colvarDestinationExchangeKey.IsNullable = false;
                colvarDestinationExchangeKey.IsPrimaryKey = false;
                colvarDestinationExchangeKey.IsForeignKey = false;
                colvarDestinationExchangeKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestinationExchangeKey);
                
                TableSchema.TableColumn colvarSourceCurrencyKey = new TableSchema.TableColumn(schema);
                colvarSourceCurrencyKey.ColumnName = "Source_Currency_Key";
                colvarSourceCurrencyKey.DataType = DbType.Int32;
                colvarSourceCurrencyKey.MaxLength = 0;
                colvarSourceCurrencyKey.AutoIncrement = false;
                colvarSourceCurrencyKey.IsNullable = true;
                colvarSourceCurrencyKey.IsPrimaryKey = false;
                colvarSourceCurrencyKey.IsForeignKey = false;
                colvarSourceCurrencyKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarSourceCurrencyKey);
                
                TableSchema.TableColumn colvarDestinationCurrencyKey = new TableSchema.TableColumn(schema);
                colvarDestinationCurrencyKey.ColumnName = "Destination_Currency_Key";
                colvarDestinationCurrencyKey.DataType = DbType.Int32;
                colvarDestinationCurrencyKey.MaxLength = 0;
                colvarDestinationCurrencyKey.AutoIncrement = false;
                colvarDestinationCurrencyKey.IsNullable = true;
                colvarDestinationCurrencyKey.IsPrimaryKey = false;
                colvarDestinationCurrencyKey.IsForeignKey = false;
                colvarDestinationCurrencyKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestinationCurrencyKey);
                
                TableSchema.TableColumn colvarSourceAddress = new TableSchema.TableColumn(schema);
                colvarSourceAddress.ColumnName = "source_address";
                colvarSourceAddress.DataType = DbType.AnsiString;
                colvarSourceAddress.MaxLength = 50;
                colvarSourceAddress.AutoIncrement = false;
                colvarSourceAddress.IsNullable = true;
                colvarSourceAddress.IsPrimaryKey = false;
                colvarSourceAddress.IsForeignKey = false;
                colvarSourceAddress.IsReadOnly = false;
                
                schema.Columns.Add(colvarSourceAddress);
                
                TableSchema.TableColumn colvarDestinationAddress = new TableSchema.TableColumn(schema);
                colvarDestinationAddress.ColumnName = "destination_address";
                colvarDestinationAddress.DataType = DbType.AnsiString;
                colvarDestinationAddress.MaxLength = 50;
                colvarDestinationAddress.AutoIncrement = false;
                colvarDestinationAddress.IsNullable = true;
                colvarDestinationAddress.IsPrimaryKey = false;
                colvarDestinationAddress.IsForeignKey = false;
                colvarDestinationAddress.IsReadOnly = false;
                
                schema.Columns.Add(colvarDestinationAddress);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["RippleTrade"].AddSchema("vw_Markets",schema);
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
	    public VwMarket()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public VwMarket(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public VwMarket(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public VwMarket(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("Automatic")]
        [Bindable(true)]
        public bool Automatic 
	    {
		    get
		    {
			    return GetColumnValue<bool>("automatic");
		    }
            set 
		    {
			    SetColumnValue("automatic", value);
            }
        }
	      
        [XmlAttribute("FeePercentage")]
        [Bindable(true)]
        public decimal? FeePercentage 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("fee_percentage");
		    }
            set 
		    {
			    SetColumnValue("fee_percentage", value);
            }
        }
	      
        [XmlAttribute("FeeStatic")]
        [Bindable(true)]
        public decimal? FeeStatic 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("fee_static");
		    }
            set 
		    {
			    SetColumnValue("fee_static", value);
            }
        }
	      
        [XmlAttribute("Ratechanges")]
        [Bindable(true)]
        public bool? Ratechanges 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("ratechanges");
		    }
            set 
		    {
			    SetColumnValue("ratechanges", value);
            }
        }
	      
        [XmlAttribute("Exchangetime")]
        [Bindable(true)]
        public int? Exchangetime 
	    {
		    get
		    {
			    return GetColumnValue<int?>("exchangetime");
		    }
            set 
		    {
			    SetColumnValue("exchangetime", value);
            }
        }
	      
        [XmlAttribute("SourceExchange")]
        [Bindable(true)]
        public string SourceExchange 
	    {
		    get
		    {
			    return GetColumnValue<string>("Source_Exchange");
		    }
            set 
		    {
			    SetColumnValue("Source_Exchange", value);
            }
        }
	      
        [XmlAttribute("SourceCurrency")]
        [Bindable(true)]
        public string SourceCurrency 
	    {
		    get
		    {
			    return GetColumnValue<string>("Source_Currency");
		    }
            set 
		    {
			    SetColumnValue("Source_Currency", value);
            }
        }
	      
        [XmlAttribute("DestinationExchange")]
        [Bindable(true)]
        public string DestinationExchange 
	    {
		    get
		    {
			    return GetColumnValue<string>("Destination_Exchange");
		    }
            set 
		    {
			    SetColumnValue("Destination_Exchange", value);
            }
        }
	      
        [XmlAttribute("DestinationCurrency")]
        [Bindable(true)]
        public string DestinationCurrency 
	    {
		    get
		    {
			    return GetColumnValue<string>("Destination_Currency");
		    }
            set 
		    {
			    SetColumnValue("Destination_Currency", value);
            }
        }
	      
        [XmlAttribute("SourceShortname")]
        [Bindable(true)]
        public string SourceShortname 
	    {
		    get
		    {
			    return GetColumnValue<string>("Source_Shortname");
		    }
            set 
		    {
			    SetColumnValue("Source_Shortname", value);
            }
        }
	      
        [XmlAttribute("DestinationShortname")]
        [Bindable(true)]
        public string DestinationShortname 
	    {
		    get
		    {
			    return GetColumnValue<string>("Destination_Shortname");
		    }
            set 
		    {
			    SetColumnValue("Destination_Shortname", value);
            }
        }
	      
        [XmlAttribute("MarketShortname")]
        [Bindable(true)]
        public string MarketShortname 
	    {
		    get
		    {
			    return GetColumnValue<string>("market_shortname");
		    }
            set 
		    {
			    SetColumnValue("market_shortname", value);
            }
        }
	      
        [XmlAttribute("SourceExchangeKey")]
        [Bindable(true)]
        public int SourceExchangeKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("Source_Exchange_Key");
		    }
            set 
		    {
			    SetColumnValue("Source_Exchange_Key", value);
            }
        }
	      
        [XmlAttribute("DestinationExchangeKey")]
        [Bindable(true)]
        public int DestinationExchangeKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("Destination_Exchange_Key");
		    }
            set 
		    {
			    SetColumnValue("Destination_Exchange_Key", value);
            }
        }
	      
        [XmlAttribute("SourceCurrencyKey")]
        [Bindable(true)]
        public int? SourceCurrencyKey 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Source_Currency_Key");
		    }
            set 
		    {
			    SetColumnValue("Source_Currency_Key", value);
            }
        }
	      
        [XmlAttribute("DestinationCurrencyKey")]
        [Bindable(true)]
        public int? DestinationCurrencyKey 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Destination_Currency_Key");
		    }
            set 
		    {
			    SetColumnValue("Destination_Currency_Key", value);
            }
        }
	      
        [XmlAttribute("SourceAddress")]
        [Bindable(true)]
        public string SourceAddress 
	    {
		    get
		    {
			    return GetColumnValue<string>("source_address");
		    }
            set 
		    {
			    SetColumnValue("source_address", value);
            }
        }
	      
        [XmlAttribute("DestinationAddress")]
        [Bindable(true)]
        public string DestinationAddress 
	    {
		    get
		    {
			    return GetColumnValue<string>("destination_address");
		    }
            set 
		    {
			    SetColumnValue("destination_address", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string MarketKey = @"market_key";
            
            public static string Source = @"source";
            
            public static string Destination = @"destination";
            
            public static string Automatic = @"automatic";
            
            public static string FeePercentage = @"fee_percentage";
            
            public static string FeeStatic = @"fee_static";
            
            public static string Ratechanges = @"ratechanges";
            
            public static string Exchangetime = @"exchangetime";
            
            public static string SourceExchange = @"Source_Exchange";
            
            public static string SourceCurrency = @"Source_Currency";
            
            public static string DestinationExchange = @"Destination_Exchange";
            
            public static string DestinationCurrency = @"Destination_Currency";
            
            public static string SourceShortname = @"Source_Shortname";
            
            public static string DestinationShortname = @"Destination_Shortname";
            
            public static string MarketShortname = @"market_shortname";
            
            public static string SourceExchangeKey = @"Source_Exchange_Key";
            
            public static string DestinationExchangeKey = @"Destination_Exchange_Key";
            
            public static string SourceCurrencyKey = @"Source_Currency_Key";
            
            public static string DestinationCurrencyKey = @"Destination_Currency_Key";
            
            public static string SourceAddress = @"source_address";
            
            public static string DestinationAddress = @"destination_address";
            
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
