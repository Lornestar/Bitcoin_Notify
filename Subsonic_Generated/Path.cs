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
	/// Strongly-typed collection for the Path class.
	/// </summary>
    [Serializable]
	public partial class PathCollection : ActiveList<Path, PathCollection>
	{	   
		public PathCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PathCollection</returns>
		public PathCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Path o = this[i];
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
	/// This is an ActiveRecord class which wraps the Paths table.
	/// </summary>
	[Serializable]
	public partial class Path : ActiveRecord<Path>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Path()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Path(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Path(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Path(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Paths", TableType.Table, DataService.GetInstance("Bitcoin_Notify"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPathKey = new TableSchema.TableColumn(schema);
				colvarPathKey.ColumnName = "path_key";
				colvarPathKey.DataType = DbType.Int32;
				colvarPathKey.MaxLength = 0;
				colvarPathKey.AutoIncrement = true;
				colvarPathKey.IsNullable = false;
				colvarPathKey.IsPrimaryKey = true;
				colvarPathKey.IsForeignKey = false;
				colvarPathKey.IsReadOnly = false;
				colvarPathKey.DefaultSetting = @"";
				colvarPathKey.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPathKey);
				
				TableSchema.TableColumn colvarPathName = new TableSchema.TableColumn(schema);
				colvarPathName.ColumnName = "path_name";
				colvarPathName.DataType = DbType.String;
				colvarPathName.MaxLength = 100;
				colvarPathName.AutoIncrement = false;
				colvarPathName.IsNullable = true;
				colvarPathName.IsPrimaryKey = false;
				colvarPathName.IsForeignKey = false;
				colvarPathName.IsReadOnly = false;
				colvarPathName.DefaultSetting = @"";
				colvarPathName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPathName);
				
				TableSchema.TableColumn colvarPageOrder = new TableSchema.TableColumn(schema);
				colvarPageOrder.ColumnName = "page_order";
				colvarPageOrder.DataType = DbType.Int32;
				colvarPageOrder.MaxLength = 0;
				colvarPageOrder.AutoIncrement = false;
				colvarPageOrder.IsNullable = true;
				colvarPageOrder.IsPrimaryKey = false;
				colvarPageOrder.IsForeignKey = false;
				colvarPageOrder.IsReadOnly = false;
				colvarPageOrder.DefaultSetting = @"";
				colvarPageOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPageOrder);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["Bitcoin_Notify"].AddSchema("Paths",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PathKey")]
		[Bindable(true)]
		public int PathKey 
		{
			get { return GetColumnValue<int>(Columns.PathKey); }
			set { SetColumnValue(Columns.PathKey, value); }
		}
		  
		[XmlAttribute("PathName")]
		[Bindable(true)]
		public string PathName 
		{
			get { return GetColumnValue<string>(Columns.PathName); }
			set { SetColumnValue(Columns.PathName, value); }
		}
		  
		[XmlAttribute("PageOrder")]
		[Bindable(true)]
		public int? PageOrder 
		{
			get { return GetColumnValue<int?>(Columns.PageOrder); }
			set { SetColumnValue(Columns.PageOrder, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varPathName,int? varPageOrder)
		{
			Path item = new Path();
			
			item.PathName = varPathName;
			
			item.PageOrder = varPageOrder;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varPathKey,string varPathName,int? varPageOrder)
		{
			Path item = new Path();
			
				item.PathKey = varPathKey;
			
				item.PathName = varPathName;
			
				item.PageOrder = varPageOrder;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PathKeyColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PathNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PageOrderColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PathKey = @"path_key";
			 public static string PathName = @"path_name";
			 public static string PageOrder = @"page_order";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
