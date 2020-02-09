using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseDatabaseClasses.Objects;
using Konfidence.DataBaseInterface;
using Ninject;
using Ninject.Parameters;
using Serilog;

namespace Konfidence.BaseData
{
    [UsedImplicitly]
    public abstract class BaseDataItemList<T> : List<T>, IBaseDataItemList<T> where T : class, IBaseDataItem
    {
        private readonly List<IDbParameterObject> _dbParameterObjects;

	    private IBaseClient _client;

        private NinjectDependencyResolver _ninject;

        public static object KernelLocker = new object();

        private IKernel Kernel
        {
            get
            {
                lock (KernelLocker)
                {
                    if (!_ninject.IsAssigned())
                    {
                        _ninject = new NinjectDependencyResolver();

                        Log.Information($"Ninject Binding: ClientBind start");

                        if (!_ninject.Kernel.GetBindings(typeof(T)).Any())
                        {
                            Log.Information($"Ninject Binding: ClientBind {typeof(T).FullName} - 33 - C:\\Projects\\Konfidence\\BaseClasses\\Konfidence.BaseDataBaseClasses\\BaseDataItemList.cs");

                            _ninject.Kernel.Bind<T>().To<T>();
                        }

                    }

                    return _ninject.Kernel;
                }
            }
        }

        [NotNull]
        [UsedImplicitly]
        public virtual IBaseClient ClientBind<TC>() where TC : IBaseClient
        {
            lock (BaseDataItemList<IBaseDataItem>.KernelLocker)
            {
                var connectionNameParam = new ConstructorArgument("connectionName", ConnectionName);

                Log.Information($"Ninject Binding: ClientBind start");

                if (!Kernel.GetBindings(typeof(TC)).Any())
                {
                    Log.Information(
                        $"Ninject Binding: ClientBind {typeof(TC).FullName} - 45 - C:\\Projects\\Konfidence\\BaseClasses\\Konfidence.BaseDataBaseClasses\\BaseDataItemList.cs");

                    Kernel.Bind<IBaseClient>().To<TC>();
                }

                return Kernel.Get<TC>(connectionNameParam);
            }
        }

        protected abstract IBaseClient ClientBind();

        public IBaseClient Client
	    {
	        get
	        {
	            if (!_client.IsAssigned())
	            {
	                _client = ClientBind();
	            }

	            return _client;
	        }
	        set => _client = value;
	    }

        protected virtual void AfterDataLoad()
        {
            //
        }

		protected string ConnectionName { get; set; } = string.Empty;

        protected string GetListStoredProcedure { get; private set; } = string.Empty;

        [UsedImplicitly]
        protected string ServiceName { get; set; } = string.Empty;

	    public BaseDataItemList()
	    {
	        _dbParameterObjects = new List<IDbParameterObject>();
        }

        protected void BuildItemList(string getListStoredProcedure)
		{
		    GetListStoredProcedure = getListStoredProcedure;

		    Client.BuildItemList(this, getListStoredProcedure);

            AfterDataLoad();
        }

        [UsedImplicitly]
		protected void RebuildItemList()
		{
			Clear();

            BuildItemList(GetListStoredProcedure);
		}

		[NotNull]
        public T GetDataItem()
		{
            //var baseDataItem = new T();
            var baseDataItem = Kernel.Get<T>();

            baseDataItem.InitializeDataItem();

			Add(baseDataItem);

			return baseDataItem;
		}

        [UsedImplicitly]
        [CanBeNull]
        public T FindById(string textId)
        {
            if (Guid.TryParse(textId, out var guidId))
            {
                return FindById(guidId);
            }

            if (int.TryParse(textId, out var id))
            {
                // TODO : remove comment start
                //if (Debugger.IsAttached || BaseItem.UnitTest)
                //{
                //    throw new Exception("id is niet toegestaan om records op te halen(BaseDataItemList.FindById(..))");
                //}
                // TODO : remove comment end

                return FindById(id);
            }

            return default(T);
        }

        [CanBeNull]
        public T FindById(int id)
        {
            return this.FirstOrDefault(x => x.GetId() == id);
        }

        [CanBeNull]
        public T FindById(Guid guidId)
        {
            return this.FirstOrDefault(x => x.GuidIdValue == guidId);
        }

        [CanBeNull]
        private T FindByIsSelected()
        {
            var firstSelected = this.FirstOrDefault(x => x.IsSelected);

            if (firstSelected.IsAssigned())
            {
                return firstSelected;
            }

            // if none selected, make first selected
            if (this.Any())
            {
                var first = this.First();

                first.IsSelected = true;

                return first;
            }

            return default(T);
        }

        [CanBeNull]
        protected T FindByIsEditing()
        {
            return this.FirstOrDefault(x => x.IsEditing);
        }

        [CanBeNull]
        public T FindCurrent()
        {
            var dataItem = FindByIsEditing();

            if (dataItem.IsAssigned())
            {
                return dataItem;
            }

            return FindByIsSelected(); 
        }

        [UsedImplicitly]
        public bool HasCurrent
        {
            get
            {
                return this.Any(x => x.IsEditing || x.IsSelected);
            }
        }

