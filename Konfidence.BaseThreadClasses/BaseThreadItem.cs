using System.Threading;
using Konfidence.Base;

namespace Konfidence.BaseThreadClasses
{
	public class BaseThreadItem: BaseItem
	{
		private Thread RunningThread = null;

		private bool _Started = false;
		private bool _Finished = true;
		private bool _Paused = false;

		public BaseThreadItem()
		{
			_Finished = false;
		}

		#region properties
		protected bool Started
		{
			get
			{
				return _Started;
			}
		}

		protected bool Finished
		{
			get
			{
				return _Finished;
			}
		}

		protected bool Paused
		{
			get
			{
				return _Paused;
			}
		}

		public bool IsStarted
		{
			get
			{
				return Started;
			}
		}

		public bool IsInterrupted  // naamgeving klopt niet: doet niet wat ie zegt!
		{
			get
			{
				if (Started && Paused)
				{
					if (!Finished)
					{
						return true;
					}
				}
				return false;
			}
		}
		#endregion

		protected virtual void ExecuteAction()
		{
			// NOP	
		}

		protected virtual void BeforeStartExecuting()
		{
			// NOP
		}

		public void StartExecuting()
		{
			ThreadStart threadStart = new ThreadStart(ExecuteAction);
			RunningThread = new Thread(threadStart);

			BeforeStartExecuting();

			_Finished = false; // weer op false zetten voor het geval dat het een pause/restart is.
			_Started = true;

			RunningThread.Start();
		}

		public void PauseExecuting()
		{
			_Paused = true;
		}

		protected bool IsPaused()
		{
			if (_Paused)
			{
				_Started = false;

				return true;
			}
			return false;
		}

		protected void FinishExecuting()
		{
			_Finished = true;
			_Started = false;
		}
	}
}
