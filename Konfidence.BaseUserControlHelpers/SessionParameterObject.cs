using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    public class SessionParameterObject : BaseParameterObject
	{
		public int PrimaryKey { get; set; }

	    [UsedImplicitly]
	    public bool IsPrimaryKeyAssigned => PrimaryKey >= 1;

	    public int ForeignKey { get; set; }

	    [UsedImplicitly]
	    public bool IsForeignKeyAssigned => ForeignKey >= 1;

	    public string SessionTicket { get; set; }
	}
}