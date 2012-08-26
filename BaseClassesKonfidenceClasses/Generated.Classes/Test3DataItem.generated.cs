using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test3DataItem : BaseDataItem
		{
			// field definitions
			internal const string TEST3ID = "Test3Id";
			internal const string ID = "Id";
			internal const string SYSINSERTTIME = "SysInsertTime";
			internal const string SYSUPDATETIME = "SysUpdateTime";
			internal const string OMSCHRIJVING = "Omschrijving";
			internal const string SYSLOCK = "SysLock";
			internal const string NAAM = "Naam";
			
			// stored procedures
			private const string TEST3_GETROW = "gen_Test3_GetRow";
			private const string TEST3_SAVEROW = "gen_Test3_SaveRow";
			private const string TEST3_DELETEROW = "gen_Test3_DeleteRow";
			
			// property storage
			private Guid _Test3Id = Guid.Empty;
			private DateTime _SysInsertTime = DateTime.MinValue;
			private DateTime _SysUpdateTime = DateTime.MinValue;
			private string _Omschrijving = string.Empty;
			private string _SysLock = string.Empty;
			private string _Naam = string.Empty;
			
			#region generated properties
			
			public Guid Test3Id
			{
				get { return _Test3Id; }
				set { _Test3Id = value; }
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
			
			public Test3DataItem()
			{
			}
			
			public Test3DataItem(int Id) : this()
			{
				GetItem(TEST3_GETROW, Id);
			}
			
			protected override void InitializeDataItem()
			{
				AutoIdField = ID;
				
				DeleteStoredProcedure = TEST3_DELETEROW;
				SaveStoredProcedure = TEST3_SAVEROW;
			}
			
			protected override void GetData()
			{
				_Test3Id = GetFieldGuid(TEST3ID);
				_SysInsertTime = GetFieldDateTime(SYSINSERTTIME);
				_SysUpdateTime = GetFieldDateTime(SYSUPDATETIME);
				_Omschrijving = GetFieldString(OMSCHRIJVING);
				_SysLock = GetFieldString(SYSLOCK);
				_Naam = GetFieldString(NAAM);
			}
			
			protected override void SetData()
			{
				base.SetData();
				
				SetField(TEST3ID, _Test3Id);
				SetField(OMSCHRIJVING, _Omschrijving);
				SetField(NAAM, _Naam);
			}
		}
	}
}
