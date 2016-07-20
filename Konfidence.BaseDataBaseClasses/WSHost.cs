using System;
using System.Collections.Generic;
using System.Configuration;
using Konfidence.Base;
using Konfidence.BaseData.ParameterObjects;
using Konfidence.BaseData.WSBaseHost;

namespace Konfidence.BaseData
{
	internal class WsHost : BaseHost
	{
		private readonly WSBaseHostService _WsBaseHostService;

        public WsHost(string serviceName, string dataBaseName) : base(serviceName, dataBaseName)
		{
            _WsBaseHostService = new WSBaseHostService();

			// _WsBaseHostService.UseDefaultCredentials = true;

			string wsUrl = GetWsUrl(serviceName);

			if (wsUrl.Equals(string.Empty))
			{
				throw new Exception("No URL provided for WebService : " + serviceName);
			}

			_WsBaseHostService.Url = wsUrl;
		}

		#region GetField Methods

	    #endregion

		internal override void Save(BaseDataItem dataItem)
		{
			var parameterDataItemList = dataItem.SetItemData();

			var parameterObjectList = new List<ParameterObject>();

			foreach(DbParameterObject parameterDataItem in parameterDataItemList)
			{
				var parameterObject = new ParameterObject {Field = parameterDataItem.Field, Value = parameterDataItem.Value};

			    parameterObjectList.Add(parameterObject);
			}

            dataItem.SetId(_WsBaseHostService.Save(parameterObjectList.ToArray(), dataItem.GetId()));
		}

		internal override void GetItem(BaseDataItem dataItem, string getStoredProcedure) // , string autoIdField, int id)
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

                parameterObjects = _WsBaseHostService.GetItemByParam(parameterObjectList.ToArray());

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

				foreach(ParameterObject parameterObject in parameterObjects)
				{
					propertyDictionary.Add(parameterObject.Field, parameterObject.Value);
				}

				dataItem.SetProperties(propertyDictionary);
			}
		}

		protected internal override void Delete(string deleteStoredProcedure, string autoIdField, int id)
		{
			_WsBaseHostService.Delete(id);
		}

		internal override void BuildItemList(IBaseDataItemList baseDataItemList, string getListStoredProcedure)
		{
			ParameterObject[][] parameterObjectsList = _WsBaseHostService.BuildItemList();

			foreach (ParameterObject[] parameterObjectList in parameterObjectsList)
			{
				var baseDataItem = baseDataItemList.GetDataItem();

				var parameterDictionary = new Dictionary<string, object>();

				foreach (ParameterObject parameterObjectItem in parameterObjectList)
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

		internal override int ExecuteTextCommand(string textCommand)
		{
			return _WsBaseHostService.ExecuteTextCommand(textCommand);
		}

		internal override bool TableExists(string tableName)
		{
			return _WsBaseHostService.TableExists(tableName);
		}

		internal override bool ViewExists(string viewName)
		{
			return _WsBaseHostService.ViewExists(viewName);
		}

		private static string GetWsUrl(string serviceName)
		{
			string wsUrl = ConfigurationManager.AppSettings[serviceName];

            if (wsUrl.IsAssigned())
			{
				return wsUrl;
			}

			return string.Empty;
		}
	}
}