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
    /// Strongly-typed collection for the VwPathRoute class.
    /// </summary>
    [Serializable]
    public partial class VwPathRouteCollection : ReadOnlyList<VwPathRoute, VwPathRouteCollection>
    {        
        public VwPathRouteCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the vw_Path_Routes view.
    /// </summary>
    [Serializable]
    public partial class VwPathRoute : ReadOnlyRecord<VwPathRoute>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("vw_Path_Routes", TableType.View, DataService.GetInstance("RippleTrade"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarPathRouteKey = new TableSchema.TableColumn(schema);
                colvarPathRouteKey.ColumnName = "path_route_key";
                colvarPathRouteKey.DataType = DbType.Int32;
                colvarPathRouteKey.MaxLength = 0;
                colvarPathRouteKey.AutoIncrement = false;
                colvarPathRouteKey.IsNullable = false;
                colvarPathRouteKey.IsPrimaryKey = false;
                colvarPathRouteKey.IsForeignKey = false;
                colvarPathRouteKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarPathRouteKey);
                
                TableSchema.TableColumn colvarPathKey = new TableSchema.TableColumn(schema);
                colvarPathKey.ColumnName = "path_key";
                colvarPathKey.DataType = DbType.Int32;
                colvarPathKey.MaxLength = 0;
                colvarPathKey.AutoIncrement = false;
                colvarPathKey.IsNullable = false;
                colvarPathKey.IsPrimaryKey = false;
                colvarPathKey.IsForeignKey = false;
                colvarPathKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarPathKey);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["RippleTrade"].AddSchema("vw_Path_Routes",schema);
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
	    public VwPathRoute()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public VwPathRoute(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public VwPathRoute(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public VwPathRoute(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("PathRouteKey")]
        [Bindable(true)]
        public int PathRouteKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("path_route_key");
		    }
            set 
		    {
			    SetColumnValue("path_route_key", value);
            }
        }
	      
        [XmlAttribute("PathKey")]
        [Bindable(true)]
        public int PathKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("path_key");
		    }
            set 
		    {
			    SetColumnValue("path_key", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PathRouteKey = @"path_route_key";
            
            public static string PathKey = @"path_key";
            
            public static string MarketKey = @"market_key";
            
            public static string SourceExchange = @"Source_Exchange";
            
            public static string SourceCurrency = @"Source_Currency";
            
            public static string DestinationExchange = @"Destination_Exchange";
            
            public static string DestinationCurrency = @"Destination_Currency";
            
            public static string SourceShortname = @"Source_Shortname";
            
            public static string DestinationShortname = @"Destination_Shortname";
            
            public static string MarketShortname = @"market_shortname";
            
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
