using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test1DataItem : BaseDataItem
		{
			// field definitions
			internal const string TEST1ID = "Test1Id";
			internal const string ID = "Id";
			internal const string YEAR = "Year";
			internal const string SYSINSERTTIME = "SysInsertTime";
			internal const string SYSUPDATETIME = "SysUpdateTime";
			internal const string OMSCHRIJVING = "omschrijving";
			internal const string SYSLOCK = "SysLock";
			internal const string NAAM = "naam";
			
			// stored procedures
			private const string TEST1_GETROW = "gen_Test1_GetRow";
			private const string TEST1_SAVEROW = "gen_Test1_SaveRow";
			private const string TEST1_DELETEROW = "gen_Test1_DeleteRow";
			
			// property storage
			private Guid _Test1Id = Guid.Empty;
			private int _Year = 0;
			private DateTime _SysInsertTime = DateTime.MinValue;
			private DateTime _SysUpdateTime = DateTime.MinValue;
			private string _omschrijving = string.Empty;
			private string _SysLock = string.Empty;
			private string _naam = string.Empty;
			
			#region generated properties
			
			public Guid Test1Id
			{
				get { return _Test1Id; }
				set { _Test1Id = value; }
			}
			
			public int Year
			{
				get { return _Year; }
			}
			
			public string omschrijving
			{
				get { return _omschrijving; }
				set { _omschrijving = value; }
			}
			
			public string naam
			{
				get { return _naam; }
				set { _naam = value; }
			}
			#endregion generated properties
			
			public Test1DataItem()
			{
			}
			
			public Test1DataItem(int Id) : this()
			{
				GetItem(TEST1_GETROW, Id);
			}
			
			protected override void InitializeDataItem()
			{
				AutoIdField = ID;
				
				DeleteStoredProcedure = TEST1_DELETEROW;
				SaveStoredProcedure = TEST1_SAVEROW;
			}
			
			protected override void GetData()
			{
				_Test1Id = GetFieldGuid(TEST1ID);
				_Year = GetFieldInt32(YEAR);
				_SysInsertTime = GetFieldDateTime(SYSINSERTTIME);
				_SysUpdateTime = GetFieldDateTime(SYSUPDATETIME);
				_omschrijving = GetFieldString(OMSCHRIJVING);
				_SysLock = GetFieldString(SYSLOCK);
				_naam = GetFieldString(NAAM);
			}
			
			protected override void SetData()
			{
				base.SetData();
				
				SetField(TEST1ID, _Test1Id);
				SetField(OMSCHRIJVING, _omschrijving);
				SetField(NAAM, _naam);
			}
		}
	}
}
