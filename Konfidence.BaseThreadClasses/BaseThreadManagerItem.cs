using System.Collections;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
	public class BaseThreadManagerItem: BaseItem
	{
		private static Hashtable _ThreadRunnerTable;

		private BaseThreadItem _ThreadItem = null;

		public BaseThreadManagerItem(BaseThreadParameterItem baseThreadParameterItem)
		{
			SetupThreadingEnvironment(baseThreadParameterItem);
		}

		protected BaseThreadItem ThreadItem
		{
			get
			{
				return _ThreadItem;
			}
		}

		protected void SetupThreadingEnvironment(BaseThreadParameterItem baseThreadParameterItem)
		{
			string threadRunnerId = baseThreadParameterItem + "-" + baseThreadParameterItem.ThreadRunnerId;

			if (!IsAssigned(_ThreadRunnerTable))
			{
				_ThreadRunnerTable = new Hashtable();
			}

			if (_ThreadRunnerTable.ContainsKey(threadRunnerId))
			{
				_ThreadItem = _ThreadRunnerTable[threadRunnerId] as BaseThreadItem;
			}
			else
			{
				_ThreadItem = GetNewThreadItem(baseThreadParameterItem);

				_ThreadRunnerTable.Add(threadRunnerId, ThreadItem);
			}
		}

		protected virtual BaseThreadItem GetNewThreadItem(BaseThreadParameterItem baseThreadParameterItem)
		{
			return null; // NOP
		}

		public bool IsInterrupted
		{
			get
			{
				if (IsAssigned(ThreadItem))
				{
					if (ThreadItem.IsInterrupted)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool IsStarted
		{
			get
			{
				return ThreadItem.IsStarted;
			}
		}

		public void StartExecuting()
		{
			if (!IsStarted || IsInterrupted)
			{
				ThreadItem.StartExecuting();
			}
		}

		public void StopExecuting()
		{
			if (IsAssigned(ThreadItem))
			{
				ThreadItem.PauseExecuting();
			}
		}
	}
}
