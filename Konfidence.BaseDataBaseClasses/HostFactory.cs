using System;
using System.Configuration;
using Konfidence.Base;

namespace Konfidence.BaseData
{
	internal class HostFactory : BaseItem
	{
		private static bool WSEnabled
		{
			get
			{
				string wsEnabled = ConfigurationManager.AppSettings["WebServicesEnabled"];

                if (!IsEmpty(wsEnabled))
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

		public static BaseHost GetHost(string serviceName, string databaseName)
		{
			if (WSEnabled)
			{
				if (serviceName.Equals(string.Empty))
				{
					throw new Exception("WS is enabled but the webservice is not declared");
				}

				return new WSHost(serviceName, databaseName);
			}

			return new SQLHost(databaseName);
		}
	}
}