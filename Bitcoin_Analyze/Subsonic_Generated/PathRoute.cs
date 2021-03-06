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
	/// Strongly-typed collection for the PathRoute class.
	/// </summary>
    [Serializable]
	public partial class PathRouteCollection : ActiveList<PathRoute, PathRouteCollection>
	{	   
		public PathRouteCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PathRouteCollection</returns>
		public PathRouteCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PathRoute o = this[i];
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
	/// This is an ActiveRecord class which wraps the Path_Routes table.
	/// </summary>
	[Serializable]
	public partial class PathRoute : ActiveRecord<PathRoute>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PathRoute()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PathRoute(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PathRoute(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PathRoute(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Path_Routes", TableType.Table, DataService.GetInstance("Bitcoin_Notify"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPathRouteKey = new TableSchema.TableColumn(schema);
				colvarPathRouteKey.ColumnName = "path_route_key";
				colvarPathRouteKey.DataType = DbType.Int32;
				colvarPathRouteKey.MaxLength = 0;
				colvarPathRouteKey.AutoIncrement = true;
				colvarPathRouteKey.IsNullable = false;
				colvarPathRouteKey.IsPrimaryKey = true;
				colvarPathRouteKey.IsForeignKey = false;
				colvarPathRouteKey.IsReadOnly = false;
				colvarPathRouteKey.DefaultSetting = @"";
				colvarPathRouteKey.ForeignKeyTableName = "";
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
				colvarPathKey.DefaultSetting = @"";
				colvarPathKey.ForeignKeyTableName = "";
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
				colvarMarketKey.DefaultSetting = @"";
				colvarMarketKey.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMarketKey);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["Bitcoin_Notify"].AddSchema("Path_Routes",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PathRouteKey")]
		[Bindable(true)]
		public int PathRouteKey 
		{
			get { return GetColumnValue<int>(Columns.PathRouteKey); }
			set { SetColumnValue(Columns.PathRouteKey, value); }
		}
		  
		[XmlAttribute("PathKey")]
		[Bindable(true)]
		public int PathKey 
		{
			get { return GetColumnValue<int>(Columns.PathKey); }
			set { SetColumnValue(Columns.PathKey, value); }
		}
		  
		[XmlAttribute("MarketKey")]
		[Bindable(true)]
		public int MarketKey 
		{
			get { return GetColumnValue<int>(Columns.MarketKey); }
			set { SetColumnValue(Columns.MarketKey, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varPathKey,int varMarketKey)
		{
			PathRoute item = new PathRoute();
			
			item.PathKey = varPathKey;
			
			item.MarketKey = varMarketKey;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varPathRouteKey,int varPathKey,int varMarketKey)
		{
			PathRoute item = new PathRoute();
			
				item.PathRouteKey = varPathRouteKey;
			
				item.PathKey = varPathKey;
			
				item.MarketKey = varMarketKey;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PathRouteKeyColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PathKeyColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn MarketKeyColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PathRouteKey = @"path_route_key";
			 public static string PathKey = @"path_key";
			 public static string MarketKey = @"market_key";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
