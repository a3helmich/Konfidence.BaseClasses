using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test3DataItemList : BaseDataItemList<Test3DataItem>
		{
			// partial methods
			partial void BeforeInitializeDataItemList();
			partial void AfterInitializeDataItemList();
			
			private const string TEST3_GETLIST = "gen_Test3_GetList";
			
			protected Test3DataItemList() : base()
			{
			}
			
			protected sealed override void InitializeDataItemList()
			{
				BeforeInitializeDataItemList();
				
				GetListStoredProcedure = TEST3_GETLIST;
				
				AfterInitializeDataItemList();
			}
			
			static public Test3DataItemList GetEmptyList()
			{
				Test3DataItemList test3List = new Test3DataItemList();
				
				return test3List;
			}
			
			static public Test3DataItemList GetList()
			{
				Test3DataItemList test3List = new Test3DataItemList();
				
				test3List.BuildItemList();
				
				return test3List;
			}
			
			public Test3DataItemList FindAll()
			{
				Test3DataItemList test3List = new Test3DataItemList();
				
				foreach (Test3DataItem test3 in this)
				{
					test3List.Add(test3);
				}
				
				return test3List;
			}
		}
	}
}
