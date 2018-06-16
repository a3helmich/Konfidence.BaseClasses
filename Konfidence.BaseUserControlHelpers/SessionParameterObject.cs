using JetBrains.Annotations;

namespace Konfidence.BaseUserControlHelpers
{
    public class SessionParameterObject 
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