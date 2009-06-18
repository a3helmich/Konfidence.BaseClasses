using System;
using System.Collections.Generic;
using System.Web.Services;
using Konfidence.Base;

namespace Konfidence.BaseData
{
    [WebService(Namespace = "http://konfidence.nl/DataItemService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BaseWebService : WebService
	{
		[WebMethod]
		public int Save(List<BaseDataItem.ParameterObject> properties, int id)
		{
			BaseDataItem baseDataItem = GetNewDataItem();

			if (IsAssigned(baseDataItem))
			{
				SetKey(baseDataItem, id);

				SetProperties(baseDataItem, properties);

				baseDataItem.Save();

				return baseDataItem.Id;
			}

			return 0;
		}

		[WebMethod]
		public List<BaseDataItem.ParameterObject> GetItem(int id)
		{
			BaseDataItem baseDataItem = GetNewDataItem(id);

			List<BaseDataItem.ParameterObject> properties = GetProperties(baseDataItem);

			return properties;
		}

        [WebMethod]
        public List<BaseDataItem.ParameterObject> GetItemByParam(List<BaseDataItem.ParameterObject> ParameterList)
        {
            BaseDataItem baseDataItem = GetNewDataItem(ParameterList);

            List<BaseDataItem.ParameterObject> properties = GetProperties(baseDataItem);

            BaseDataItem.ParameterObject idProperty = new BaseDataItem.ParameterObject();

            idProperty.Field = "AutoIdField";
            idProperty.Value = baseDataItem.Id;

            properties.Insert(0, idProperty);

            return properties;
        }

		[WebMethod]
		public void Delete(int id)
		{
			BaseDataItem baseDataItem = GetNewDataItem();

			if (IsAssigned(baseDataItem))
			{
				SetKey(baseDataItem, id);
			}

			baseDataItem.Delete();
		}

		[WebMethod]
		public List<List<BaseDataItem.ParameterObject>> BuildItemList()
		{
			IBaseDataItemList baseDataItemList = GetNewDataItemList();

			return baseDataItemList.Convert2ListOfParameterObjectList();
		}

		[WebMethod]
		public int ExecuteCommand(string storedProcedure, params object[] parameters)
		{
			BaseDataItem baseDataItem = GetNewDataItem();

			return baseDataItem.ExecuteCommand(storedProcedure, parameters);
		}

		[WebMethod]
		public int ExecuteTextCommand(string textCommand)
		{
			BaseDataItem baseDataItem = GetNewDataItem();

			return baseDataItem.ExecuteTextCommand(textCommand);
		}

		[WebMethod]
		public bool TableExists(string tableName)
		{
			BaseDataItem baseDataItem = GetNewDataItem();

			return baseDataItem.TableExists(tableName);
		}

		[WebMethod]
		public bool ViewExists(string viewName)
		{
			BaseDataItem baseDataItem = GetNewDataItem();

			return baseDataItem.ViewExists(viewName);
		}

		protected static void SetKey(BaseDataItem baseDataItem, int id)
		{
			baseDataItem.SetKey(id);
		}

		protected static void SetProperties(BaseDataItem baseDataItem, List<BaseDataItem.ParameterObject> properties)
		{
			Dictionary<string, object> propertyDictionary = new Dictionary<string, object>();

			foreach (BaseDataItem.ParameterObject parameterObject in properties)
			{
				propertyDictionary.Add(parameterObject.Field, parameterObject.Value);
			}

			baseDataItem.SetProperties(propertyDictionary);
		}

		protected static List<BaseDataItem.ParameterObject> GetProperties(BaseDataItem baseDataItem)
		{
			List<BaseDataItem.ParameterObject> properties = new List<BaseDataItem.ParameterObject>();

			baseDataItem.GetProperties(properties);

			return properties;
		}

		protected virtual BaseDataItem GetNewDataItem()
		{
			throw new NotImplementedException(); // NOP
		}

		protected virtual BaseDataItem GetNewDataItem(int id)
		{
			throw new NotImplementedException(); // NOP
		}

        protected virtual BaseDataItem GetNewDataItem(List<BaseDataItem.ParameterObject> ParameterList)
        {
            throw new NotImplementedException(); // NOP
        }

		protected virtual IBaseDataItemList GetNewDataItemList()
		{
			throw new NotImplementedException(); // NOP
		}

		protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

        protected void LoadParameterList(BaseDataItem dataItem, List<BaseDataItem.ParameterObject> ParameterList)
        {
            dataItem.GetItem(ParameterList);
        }
	}
}