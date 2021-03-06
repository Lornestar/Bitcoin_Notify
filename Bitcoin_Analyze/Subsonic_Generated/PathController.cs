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
    /// Controller class for Paths
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PathController
    {
        // Preload our schema..
        Path thisSchemaLoad = new Path();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public PathCollection FetchAll()
        {
            PathCollection coll = new PathCollection();
            Query qry = new Query(Path.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PathCollection FetchByID(object PathKey)
        {
            PathCollection coll = new PathCollection().Where("path_key", PathKey).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PathCollection FetchByQuery(Query qry)
        {
            PathCollection coll = new PathCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PathKey)
        {
            return (Path.Delete(PathKey) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PathKey)
        {
            return (Path.Destroy(PathKey) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string PathName,int? PageOrder)
	    {
		    Path item = new Path();
		    
            item.PathName = PathName;
            
            item.PageOrder = PageOrder;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PathKey,string PathName,int? PageOrder)
	    {
		    Path item = new Path();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PathKey = PathKey;
				
			item.PathName = PathName;
				
			item.PageOrder = PageOrder;
				
	        item.Save(UserName);
	    }
    }
}
