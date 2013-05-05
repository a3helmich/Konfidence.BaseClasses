using System;
using Konfidence.Base;
using System.Data;
using Konfidence.BaseData.ParameterObjects;

namespace Konfidence.BaseData
{
	internal class BaseHost: BaseItem
	{
	    private const int ID = 0;
	    //private Guid _GuidId = Guid.Empty;

		private readonly string _DataBaseName = string.Empty;
		private readonly string _ServiceName = string.Empty;

		#region properties

		protected string DataBaseName
		{
			get
			{
				return _DataBaseName;
			}
		}

		protected string ServiceName
		{
			get
			{
				return _ServiceName;
			}
		}

		public int Id
		{
			get
			{
				return ID;
			}
		}

		#endregion

		public BaseHost(string serviceName, string databaseName)
		{
			_ServiceName = serviceName;
			_DataBaseName = databaseName;
		}

		internal virtual void Save(BaseDataItem dataItem)
		{
		}

        internal virtual void GetItem(BaseDataItem dataItem, string getStoredProcedure)
        {
        }

        internal protected virtual void Delete(string deleteStoredProcedure, string autoIdField, int id)
		{
		}

        internal virtual void BuildItemList(IBaseDataItemList parentDataItemList,
                                                   IBaseDataItemList relatedDataItemList,
                                                   IBaseDataItemList childDataItemList, string getRelatedStoredProcedure)
        {
            
        }

		internal virtual void BuildItemList(IBaseDataItemList baseDataItemList, string getListStoredProcedure)
		{
		}

        internal virtual int ExecuteCommand(string storedProcedure, DbParameterObjectList parameterObjectList)
        {
            return 0;
        }

		internal virtual int ExecuteTextCommand(string textCommand)
		{
			return 0;
		}
		
        internal virtual bool TableExists(string tableName)
		{
			return false;
		}

		internal virtual bool ViewExists(string viewName)
		{
			return false;			
		}

        internal virtual bool StoredProcedureExists(string storedPprocedureName)
        {
            return false;
        }
        
        internal virtual DataTable GetSchemaObject(string collection)
        {
            return null;
        }

        internal virtual Int16 GetFieldInt16(string fieldName)
        {
            throw new NotImplementedException();
        }

		internal virtual Int32 GetFieldInt32(string fieldName)
		{
			throw new NotImplementedException();
		}

		internal virtual string GetFieldString(string fieldName)
		{
			throw new NotImplementedException();
		}

        internal virtual Guid GetFieldGuid(string fieldName)
        {
            throw new NotImplementedException();
        }

		internal virtual bool GetFieldBool(string fieldName)
		{
			throw new NotImplementedException();
		}

		internal virtual DateTime GetFieldDateTime(string fieldName)
		{
			throw new NotImplementedException();
		}

        internal virtual TimeSpan GetFieldTimeSpan(string fieldName)
        {
            throw new NotImplementedException();
        }

        internal virtual Decimal GetFieldDecimal(string fieldName)
        {
            throw new NotImplementedException();
        }
    }
}
