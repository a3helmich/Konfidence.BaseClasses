using System;
using System.Collections.Generic;
using System.Web.Services;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.ParameterObjects;

namespace Konfidence.BaseData
{
    [WebService(Namespace = "http://konfidence.nl/DataItemService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BaseDataWebService : WebService
	{
		[WebMethod]
        public int Save(DbParameterObjectList properties, int id)
		{
			var baseDataItem = GetNewDataItem();

			if (IsAssigned(baseDataItem))
			{
				SetKey(baseDataItem, id);

				SetProperties(baseDataItem, properties);

				baseDataItem.Save();

				return baseDataItem.GetId();
			}

			return 0;
		}

		[WebMethod]
        public DbParameterObjectList GetItem(int id)
		{
			var baseDataItem = GetNewDataItem(id);

            DbParameterObjectList properties = GetProperties(baseDataItem);

			return properties;
		}

        [WebMethod]
        public DbParameterObjectList GetItemByParam(DbParameterObjectList parameterList)
        {
            var baseDataItem = GetNewDataItem(parameterList);

            DbParameterObjectList properties = GetProperties(baseDataItem);

            var idProperty = new DbParameterObject {Field = "AutoIdField", Value = baseDataItem.GetId()};

            properties.Insert(0, idProperty);

            return properties;
        }

		[WebMethod]
		public void Delete(int id)
		{
			var baseDataItem = GetNewDataItem();

			if (IsAssigned(baseDataItem))
			{
				SetKey(baseDataItem, id);
			}

			baseDataItem.Delete();
		}

		[WebMethod]
        public List<DbParameterObjectList> BuildItemList()
		{
			var baseDataItemList = GetNewDataItemList();

			return baseDataItemList.Convert2ListOfParameterObjectList();
		}

        //[WebMethod]
        //public int ExecuteCommand(string storedProcedure, params object[] parameters)
        //{
        //    BaseDataItem baseDataItem = GetNewDataItem();

        //    return baseDataItem.ExecuteCommand(storedProcedure, parameters);
        //}

		[WebMethod]
		public int ExecuteTextCommand(string textCommand)
		{
			var baseDataItem = GetNewDataItem();

			return baseDataItem.ExecuteTextCommand(textCommand);
		}

		[WebMethod]
		public bool TableExists(string tableName)
		{
			var baseDataItem = GetNewDataItem();

			return baseDataItem.TableExists(tableName);
		}

		[WebMethod]
		public bool ViewExists(string viewName)
		{
			var baseDataItem = GetNewDataItem();

			return baseDataItem.ViewExists(viewName);
		}

		protected static void SetKey(BaseDataItem baseDataItem, int id)
		{
			baseDataItem.SetId(id);
		}

        protected static void SetProperties(BaseDataItem baseDataItem, DbParameterObjectList properties)
		{
			var propertyDictionary = new Dictionary<string, object>();

			foreach (var parameterObject in properties)
			{
				propertyDictionary.Add(parameterObject.Field, parameterObject.Value);
			}

			baseDataItem.SetProperties(propertyDictionary);
		}

        internal static DbParameterObjectList GetProperties(BaseDataItem baseDataItem)
		{
            var properties = new DbParameterObjectList();

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

        protected virtual BaseDataItem GetNewDataItem(DbParameterObjectList parameterList)
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