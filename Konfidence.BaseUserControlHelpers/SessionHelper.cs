using System.Collections;
using System.Web;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
	public class SessionHelper 
	{
		public const string SESSION_ID_VALUE = "KIT_SESSIONID";
		public const string PAGE_ID_VALUE = "KIT_PAGEID";

		private const string PARAMETER_OBJECT_HASHTABLE_CLASS_TYPE = "ParameterObjectHashtable";

		private readonly HttpContext _context;
		private readonly string _controlName;

		public SessionHelper(HttpContext context, string controlName)
		{
			_context = context;
			_controlName = controlName;
		}

		[NotNull]
        private Hashtable ParameterObjectHashtable
		{
			get
			{
			    var parameterObjectHashtable = _context.Session[PARAMETER_OBJECT_HASHTABLE_CLASS_TYPE] as Hashtable;

				if (!parameterObjectHashtable.IsAssigned())
				{
					parameterObjectHashtable = new Hashtable();

					_context.Session[PARAMETER_OBJECT_HASHTABLE_CLASS_TYPE] = parameterObjectHashtable;
				}

				return parameterObjectHashtable;
			}
		}

		[NotNull]
        public SessionParameterObject SessionParameterObject
		{
			get
			{
			    var pageId = _context.Items[PAGE_ID_VALUE] as string;

				var sessionParameterObject = ParameterObjectHashtable[pageId + "_" + _controlName] as SessionParameterObject;

				if (!sessionParameterObject.IsAssigned())
				{
					sessionParameterObject = new SessionParameterObject();

					ParameterObjectHashtable[pageId + "_" + _controlName] = sessionParameterObject;
				}

				return sessionParameterObject;
			}
		}
	}
}