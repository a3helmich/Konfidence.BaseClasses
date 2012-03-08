using System;
using System.Collections.Generic;
using System.Data.Common;
using Konfidence.Base;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData
{
	public interface IBaseDataItemList
	{
		void SetParameters(string storedProcedure, Database database, DbCommand dbCommand);
		void AddItem(BaseHost dataHost);
		BaseDataItem GetDataItem();
		List<List<BaseDataItem.ParameterObject>> Convert2ListOfParameterObjectList();
	}

	public class BaseDataItemList<T>: List<T>, IBaseDataItemList where T: BaseDataItem
	{

		private string _GetListStoredProcedure = string.Empty;
		private string _DataBaseName = string.Empty;
		private string _ServiceName = string.Empty;

		#region properties

		protected string DataBaseName
		{
			get { return _DataBaseName; }
			set { _DataBaseName = value; }
		}

		protected string GetListStoredProcedure
		{
			get { return _GetListStoredProcedure; }
			set { _GetListStoredProcedure = value; }
		}

		protected string ServiceName
		{
			get { return _ServiceName; }
			set { _ServiceName = value; }
		}

		#endregion

		public BaseDataItemList()
		{
			InitializeDataItemList();
		}

		protected void BuildItemList(string getListStoredProcedure)
		{
            // TODO : fix: don't forget the defaultStoredProcedure this way
            string defaultList = GetListStoredProcedure;

			GetListStoredProcedure = getListStoredProcedure;

			BuildItemList();

            GetListStoredProcedure = defaultList;
		}

        private BaseHost GetHost()
        {
            return HostFactory.GetHost(_ServiceName, _DataBaseName);
        }

		protected void BuildItemList()
		{
            BaseHost dataHost = GetHost();

			dataHost.BuildItemList(this, GetListStoredProcedure);
		}

		protected void RebuildItemList()
		{
			Clear();

			BuildItemList();
		}

		protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

        protected static bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }

		/// <summary>
		/// create and return a xxxDataItem derived from BaseDataItem
		/// </summary>
		/// <returns></returns>
		protected virtual T GetNewDataItem()
		{
			throw new NotImplementedException(); // NOP
		}

		public BaseDataItem GetDataItem()
		{
			T baseDataItem = GetNewDataItem();

			Add(baseDataItem);

			return baseDataItem;
		}

		public List<List<BaseDataItem.ParameterObject>> Convert2ListOfParameterObjectList()
		{
			List<List<BaseDataItem.ParameterObject>> baseDataItemListList = new List<List<BaseDataItem.ParameterObject>>();

			foreach (BaseDataItem baseDataItem in this)
			{
				List<BaseDataItem.ParameterObject> properties = GetProperties(baseDataItem);

				if (baseDataItem.AutoIdField.Length > 0)
				{
					BaseDataItem.ParameterObject property = new BaseDataItem.ParameterObject();

					property.Field = "BaseDataItem_KeyValue";
					property.Value = baseDataItem.Id;

					properties.Add(property);
				}

				baseDataItemListList.Add(properties);
			}

			return baseDataItemListList;
		}

		private static List<BaseDataItem.ParameterObject> GetProperties(BaseDataItem baseDataItem)
		{
			List<BaseDataItem.ParameterObject> properties = new List<BaseDataItem.ParameterObject>();

			baseDataItem.GetProperties(properties);

			return properties;
		}

		/// <summary>
		/// Add parameters for filtering
		/// </summary>
		/// <returns></returns>
		public virtual void SetParameters(string storedProcedure, Database database, DbCommand dbCommand)
		{
			// NOP
		}

		protected virtual void InitializeDataItemList()
		{
			// NOP
		}

		public void AddItem(BaseHost dataHost)
		{
			T baseDataItem = GetNewDataItem();

			baseDataItem._DataHost = dataHost;

			if (IsAssigned(baseDataItem))
			{
				baseDataItem.GetKey();

				baseDataItem.GetData();

				Add(baseDataItem);
			}
		}

		protected int ExecuteTextCommand(string textCommand)
		{
            BaseHost dataHost = GetHost(); 

			return dataHost.ExecuteTextCommand(textCommand);
		}

		protected int ExecuteCommand(string storedProcedure, params object[] parameters)
		{
            BaseHost dataHost = GetHost();

			return dataHost.ExecuteCommand(storedProcedure, parameters);
		}

		protected bool TableExists(string tableName)
		{
            BaseHost dataHost = GetHost();

			return dataHost.TableExists(tableName);
		}

		protected bool ViewExists(string viewName)
		{
            BaseHost dataHost = GetHost();

			return dataHost.ViewExists(viewName);
		}

        protected bool StoredProcedureExists(string storedProcedureName)
        {
            BaseHost dataHost = GetHost();

            return dataHost.StoredProcedureExists(storedProcedureName);
        }
	}
}