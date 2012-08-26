using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test2DataItemList : BaseDataItemList<Test2DataItem>
		{
			// partial methods
			partial void BeforeInitializeDataItemList();
			partial void AfterInitializeDataItemList();
			
			private const string TEST2_GETLIST = "gen_Test2_GetList";
			
			protected Test2DataItemList() : base()
			{
			}
			
			protected sealed override void InitializeDataItemList()
			{
				BeforeInitializeDataItemList();
				
				GetListStoredProcedure = TEST2_GETLIST;
				
				AfterInitializeDataItemList();
			}
			
			static public Test2DataItemList GetEmptyList()
			{
				Test2DataItemList test2List = new Test2DataItemList();
				
				return test2List;
			}
			
			static public Test2DataItemList GetList()
			{
				Test2DataItemList test2List = new Test2DataItemList();
				
				test2List.BuildItemList();
				
				return test2List;
			}
			
			public Test2DataItemList FindAll()
			{
				Test2DataItemList test2List = new Test2DataItemList();
				
				foreach (Test2DataItem test2 in this)
				{
					test2List.Add(test2);
				}
				
				return test2List;
			}
		}
	}
}
