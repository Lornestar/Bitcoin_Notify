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
    /// Strongly-typed collection for the VwArbResult class.
    /// </summary>
    [Serializable]
    public partial class VwArbResultCollection : ReadOnlyList<VwArbResult, VwArbResultCollection>
    {        
        public VwArbResultCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the vw_Arb_Results view.
    /// </summary>
    [Serializable]
    public partial class VwArbResult : ReadOnlyRecord<VwArbResult>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("vw_Arb_Results", TableType.View, DataService.GetInstance("Bitcoin_Notify"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarArbResultsKey = new TableSchema.TableColumn(schema);
                colvarArbResultsKey.ColumnName = "arb_results_key";
                colvarArbResultsKey.DataType = DbType.Int32;
                colvarArbResultsKey.MaxLength = 0;
                colvarArbResultsKey.AutoIncrement = false;
                colvarArbResultsKey.IsNullable = false;
                colvarArbResultsKey.IsPrimaryKey = false;
                colvarArbResultsKey.IsForeignKey = false;
                colvarArbResultsKey.IsReadOnly = false;
                
                schema.Columns.Add(colvarArbResultsKey);
                
                TableSchema.TableColumn colvarLabel = new TableSchema.TableColumn(schema);
                colvarLabel.ColumnName = "label";
                colvarLabel.DataType = DbType.String;
                colvarLabel.MaxLength = 100;
                colvarLabel.AutoIncrement = false;
                colvarLabel.IsNullable = true;
                colvarLabel.IsPrimaryKey = false;
                colvarLabel.IsForeignKey = false;
                colvarLabel.IsReadOnly = false;
                
                schema.Columns.Add(colvarLabel);
                
                TableSchema.TableColumn colvarStartingNode = new TableSchema.TableColumn(schema);
                colvarStartingNode.ColumnName = "starting_node";
                colvarStartingNode.DataType = DbType.Int32;
                colvarStartingNode.MaxLength = 0;
                colvarStartingNode.AutoIncrement = false;
                colvarStartingNode.IsNullable = false;
                colvarStartingNode.IsPrimaryKey = false;
                colvarStartingNode.IsForeignKey = false;
                colvarStartingNode.IsReadOnly = false;
                
                schema.Columns.Add(colvarStartingNode);
                
                TableSchema.TableColumn colvarEndNode = new TableSchema.TableColumn(schema);
                colvarEndNode.ColumnName = "end_node";
                colvarEndNode.DataType = DbType.Int32;
                colvarEndNode.MaxLength = 0;
                colvarEndNode.AutoIncrement = false;
                colvarEndNode.IsNullable = false;
                colvarEndNode.IsPrimaryKey = false;
                colvarEndNode.IsForeignKey = false;
                colvarEndNode.IsReadOnly = false;
                
                schema.Columns.Add(colvarEndNode);
                
                TableSchema.TableColumn colvarPercentage = new TableSchema.TableColumn(schema);
                colvarPercentage.ColumnName = "percentage";
                colvarPercentage.DataType = DbType.Decimal;
                colvarPercentage.MaxLength = 0;
                colvarPercentage.AutoIncrement = false;
                colvarPercentage.IsNullable = true;
                colvarPercentage.IsPrimaryKey = false;
                colvarPercentage.IsForeignKey = false;
                colvarPercentage.IsReadOnly = false;
                
                schema.Columns.Add(colvarPercentage);
                
                TableSchema.TableColumn colvarLastChanged = new TableSchema.TableColumn(schema);
                colvarLastChanged.ColumnName = "last_changed";
                colvarLastChanged.DataType = DbType.DateTime;
                colvarLastChanged.MaxLength = 0;
                colvarLastChanged.AutoIncrement = false;
                colvarLastChanged.IsNullable = true;
                colvarLastChanged.IsPrimaryKey = false;
                colvarLastChanged.IsForeignKey = false;
                colvarLastChanged.IsReadOnly = false;
                
                schema.Columns.Add(colvarLastChanged);
                
                TableSchema.TableColumn colvarTriptime = new TableSchema.TableColumn(schema);
                colvarTriptime.ColumnName = "triptime";
                colvarTriptime.DataType = DbType.Int32;
                colvarTriptime.MaxLength = 0;
                colvarTriptime.AutoIncrement = false;
                colvarTriptime.IsNullable = true;
                colvarTriptime.IsPrimaryKey = false;
                colvarTriptime.IsForeignKey = false;
                colvarTriptime.IsReadOnly = false;
                
                schema.Columns.Add(colvarTriptime);
                
                TableSchema.TableColumn colvarStartingNodeCurrency = new TableSchema.TableColumn(schema);
                colvarStartingNodeCurrency.ColumnName = "starting_node_currency";
                colvarStartingNodeCurrency.DataType = DbType.Int32;
                colvarStartingNodeCurrency.MaxLength = 0;
                colvarStartingNodeCurrency.AutoIncrement = false;
                colvarStartingNodeCurrency.IsNullable = true;
                colvarStartingNodeCurrency.IsPrimaryKey = false;
                colvarStartingNodeCurrency.IsForeignKey = false;
                colvarStartingNodeCurrency.IsReadOnly = false;
                
                schema.Columns.Add(colvarStartingNodeCurrency);
                
                TableSchema.TableColumn colvarStartingNodeExchange = new TableSchema.TableColumn(schema);
                colvarStartingNodeExchange.ColumnName = "starting_node_exchange";
                colvarStartingNodeExchange.DataType = DbType.Int32;
                colvarStartingNodeExchange.MaxLength = 0;
                colvarStartingNodeExchange.AutoIncrement = false;
                colvarStartingNodeExchange.IsNullable = true;
                colvarStartingNodeExchange.IsPrimaryKey = false;
                colvarStartingNodeExchange.IsForeignKey = false;
                colvarStartingNodeExchange.IsReadOnly = false;
                
                schema.Columns.Add(colvarStartingNodeExchange);
                
                TableSchema.TableColumn colvarEndNodeCurrency = new TableSchema.TableColumn(schema);
                colvarEndNodeCurrency.ColumnName = "end_node_currency";
                colvarEndNodeCurrency.DataType = DbType.Int32;
                colvarEndNodeCurrency.MaxLength = 0;
                colvarEndNodeCurrency.AutoIncrement = false;
                colvarEndNodeCurrency.IsNullable = true;
                colvarEndNodeCurrency.IsPrimaryKey = false;
                colvarEndNodeCurrency.IsForeignKey = false;
                colvarEndNodeCurrency.IsReadOnly = false;
                
                schema.Columns.Add(colvarEndNodeCurrency);
                
                TableSchema.TableColumn colvarEndNodeExchange = new TableSchema.TableColumn(schema);
                colvarEndNodeExchange.ColumnName = "end_node_exchange";
                colvarEndNodeExchange.DataType = DbType.Int32;
                colvarEndNodeExchange.MaxLength = 0;
                colvarEndNodeExchange.AutoIncrement = false;
                colvarEndNodeExchange.IsNullable = true;
                colvarEndNodeExchange.IsPrimaryKey = false;
                colvarEndNodeExchange.IsForeignKey = false;
                colvarEndNodeExchange.IsReadOnly = false;
                
                schema.Columns.Add(colvarEndNodeExchange);
                
                TableSchema.TableColumn colvarStartingmaker = new TableSchema.TableColumn(schema);
                colvarStartingmaker.ColumnName = "startingmaker";
                colvarStartingmaker.DataType = DbType.Boolean;
                colvarStartingmaker.MaxLength = 0;
                colvarStartingmaker.AutoIncrement = false;
                colvarStartingmaker.IsNullable = true;
                colvarStartingmaker.IsPrimaryKey = false;
                colvarStartingmaker.IsForeignKey = false;
                colvarStartingmaker.IsReadOnly = false;
                
                schema.Columns.Add(colvarStartingmaker);
                
                TableSchema.TableColumn colvarNumberofnodes = new TableSchema.TableColumn(schema);
                colvarNumberofnodes.ColumnName = "numberofnodes";
                colvarNumberofnodes.DataType = DbType.Int32;
                colvarNumberofnodes.MaxLength = 0;
                colvarNumberofnodes.AutoIncrement = false;
                colvarNumberofnodes.IsNullable = true;
                colvarNumberofnodes.IsPrimaryKey = false;
                colvarNumberofnodes.IsForeignKey = false;
                colvarNumberofnodes.IsReadOnly = false;
                
                schema.Columns.Add(colvarNumberofnodes);
                
                TableSchema.TableColumn colvarVolume = new TableSchema.TableColumn(schema);
                colvarVolume.ColumnName = "volume";
                colvarVolume.DataType = DbType.Currency;
                colvarVolume.MaxLength = 0;
                colvarVolume.AutoIncrement = false;
                colvarVolume.IsNullable = true;
                colvarVolume.IsPrimaryKey = false;
                colvarVolume.IsForeignKey = false;
                colvarVolume.IsReadOnly = false;
                
                schema.Columns.Add(colvarVolume);
                
                TableSchema.TableColumn colvarProfit = new TableSchema.TableColumn(schema);
                colvarProfit.ColumnName = "profit";
                colvarProfit.DataType = DbType.Currency;
                colvarProfit.MaxLength = 0;
                colvarProfit.AutoIncrement = false;
                colvarProfit.IsNullable = true;
                colvarProfit.IsPrimaryKey = false;
                colvarProfit.IsForeignKey = false;
                colvarProfit.IsReadOnly = false;
                
                schema.Columns.Add(colvarProfit);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["Bitcoin_Notify"].AddSchema("vw_Arb_Results",schema);
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
	    public VwArbResult()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public VwArbResult(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public VwArbResult(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public VwArbResult(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("ArbResultsKey")]
        [Bindable(true)]
        public int ArbResultsKey 
	    {
		    get
		    {
			    return GetColumnValue<int>("arb_results_key");
		    }
            set 
		    {
			    SetColumnValue("arb_results_key", value);
            }
        }
	      
        [XmlAttribute("Label")]
        [Bindable(true)]
        public string Label 
	    {
		    get
		    {
			    return GetColumnValue<string>("label");
		    }
            set 
		    {
			    SetColumnValue("label", value);
            }
        }
	      
        [XmlAttribute("StartingNode")]
        [Bindable(true)]
        public int StartingNode 
	    {
		    get
		    {
			    return GetColumnValue<int>("starting_node");
		    }
            set 
		    {
			    SetColumnValue("starting_node", value);
            }
        }
	      
        [XmlAttribute("EndNode")]
        [Bindable(true)]
        public int EndNode 
	    {
		    get
		    {
			    return GetColumnValue<int>("end_node");
		    }
            set 
		    {
			    SetColumnValue("end_node", value);
            }
        }
	      
        [XmlAttribute("Percentage")]
        [Bindable(true)]
        public decimal? Percentage 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("percentage");
		    }
            set 
		    {
			    SetColumnValue("percentage", value);
            }
        }
	      
        [XmlAttribute("LastChanged")]
        [Bindable(true)]
        public DateTime? LastChanged 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("last_changed");
		    }
            set 
		    {
			    SetColumnValue("last_changed", value);
            }
        }
	      
        [XmlAttribute("Triptime")]
        [Bindable(true)]
        public int? Triptime 
	    {
		    get
		    {
			    return GetColumnValue<int?>("triptime");
		    }
            set 
		    {
			    SetColumnValue("triptime", value);
            }
        }
	      
        [XmlAttribute("StartingNodeCurrency")]
        [Bindable(true)]
        public int? StartingNodeCurrency 
	    {
		    get
		    {
			    return GetColumnValue<int?>("starting_node_currency");
		    }
            set 
		    {
			    SetColumnValue("starting_node_currency", value);
            }
        }
	      
        [XmlAttribute("StartingNodeExchange")]
        [Bindable(true)]
        public int? StartingNodeExchange 
	    {
		    get
		    {
			    return GetColumnValue<int?>("starting_node_exchange");
		    }
            set 
		    {
			    SetColumnValue("starting_node_exchange", value);
            }
        }
	      
        [XmlAttribute("EndNodeCurrency")]
        [Bindable(true)]
        public int? EndNodeCurrency 
	    {
		    get
		    {
			    return GetColumnValue<int?>("end_node_currency");
		    }
            set 
		    {
			    SetColumnValue("end_node_currency", value);
            }
        }
	      
        [XmlAttribute("EndNodeExchange")]
        [Bindable(true)]
        public int? EndNodeExchange 
	    {
		    get
		    {
			    return GetColumnValue<int?>("end_node_exchange");
		    }
            set 
		    {
			    SetColumnValue("end_node_exchange", value);
            }
        }
	      
        [XmlAttribute("Startingmaker")]
        [Bindable(true)]
        public bool? Startingmaker 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("startingmaker");
		    }
            set 
		    {
			    SetColumnValue("startingmaker", value);
            }
        }
	      
        [XmlAttribute("Numberofnodes")]
        [Bindable(true)]
        public int? Numberofnodes 
	    {
		    get
		    {
			    return GetColumnValue<int?>("numberofnodes");
		    }
            set 
		    {
			    SetColumnValue("numberofnodes", value);
            }
        }
	      
        [XmlAttribute("Volume")]
        [Bindable(true)]
        public decimal? Volume 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("volume");
		    }
            set 
		    {
			    SetColumnValue("volume", value);
            }
        }
	      
        [XmlAttribute("Profit")]
        [Bindable(true)]
        public decimal? Profit 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("profit");
		    }
            set 
		    {
			    SetColumnValue("profit", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ArbResultsKey = @"arb_results_key";
            
            public static string Label = @"label";
            
            public static string StartingNode = @"starting_node";
            
            public static string EndNode = @"end_node";
            
            public static string Percentage = @"percentage";
            
            public static string LastChanged = @"last_changed";
            
            public static string Triptime = @"triptime";
            
            public static string StartingNodeCurrency = @"starting_node_currency";
            
            public static string StartingNodeExchange = @"starting_node_exchange";
            
            public static string EndNodeCurrency = @"end_node_currency";
            
            public static string EndNodeExchange = @"end_node_exchange";
            
            public static string Startingmaker = @"startingmaker";
            
            public static string Numberofnodes = @"numberofnodes";
            
            public static string Volume = @"volume";
            
            public static string Profit = @"profit";
            
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
