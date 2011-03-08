using System.Collections;
using System.Web;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public class SessionHelper : BaseItem
	{
		public const string SessionIdValue = "KIT_SESSIONID";
		public const string PageIdValue = "KIT_PAGEID";

		private const string ParameterObjectHashtableClassType = "ParameterObjectHashtable";

		private HttpContext _Context;
		private string _ControlName;

		public SessionHelper(HttpContext context, string controlName)
		{
			_Context = context;
			_ControlName = controlName;
		}

		private Hashtable _ParameterObjectHashtable
		{
			get
			{
				Hashtable parameterObjectHashtable;

				parameterObjectHashtable = _Context.Session[ParameterObjectHashtableClassType] as Hashtable;

				if (!IsAssigned(parameterObjectHashtable))
				{
					parameterObjectHashtable = new Hashtable();

					_Context.Session[ParameterObjectHashtableClassType] = parameterObjectHashtable;
				}

				return parameterObjectHashtable;
			}
		}

		public SessionParameterObject SessionParameterObject
		{
			get
			{
				SessionParameterObject _SessionParameterObject;

				string PageId = _Context.Items[PageIdValue] as string;

				_SessionParameterObject = _ParameterObjectHashtable[PageId + "_" + _ControlName] as SessionParameterObject;

				if (!IsAssigned(_SessionParameterObject))
				{
					_SessionParameterObject = new SessionParameterObject();

					_ParameterObjectHashtable[PageId + "_" + _ControlName] = _SessionParameterObject;
				}

				return _SessionParameterObject;
			}
		}
	}
}