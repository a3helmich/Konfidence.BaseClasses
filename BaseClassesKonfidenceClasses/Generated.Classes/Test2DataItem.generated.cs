using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test2DataItem : BaseDataItem
		{
			// field definitions
			internal const string TEST2ID = "Test2Id";
			internal const string ID = "Id";
			internal const string SYSINSERTTIME = "SysInsertTime";
			internal const string SYSUPDATETIME = "SysUpdateTime";
			internal const string OMSCHRIJVING = "Omschrijving";
			internal const string SYSLOCK = "SysLock";
			internal const string NAAM = "Naam";
			
			// stored procedures
			private const string TEST2_GETROW = "gen_Test2_GetRow";
			private const string TEST2_SAVEROW = "gen_Test2_SaveRow";
			private const string TEST2_DELETEROW = "gen_Test2_DeleteRow";
			
			// property storage
			private Guid _Test2Id = Guid.Empty;
			private DateTime _SysInsertTime = DateTime.MinValue;
			private DateTime _SysUpdateTime = DateTime.MinValue;
			private string _Omschrijving = string.Empty;
			private string _SysLock = string.Empty;
			private string _Naam = string.Empty;
			
			#region generated properties
			
			public Guid Test2Id
			{
				get { return _Test2Id; }
				set { _Test2Id = value; }
			}
			
			public string Omschrijving
			{
				get { return _Omschrijving; }
				set { _Omschrijving = value; }
			}
			
			public string Naam
			{
				get { return _Naam; }
				set { _Naam = value; }
			}
			#endregion generated properties
			
			public Test2DataItem()
			{
			}
			
			public Test2DataItem(int Id) : this()
			{
				GetItem(TEST2_GETROW, Id);
			}
			
			protected override void InitializeDataItem()
			{
				AutoIdField = ID;
				
				DeleteStoredProcedure = TEST2_DELETEROW;
				SaveStoredProcedure = TEST2_SAVEROW;
			}
			
			protected override void GetData()
			{
				_Test2Id = GetFieldGuid(TEST2ID);
				_SysInsertTime = GetFieldDateTime(SYSINSERTTIME);
				_SysUpdateTime = GetFieldDateTime(SYSUPDATETIME);
				_Omschrijving = GetFieldString(OMSCHRIJVING);
				_SysLock = GetFieldString(SYSLOCK);
				_Naam = GetFieldString(NAAM);
			}
			
			protected override void SetData()
			{
				base.SetData();
				
				SetField(TEST2ID, _Test2Id);
				SetField(OMSCHRIJVING, _Omschrijving);
				SetField(NAAM, _Naam);
			}
		}
	}
}
