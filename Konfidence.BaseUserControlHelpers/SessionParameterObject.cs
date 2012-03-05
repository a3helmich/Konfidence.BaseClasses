using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    public class SessionParameterObject : BaseParameterObject
	{
		private int _PrimaryKey; // default value 0 
		private int _ForeignKey; // default value 0
		private string _SessionTicket; // default value 0
		
		#region properties
		public int PrimaryKey
		{
			get
			{
				return _PrimaryKey;
			}
			set
			{
				_PrimaryKey = value;
			}
		}

		public bool IsPrimaryKeyAssigned
		{
			get
			{
				if (_PrimaryKey < 1)
					return false;

				return true;
			}
		}

		public int ForeignKey
		{
			get
			{
				return _ForeignKey;
			}
			set
			{
				_ForeignKey = value;
			}
		}

		public bool IsForeignKeyAssigned
		{
			get
			{
				if (_ForeignKey < 1)
					return false;

				return true;
			}
		}

		public string SessionTicket
		{
			get
			{
				return _SessionTicket;
			}
			set
			{
				_SessionTicket = value;
			}
		}

		#endregion
	}
}