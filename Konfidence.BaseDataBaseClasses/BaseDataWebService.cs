using System;
using System.Collections.Generic;
using System.Web.Services;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseData
{
    [WebService(Namespace = "http://konfidence.nl/DataItemService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BaseDataWebService : WebService
	{
		[WebMethod]
		public int Save(List<DbParameterObject> properties, int id)
		{
			BaseDataItem baseDataItem = GetNewDataItem();

			if (IsAssigned(baseDataItem))
			{
				SetKey(baseDataItem, id);

				SetProperties(baseDataItem, properties);

				baseDataItem.Save();

				return baseDataItem._Id;
			}

			return 0;
		}

		[WebMethod]
		public List<DbParameterObject> GetItem(int id)
		{
			BaseDataItem baseDataItem = GetNewDataItem(id);

			List<DbParameterObject> properties = GetProperties(baseDataItem);

			return properties;
		}

        [WebMethod]
        public List<DbParameterObject> GetItemByParam(List<DbParameterObject> parameterList)
        {
            BaseDataItem baseDataItem = GetNewDataItem(parameterList);

            List<DbParameterObject> properties = GetProperties(baseDataItem);

            var idProperty = new DbParameterObject {Field = "AutoIdField", Value = baseDataItem._Id};

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
		public List<List<DbParameterObject>> BuildItemList()
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

		protected static void SetProperties(BaseDataItem baseDataItem, List<DbParameterObject> properties)
		{
			var propertyDictionary = new Dictionary<string, object>();

			foreach (DbParameterObject parameterObject in properties)
			{
				propertyDictionary.Add(parameterObject.Field, parameterObject.Value);
			}

			baseDataItem.SetProperties(propertyDictionary);
		}

		protected static List<DbParameterObject> GetProperties(BaseDataItem baseDataItem)
		{
			var properties = new List<DbParameterObject>();

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

        protected virtual BaseDataItem GetNewDataItem(List<DbParameterObject> ParameterList)
        {
            throw new NotImplementedException(); // NOP
        }

		protected virtual IBaseDataItemList GetNewDataItemList()
		{
			throw new NotImplementedException(); // NOP
		}

        [ContractAnnotation("assignedObject:null => false")]
        protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

        //protected void LoadParameterList(BaseDataItem dataItem, List<BaseDataItem.ParameterObject> ParameterList)
        //{
        //    dataItem.GetItem(ParameterList);
        //}
	}
}