using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseDatabaseClasses.Objects;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData
{
	//[WebService(Namespace = "http://konfidence.nl/DataItemService")]
	//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class BaseDataWebService //: WebService
	{
		//[WebMethod]
		[UsedImplicitly]
		public int Save(List<IDbParameterObject> properties, int id)
		{
			var baseDataItem = GetNewDataItem();

			if (baseDataItem.IsAssigned())
			{
				SetKey(baseDataItem, id);

				SetProperties(baseDataItem, properties);

				baseDataItem.Save();

				return baseDataItem.GetId();
			}

			return 0;
		}

		//[WebMethod]
		[UsedImplicitly]
		[NotNull]
		public List<IDbParameterObject> GetItem(int id)
		{
			var baseDataItem = GetNewDataItem(id);

			var properties = GetProperties(baseDataItem);

			return properties;
		}

		//[WebMethod]
		[NotNull]
		[UsedImplicitly]
		public List<IDbParameterObject> GetItemByParam(List<IDbParameterObject> parameterList)
		{
			var baseDataItem = GetNewDataItem(parameterList);

			var properties = GetProperties(baseDataItem);

			//var idProperty = new DbParameterObject { ParameterName = "AutoIdField", Value = baseDataItem.GetId() };

			//properties.Insert(0, idProperty);

			properties.SetParameter("AutoIdField", baseDataItem.GetId());

			return properties;
		}

		//[WebMethod]
		[UsedImplicitly]
		public void Delete(int id)
		{
			var baseDataItem = GetNewDataItem();

			if (baseDataItem.IsAssigned())
			{
				SetKey(baseDataItem, id);
			}

			baseDataItem.Delete();
		}

		//[WebMethod]
		[CanBeNull]
		[UsedImplicitly]
		public List<List<IDbParameterObject>> BuildItemList()
		{
			return null;
		}

		[UsedImplicitly]
		private static List<IDbParameterObject> GetNewDataItemList()
		{
			throw new NotImplementedException();
		}

		//[WebMethod]
		//public int ExecuteCommand(string storedProcedure, params object[] parameters)
		//{
		//    BaseDataItem baseDataItem = GetNewDataItem();

		//    return baseDataItem.ExecuteCommand(storedProcedure, parameters);
		//}

		//[WebMethod]
		[UsedImplicitly]
		public int ExecuteTextCommand(string textCommand)
		{
			var baseDataItem = GetNewDataItem();

			return baseDataItem.ExecuteTextCommand(textCommand);
		}

		//[WebMethod]
		[UsedImplicitly]
		public bool TableExists(string tableName)
		{
			var baseDataItem = GetNewDataItem();

			return baseDataItem.TableExists(tableName);
		}

		//[WebMethod]
		[UsedImplicitly]
		public bool ViewExists(string viewName)
		{
			var baseDataItem = GetNewDataItem();

			return baseDataItem.ViewExists(viewName);
		}

		protected static void SetKey([NotNull] BaseDataItem baseDataItem, int id)
		{
			baseDataItem.SetId(id);
		}

		protected static void SetProperties([NotNull] BaseDataItem baseDataItem, [NotNull] List<IDbParameterObject> properties)
		{
			var propertyDictionary = new Dictionary<string, object>();

			foreach (var parameterObject in properties)
			{
				propertyDictionary.Add(parameterObject.ParameterName, parameterObject.Value);
			}

			baseDataItem.SetProperties(propertyDictionary);
		}

		[NotNull]
		internal static List<IDbParameterObject> GetProperties([NotNull] BaseDataItem baseDataItem)
		{
			var properties = new List<IDbParameterObject>();

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

		protected virtual BaseDataItem GetNewDataItem(List<IDbParameterObject> parameterList)
		{
			throw new NotImplementedException(); // NOP
		}
	}
}