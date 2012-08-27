using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test1DataItemList : BaseDataItemList<Test1DataItem>
		{
			// partial methods
			partial void BeforeInitializeDataItemList();
			partial void AfterInitializeDataItemList();
			
			private const string TEST1_GETLIST = "gen_Test1_GetList";
			
			protected Test1DataItemList() : base()
			{
			}
			
			protected sealed override void InitializeDataItemList()
			{
				BeforeInitializeDataItemList();
				
				GetListStoredProcedure = TEST1_GETLIST;
				
				AfterInitializeDataItemList();
			}
			
			static public Test1DataItemList GetEmptyList()
			{
				Test1DataItemList test1List = new Test1DataItemList();
				
				return test1List;
			}
			
			static public Test1DataItemList GetList()
			{
				Test1DataItemList test1List = new Test1DataItemList();
				
				test1List.BuildItemList();
				
				return test1List;
			}
			
			public Test1DataItemList FindAll()
			{
				Test1DataItemList test1List = new Test1DataItemList();
				
				foreach (Test1DataItem test1 in this)
				{
					test1List.Add(test1);
				}
				
				return test1List;
			}
		}
	}
}
