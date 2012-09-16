using System;
using System.Collections.Generic;
using System.Configuration;
using Konfidence.BaseData.WSBaseHost;

namespace Konfidence.BaseData
{
	internal class WSHost : BaseHost
	{
		private WSBaseHostService _WsBaseHostService;

        public WSHost(string serviceName, string dataBaseName) : base(serviceName, dataBaseName)
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
        internal override Int16 GetFieldInt16(string fieldName)
        {
            return base.GetFieldInt16(fieldName);
        }
        
        internal override int GetFieldInt32(string fieldName)
		{
			return base.GetFieldInt32(fieldName);
		}

        internal override Guid GetFieldGuid(string fieldName)
        {
            return base.GetFieldGuid(fieldName);
        }

        internal override string GetFieldString(string fieldName)
		{
			return base.GetFieldString(fieldName);
		}

		internal override bool GetFieldBool(string fieldName)
		{
			return base.GetFieldBool(fieldName);
		}

		internal override DateTime GetFieldDateTime(string fieldName)
		{
			return base.GetFieldDateTime(fieldName);
		}

        internal override TimeSpan GetFieldTimeSpan(string fieldName)
        {
            return base.GetFieldTimeSpan(fieldName);
        }

        internal override Decimal GetFieldDecimal(string fieldName)
        {
            return base.GetFieldDecimal(fieldName);
        }
#endregion

		internal override void Save(BaseDataItem dataItem)
		{
			List<DbParameterObject> ParameterDataItemList = dataItem.SetItemData();

			List<ParameterObject> parameterObjectList = new List<ParameterObject>();

			foreach(DbParameterObject parameterDataItem in ParameterDataItemList)
			{
				ParameterObject parameterObject = new ParameterObject();

				parameterObject.Field = parameterDataItem.Field;
				parameterObject.Value = parameterDataItem.Value;

				parameterObjectList.Add(parameterObject);
			}

            dataItem._Id = _WsBaseHostService.Save(parameterObjectList.ToArray(), dataItem.Id);
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
                List<DbParameterObject> ParameterDataItemList = dataItem.SetParameterData();

                List<ParameterObject> parameterObjectList = new List<ParameterObject>();

                ParameterObject parameterObject = new ParameterObject();

                parameterObject.Field = "StoredProcedure";
                parameterObject.Value = getStoredProcedure;

                parameterObjectList.Add(parameterObject);

                foreach (DbParameterObject parameterDataItem in ParameterDataItemList)
                {
                    parameterObject = new ParameterObject();

                    parameterObject.Field = parameterDataItem.Field;
                    parameterObject.Value = parameterDataItem.Value;

                    parameterObjectList.Add(parameterObject);
                }

                parameterObjects = _WsBaseHostService.GetItemByParam(parameterObjectList.ToArray());

                if (parameterObjects.Length > 0)
                {
                    parameterObject = parameterObjects[0];

                    if (parameterObject.Field.Equals("AutoIdField"))
                    {
                        ItemId = (int)parameterObject.Value;
                    }
                }
            }

			if (IsAssigned(parameterObjects))
			{
				Dictionary<string, object> propertyDictionary = new Dictionary<string, object>();

				foreach(ParameterObject parameterObject in parameterObjects)
				{
					propertyDictionary.Add(parameterObject.Field, parameterObject.Value);
				}

				dataItem.SetProperties(propertyDictionary);
			}
		}

		internal override void Delete(string deleteStoredProcedure, string autoIdField, int id)
		{
			_WsBaseHostService.Delete(id);
		}

		internal override void BuildItemList(IBaseDataItemList baseDataItemList, string getListStoredProcedure)
		{
			ParameterObject[][] parameterObjectsList = _WsBaseHostService.BuildItemList();

			foreach (ParameterObject[] parameterObjectList in parameterObjectsList)
			{
				BaseDataItem baseDataItem = baseDataItemList.GetDataItem();

				Dictionary<string, object> parameterDictionary = new Dictionary<string, object>();

				foreach (ParameterObject parameterObjectItem in parameterObjectList)
				{
					if (parameterObjectItem.Field.Equals("BaseDataItem_KeyValue"))
					{
						baseDataItem.SetKey((int)parameterObjectItem.Value);
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

		internal override int ExecuteCommand(string storedProcedure, params object[] parameters)
		{
			return _WsBaseHostService.ExecuteCommand(storedProcedure, parameters);
		}

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

            if (!IsEmpty(wsUrl))
			{
				return wsUrl;
			}

			return string.Empty;
		}
	}
}