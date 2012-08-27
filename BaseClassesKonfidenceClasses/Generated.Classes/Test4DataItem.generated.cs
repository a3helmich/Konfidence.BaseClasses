using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test4DataItem : BaseDataItem
		{
			// field definitions
			internal const string TEST4ID = "Test4Id";
			internal const string ID = "Id";
			internal const string SYSINSERTTIME = "SysInsertTime";
			internal const string SYSUPDATETIME = "SysUpdateTime";
			internal const string OMSCHRIJVING = "Omschrijving";
			internal const string SYSLOCK = "SysLock";
			internal const string NAAM = "Naam";
			
			// stored procedures
			private const string TEST4_GETROW = "gen_Test4_GetRow";
			private const string TEST4_SAVEROW = "gen_Test4_SaveRow";
			private const string TEST4_DELETEROW = "gen_Test4_DeleteRow";
			
			// property storage
			private Guid _Test4Id = Guid.Empty;
			private DateTime _SysInsertTime = DateTime.MinValue;
			private DateTime _SysUpdateTime = DateTime.MinValue;
			private string _Omschrijving = string.Empty;
			private string _SysLock = string.Empty;
			private string _Naam = string.Empty;
			
			#region generated properties
			
			public Guid Test4Id
			{
				get { return _Test4Id; }
				set { _Test4Id = value; }
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
			
			public Test4DataItem()
			{
			}
			
			public Test4DataItem(int Id) : this()
			{
				GetItem(TEST4_GETROW, Id);
			}
			
			protected override void InitializeDataItem()
			{
				AutoIdField = ID;
				
				DeleteStoredProcedure = TEST4_DELETEROW;
				SaveStoredProcedure = TEST4_SAVEROW;
			}
			
			protected override void GetData()
			{
				_Test4Id = GetFieldGuid(TEST4ID);
				_SysInsertTime = GetFieldDateTime(SYSINSERTTIME);
				_SysUpdateTime = GetFieldDateTime(SYSUPDATETIME);
				_Omschrijving = GetFieldString(OMSCHRIJVING);
				_SysLock = GetFieldString(SYSLOCK);
				_Naam = GetFieldString(NAAM);
			}
			
			protected override void SetData()
			{
				base.SetData();
				
				SetField(TEST4ID, _Test4Id);
				SetField(OMSCHRIJVING, _Omschrijving);
				SetField(NAAM, _Naam);
			}
		}
	}
}
