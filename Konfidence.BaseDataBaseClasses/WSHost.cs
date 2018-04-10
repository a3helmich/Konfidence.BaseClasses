using System;
using System.Collections.Generic;
using System.Configuration;
using Konfidence.Base;
using Konfidence.BaseData.Objects;
using Konfidence.BaseData.WSBaseHost;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.BaseData
{
    // TODO: internal
    internal class WsHost : BaseClient
	{
		private readonly WSBaseHostService _wsBaseHostService;

        public WsHost(string serviceName, string databaseName) : base(serviceName, databaseName)
		{
            _wsBaseHostService = new WSBaseHostService();

			// _WsBaseHostService.UseDefaultCredentials = true;

			var wsUrl = GetWsUrl(serviceName);

			if (wsUrl.Equals(string.Empty))
			{
				throw new Exception("No URL provided for WebService : " + serviceName);
			}

			_wsBaseHostService.Url = wsUrl;
		}

		#region GetField Methods

	    #endregion

		public void Save(BaseDataItem dataItem)
		{
			var parameterDataItemList = dataItem.SetItemData();

			var parameterObjectList = new List<ParameterObject>();

			foreach(DbParameterObject parameterDataItem in parameterDataItemList)
			{
				var parameterObject = new ParameterObject {Field = parameterDataItem.Field, Value = parameterDataItem.Value};

			    parameterObjectList.Add(parameterObject);
			}

            dataItem.SetId(_wsBaseHostService.Save(parameterObjectList.ToArray(), dataItem.GetId()));
		}

	    public void GetItem(BaseDataItem dataItem, string getStoredProcedure) // , string autoIdField, int id)
		{
            ParameterObject[] parameterObjects;

            //if (id > 0)
            //{
            //    parameterObjects = _WsBaseHostService.GetItem(id);

            //    if (IsAssigned(parameterObjects))
            //    {
            //        ItemId = id;
            //    }
            //}
            //else
            {
                var parameterDataItemList = dataItem.SetParameterData();

                var parameterObjectList = new List<ParameterObject>();

                var parameterObject = new ParameterObject {Field = "StoredProcedure", Value = getStoredProcedure};

                parameterObjectList.Add(parameterObject);

                foreach (DbParameterObject parameterDataItem in parameterDataItemList)
                {
                    parameterObject = new ParameterObject
                        {
                            Field = parameterDataItem.Field,
                            Value = parameterDataItem.Value
                        };

                    parameterObjectList.Add(parameterObject);
                }

                parameterObjects = _wsBaseHostService.GetItemByParam(parameterObjectList.ToArray());

                //if (parameterObjects.Length > 0)
                //{
                //    parameterObject = parameterObjects[0];

                //    if (parameterObject.Field.Equals("AutoIdField"))
                //    {
                //        ItemId = (int)parameterObject.Value;
                //    }
                //}B
            }

			if (parameterObjects.IsAssigned())
			{
				var propertyDictionary = new Dictionary<string, object>();

				foreach(var parameterObject in parameterObjects)
				{
					propertyDictionary.Add(parameterObject.Field, parameterObject.Value);
				}

				dataItem.SetProperties(propertyDictionary);
			}
		}

	    public override void Delete(string deleteStoredProcedure, string autoIdField, int id)
		{
			_wsBaseHostService.Delete(id);
		}

		internal override void BuildItemList<T>(IBaseDataItemList<T> baseDataItemList, string getListStoredProcedure) 
        {
			var parameterObjectsList = _wsBaseHostService.BuildItemList();

			foreach (var parameterObjectList in parameterObjectsList)
			{
				var baseDataItem = baseDataItemList.GetDataItem();

				var parameterDictionary = new Dictionary<string, object>();

				foreach (var parameterObjectItem in parameterObjectList)
				{
					if (parameterObjectItem.Field.Equals("BaseDataItem_KeyValue"))
					{
						baseDataItem.SetId((int)parameterObjectItem.Value);
					}
					else
					{
						parameterDictionary.Add(parameterObjectItem.Field, parameterObjectItem.Value);
					}
				}

				baseDataItem.SetProperties(parameterDictionary);
			}

			base.BuildItemList(baseDataItemList, getListStoredProcedure);
		}

        //internal override int ExecuteCommand(string storedProcedure, params object[] parameters)
        //{
        //    return _WsBaseHostService.ExecuteCommand(storedProcedure, parameters);
        //}

	    public override int ExecuteTextCommand(string textCommand)
		{
			return _wsBaseHostService.ExecuteTextCommand(textCommand);
		}

	    public override bool TableExists(string tableName)
		{
			return _wsBaseHostService.TableExists(tableName);
		}

	    public override bool ViewExists(string viewName)
		{
			return _wsBaseHostService.ViewExists(viewName);
		}

		private static string GetWsUrl(string serviceName)
		{
			var wsUrl = ConfigurationManager.AppSettings[serviceName];

            if (wsUrl.IsAssigned())
			{
				return wsUrl;
			}

			return string.Empty;
		}
	}
}