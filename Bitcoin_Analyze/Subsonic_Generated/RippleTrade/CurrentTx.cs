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
	/// Strongly-typed collection for the CurrentTx class.
	/// </summary>
    [Serializable]
	public partial class CurrentTxCollection : ActiveList<CurrentTx, CurrentTxCollection>
	{	   
		public CurrentTxCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CurrentTxCollection</returns>
		public CurrentTxCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CurrentTx o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the CurrentTx table.
	/// </summary>
	[Serializable]
	public partial class CurrentTx : ActiveRecord<CurrentTx>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CurrentTx()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CurrentTx(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CurrentTx(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CurrentTx(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("CurrentTx", TableType.Table, DataService.GetInstance("RippleTrade"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarCurrentTxKey = new TableSchema.TableColumn(schema);
				colvarCurrentTxKey.ColumnName = "current_tx_key";
				colvarCurrentTxKey.DataType = DbType.Int32;
				colvarCurrentTxKey.MaxLength = 0;
				colvarCurrentTxKey.AutoIncrement = true;
				colvarCurrentTxKey.IsNullable = false;
				colvarCurrentTxKey.IsPrimaryKey = true;
				colvarCurrentTxKey.IsForeignKey = false;
				colvarCurrentTxKey.IsReadOnly = false;
				colvarCurrentTxKey.DefaultSetting = @"";
				colvarCurrentTxKey.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrentTxKey);
				
				TableSchema.TableColumn colvarMarketKey = new TableSchema.TableColumn(schema);
				colvarMarketKey.ColumnName = "market_key";
				colvarMarketKey.DataType = DbType.Int32;
				colvarMarketKey.MaxLength = 0;
				colvarMarketKey.AutoIncrement = false;
				colvarMarketKey.IsNullable = false;
				colvarMarketKey.IsPrimaryKey = false;
				colvarMarketKey.IsForeignKey = false;
				colvarMarketKey.IsReadOnly = false;
				colvarMarketKey.DefaultSetting = @"";
				colvarMarketKey.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMarketKey);
				
				TableSchema.TableColumn colvarDateCreated = new TableSchema.TableColumn(schema);
				colvarDateCreated.ColumnName = "date_created";
				colvarDateCreated.DataType = DbType.DateTime;
				colvarDateCreated.MaxLength = 0;
				colvarDateCreated.AutoIncrement = false;
				colvarDateCreated.IsNullable = false;
				colvarDateCreated.IsPrimaryKey = false;
				colvarDateCreated.IsForeignKey = false;
				colvarDateCreated.IsReadOnly = false;
				colvarDateCreated.DefaultSetting = @"";
				colvarDateCreated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateCreated);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["RippleTrade"].AddSchema("CurrentTx",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("CurrentTxKey")]
		[Bindable(true)]
		public int CurrentTxKey 
		{
			get { return GetColumnValue<int>(Columns.CurrentTxKey); }
			set { SetColumnValue(Columns.CurrentTxKey, value); }
		}
		  
		[XmlAttribute("MarketKey")]
		[Bindable(true)]
		public int MarketKey 
		{
			get { return GetColumnValue<int>(Columns.MarketKey); }
			set { SetColumnValue(Columns.MarketKey, value); }
		}
		  
		[XmlAttribute("DateCreated")]
		[Bindable(true)]
		public DateTime DateCreated 
		{
			get { return GetColumnValue<DateTime>(Columns.DateCreated); }
			set { SetColumnValue(Columns.DateCreated, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varMarketKey,DateTime varDateCreated)
		{
			CurrentTx item = new CurrentTx();
			
			item.MarketKey = varMarketKey;
			
			item.DateCreated = varDateCreated;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varCurrentTxKey,int varMarketKey,DateTime varDateCreated)
		{
			CurrentTx item = new CurrentTx();
			
				item.CurrentTxKey = varCurrentTxKey;
			
				item.MarketKey = varMarketKey;
			
				item.DateCreated = varDateCreated;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn CurrentTxKeyColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MarketKeyColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DateCreatedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string CurrentTxKey = @"current_tx_key";
			 public static string MarketKey = @"market_key";
			 public static string DateCreated = @"date_created";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
