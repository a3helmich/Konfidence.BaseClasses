using System;
using System.Data;
using JetBrains.Annotations;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData
{
    // TODO: internal
    public class BaseClient: IBaseClient
	{
	    //private const int ID = 0;
	    //private Guid _GuidId = Guid.Empty;

	    #region properties

		protected string ConnectionName { get; }

	    protected string ServiceName { get; }

	    //public int Id => ID;

	    #endregion

		public BaseClient(string serviceName, string connectionName)
		{
			ServiceName = serviceName;
			ConnectionName = connectionName;
		}

		public virtual void Save(IBaseDataItem dataItem)
		{
		}

        public virtual void GetItem(IBaseDataItem dataItem, string getStoredProcedure)
        {
        }

        public virtual void Delete(string deleteStoredProcedure, string autoIdField, int id)
		{
		}

	    public virtual void BuildItemList<T>(IBaseDataItemList<T> parentDataItemList,
                                                   IBaseDataItemList<T> relatedDataItemList,
                                                   IBaseDataItemList<T> childDataItemList, string getRelatedStoredProcedure) where T : IBaseDataItem
        {
            
        }

		public virtual void BuildItemList<T>(IBaseDataItemList<T> baseDataItemList, string getListStoredProcedure) where T : IBaseDataItem
        {
		}

        public virtual int ExecuteCommand(string storedProcedure, IDbParameterObjectList parameterObjectList)
        {
            return 0;
        }

		public virtual int ExecuteTextCommand(string textCommand)
		{
			return 0;
		}
		
        public virtual bool TableExists(string tableName)
		{
			return false;
		}

		public virtual bool ViewExists(string viewName)
		{
			return false;			
		}

	    public virtual bool StoredProcedureExists(string storedPprocedureName)
        {
            return false;
        }
        
        // TODO: make internal again 
        [CanBeNull]
        public virtual DataTable GetSchemaObject(string collection)
        {
            return null;
        }

        public virtual byte GetFieldInt8(string fieldName)
        {
            throw new NotImplementedException();
        }

	    public virtual short GetFieldInt16(string fieldName)
	    {
	        throw new NotImplementedException();
	    }

        public virtual int GetFieldInt32(string fieldName)
		{
			throw new NotImplementedException();
		}

	    public virtual long GetFieldInt64(string fieldName)
	    {
	        throw new NotImplementedException();
	    }

        public virtual string GetFieldString(string fieldName)
		{
			throw new NotImplementedException();
		}

        public virtual Guid GetFieldGuid(string fieldName)
        {
            throw new NotImplementedException();
        }

		public virtual bool GetFieldBool(string fieldName)
		{
			throw new NotImplementedException();
		}

		public virtual DateTime GetFieldDateTime(string fieldName)
		{
			throw new NotImplementedException();
		}

        public virtual TimeSpan GetFieldTimeSpan(string fieldName)
        {
            throw new NotImplementedException();
        }

        public virtual decimal GetFieldDecimal(string fieldName)
        {
            throw new NotImplementedException();
        }
    }
}
