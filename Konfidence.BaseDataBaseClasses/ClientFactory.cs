using System;
using System.Configuration;
using Konfidence.Base;

namespace Konfidence.BaseData
{
	internal class ClientFactory
	{
		private static bool WsEnabled
		{
			get
			{
				var wsEnabled = ConfigurationManager.AppSettings["WebServicesEnabled"];

                if (wsEnabled.IsAssigned())
                {
                    if (wsEnabled.Equals("1"))
                    {
                        return true;
                    }

                    if (wsEnabled.Equals("2"))
                    {
                        return false;
                    }
                }

                return false;
            }
		}

		public static BaseClient GetClient(string serviceName, string databaseName)
		{
			if (WsEnabled)
			{
				if (serviceName.Equals(string.Empty))
				{
					throw new Exception("WebServices is enabled but the webservice is not declared");
				}

				return new WsClient(serviceName, databaseName);
			}

			//return new SqlClient(databaseName);
		    return null;
		}
	}
}