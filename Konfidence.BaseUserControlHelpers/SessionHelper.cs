using System.Collections;
using System.Web;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public class SessionHelper : BaseItem
	{
		public const string SESSION_ID_VALUE = "KIT_SESSIONID";
		public const string PAGE_ID_VALUE = "KIT_PAGEID";

		private const string PARAMETER_OBJECT_HASHTABLE_CLASS_TYPE = "ParameterObjectHashtable";

		private readonly HttpContext _Context;
		private readonly string _ControlName;

		public SessionHelper(HttpContext context, string controlName)
		{
			_Context = context;
			_ControlName = controlName;
		}

		private Hashtable ParameterObjectHashtable
		{
			get
			{
			    var parameterObjectHashtable = _Context.Session[PARAMETER_OBJECT_HASHTABLE_CLASS_TYPE] as Hashtable;

				if (!IsAssigned(parameterObjectHashtable))
				{
					parameterObjectHashtable = new Hashtable();

					_Context.Session[PARAMETER_OBJECT_HASHTABLE_CLASS_TYPE] = parameterObjectHashtable;
				}

				return parameterObjectHashtable;
			}
		}

		public SessionParameterObject SessionParameterObject
		{
			get
			{
			    var pageId = _Context.Items[PAGE_ID_VALUE] as string;

				var sessionParameterObject = ParameterObjectHashtable[pageId + "_" + _ControlName] as SessionParameterObject;

				if (!IsAssigned(sessionParameterObject))
				{
					sessionParameterObject = new SessionParameterObject();

					ParameterObjectHashtable[pageId + "_" + _ControlName] = sessionParameterObject;
				}

				return sessionParameterObject;
			}
		}
	}
}