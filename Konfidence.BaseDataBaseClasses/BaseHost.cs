using System;
using Konfidence.Base;
using System.Data;

namespace Konfidence.BaseData
{
	public class BaseHost: BaseItem
	{
		private int _Id = 0;

		private string _DataBaseName = string.Empty;
		private string _ServiceName = string.Empty;

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
				return ItemId;
			}
		}

		protected int ItemId
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
			}
		}

		#endregion

		public BaseHost(string serviceName, string databaseName)
		{
			_ServiceName = serviceName;
			_DataBaseName = databaseName;
		}

		internal virtual void Save(BaseDataItem dataItem, string saveStoredProcedure, string autoIdField, int id)
		{
		}

		internal virtual void GetItem(BaseDataItem dataItem, string getStoredProcedure, string autoIdField, int id)
		{
		}

		internal virtual void Delete(string deleteStoredProcedure, string autoIdField, int id)
		{
		}

		internal virtual void BuildItemList(IBaseDataItemList baseDataItemList, string getListStoredProcedure)
		{
		}

		internal virtual int ExecuteCommand(string storedProcedure, params object[] parameters)
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

        internal virtual Decimal GetFieldDecimal(string fieldName)
        {
            throw new NotImplementedException();
        }
    }
}
