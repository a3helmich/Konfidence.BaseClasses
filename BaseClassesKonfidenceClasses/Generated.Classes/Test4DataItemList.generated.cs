using System;
using Konfidence.BaseData;

namespace Konfidence.TestClassGeneratorClasses
{
	public partial class Bl
	{
		public partial class Test4DataItemList : BaseDataItemList<Test4DataItem>
		{
			// partial methods
			partial void BeforeInitializeDataItemList();
			partial void AfterInitializeDataItemList();
			
			private const string TEST4_GETLIST = "gen_Test4_GetList";
			
			protected Test4DataItemList() : base()
			{
			}
			
			protected sealed override void InitializeDataItemList()
			{
				BeforeInitializeDataItemList();
				
				GetListStoredProcedure = TEST4_GETLIST;
				
				AfterInitializeDataItemList();
			}
			
			static public Test4DataItemList GetEmptyList()
			{
				Test4DataItemList test4List = new Test4DataItemList();
				
				return test4List;
			}
			
			static public Test4DataItemList GetList()
			{
				Test4DataItemList test4List = new Test4DataItemList();
				
				test4List.BuildItemList();
				
				return test4List;
			}
			
			public Test4DataItemList FindAll()
			{
				Test4DataItemList test4List = new Test4DataItemList();
				
				foreach (Test4DataItem test4 in this)
				{
					test4List.Add(test4);
				}
				
				return test4List;
			}
		}
	}
}