        #region list selecting state control

        [UsedImplicitly]
        public void SetSelected(string idText, string isEditingText)
        {
            bool.TryParse(isEditingText, out var isEditing);

            if (Guid.TryParse(idText, out var guidId))
            {
                SetSelected(guidId, isEditing);
            }
            else
            {

                if (int.TryParse(idText, out var id))
                {
                    // TODO : remove comment start
                    //if (Debugger.IsAttached || BaseItem.UnitTest)
                    //{
                    //    throw new Exception("id is niet toegestaan om reocrds op te halen(BaseDataItemList.SetSelected(..))");
                    //}
                    // TODO : remove comment end

                    SetSelected(id, isEditing);
                }
            }
        }

        [UsedImplicitly]
        public void SetSelected(string idText)
        {
            if (Guid.TryParse(idText, out var guidId))
            {
                SetSelected(guidId, false);
            }
            else
            {
                if (int.TryParse(idText, out var id))
                {
                    // TODO : remove comment start
                    //if (Debugger.IsAttached || BaseItem.UnitTest)
                    //{
                    //    throw new Exception("id is niet toegestaan om reocrds op te halen(BaseDataItemList.SetSelected(..))");
                    //}
                    // TODO : remove comment end

                    SetSelected(id);
                }
            }
        }

        [UsedImplicitly]
        public void SetSelected(BaseDataItem dataItem)
        {
            if (dataItem.IsAssigned())
            {
                SetSelected(dataItem.GetId());
            }
        }

        private void SetSelected(int id, bool isEditing = false)
        {
            SetSelected(id, Guid.Empty, isEditing);
        }
        
        private void SetSelected(Guid guidId, bool isEditing)
        {
            SetSelected(0, guidId, isEditing);
        }

        private void SetSelected(int id, Guid guidId, bool isEditing)
        {
            if (Count > 0)
            {
                foreach (var dataItem in this)
                {
                    dataItem.IsSelected = false;
                }

                if (id < 1 && !guidId.IsAssigned())
                {
                    this[0].IsSelected = true;
                }
                else
                {
                    T dataItem;

                    dataItem = id > 0 ? FindById(id) : FindById(guidId);

                    if (dataItem.IsAssigned())
                    {
                        dataItem.IsSelected = true;
                        dataItem.IsEditing = isEditing;
                    }
                }
            }
        }
        #endregion list selecting state control

        #region list editing state control

        [UsedImplicitly]
        public void New()
        {
            var dataItem = FindCurrent();

            if (dataItem.IsAssigned())
            {
                dataItem.IsSelected = false;
                dataItem.IsEditing = true; // nieuw
            }
        }

        [UsedImplicitly]
        public void Edit(T dataItem)
        {
            if (dataItem.IsAssigned())
            {
                dataItem.IsEditing = true; // nieuw
            }
        }

        [UsedImplicitly]
        public void Save(T dataItem)
        {
            if (dataItem.IsAssigned())
            {
                dataItem.Save();

                SetSelected(dataItem.GetId());

                dataItem.IsEditing = false; // nieuw
            }
        }

        [UsedImplicitly]
        public void Cancel([NotNull] T dataItem)
        {
            dataItem.LoadDataItem();

            dataItem.IsEditing = false;
        }

        [UsedImplicitly]
        public void Delete(T dataItem)
        {
            if (dataItem.IsAssigned())
            {
                dataItem.Delete();

                var selectedIndex = IndexOf(dataItem);

                Remove(dataItem);

                if (Count > 0)
                {
                    if (selectedIndex < Count)
                    {
                        this[selectedIndex].IsSelected = true;
                    }
                    else
                    {
                        this[selectedIndex - 1].IsSelected = true;
                    }
                }
            }
        }
        #endregion list editing

        [NotNull]
        private static List<IDbParameterObject> GetProperties([NotNull] IBaseDataItem baseDataItem)
		{
            var properties = new List<IDbParameterObject>();

			baseDataItem.GetProperties(properties);

			return properties;
		}

        public List<IDbParameterObject> GetParameterObjectList()
        {
            return _dbParameterObjects;
        }

        protected void SetParameter<TP>(string fieldName, TP value)
        {
            _dbParameterObjects.SetParameter(fieldName, value);
        }

		/// <summary>
		/// Add parameters for filtering
		/// </summary>
		/// <returns></returns>
		public virtual void SetParameters(string storedProcedure)
		{
			// NOP
		}

        [UsedImplicitly]
		protected int ExecuteTextCommand(string textCommand)
		{
			return Client.ExecuteTextCommand(textCommand);
		}

        [UsedImplicitly]
		protected bool TableExists(string tableName)
		{
			return Client.TableExists(tableName);
		}

        [UsedImplicitly]
		protected bool ViewExists(string viewName)
		{
			return Client.ViewExists(viewName);
		}

        [UsedImplicitly]
        protected bool StoredProcedureExists(string storedProcedureName)
        {
            return Client.StoredProcedureExists(storedProcedureName);
        }
    }
}