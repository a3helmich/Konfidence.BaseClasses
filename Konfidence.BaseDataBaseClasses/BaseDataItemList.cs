using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.Sp;
using Konfidence.DataBaseInterface;
using Ninject;
using Ninject.Parameters;
using Serilog;

namespace Konfidence.BaseData
{
    [UsedImplicitly]
    public abstract class BaseDataItemList<T> : List<T>, IBaseDataItemList<T> where T : class, IBaseDataItem
    {
        private readonly List<ISpParameterData> _spParameterData;

        private IBaseClient _client;

        private NinjectDependencyResolver _ninject;

        [NotNull] public static object KernelLocker = new object();

        private IKernel Kernel
        {
            get
            {
                lock (KernelLocker)
                {
                    if (!_ninject.IsAssigned())
                    {
                        _ninject = new NinjectDependencyResolver();

                        if (!_ninject.Kernel.GetBindings(typeof(T)).Any())
                        {
                            Log.Debug($"Ninject Binding: ClientBind {typeof(T).FullName} - 33 - C:\\Projects\\Konfidence\\BaseClasses\\Konfidence.BaseDataBaseClasses\\BaseDataItemList.cs");

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

                if (!Kernel.GetBindings(typeof(TC)).Any())
                {
                    Log.Debug($"Ninject Binding: ClientBind {typeof(TC).FullName} - 45 - C:\\Projects\\Konfidence\\BaseClasses\\Konfidence.BaseDataBaseClasses\\BaseDataItemList.cs");

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

        //public BaseDataItemList()
        //{
        //    _spParameterData = new List<ISpParameterData>();
        //}

        //protected void BuildItemList(string getListStoredProcedure)
        //{
        //    Client.BuildItemList(this, getListStoredProcedure);

        //    AfterDataLoad();
        //}

        [NotNull]
        public T GetDataItem()
        {
            //var dataItem = new T();
            var dataItem = Kernel.Get<T>();

            Add(dataItem);

            return dataItem;
        }

        public List<ISpParameterData> GetParameterObjectList()
        {
            return _spParameterData;
        }

        protected void SetParameter<TP>(string fieldName, TP value)
        {
            _spParameterData.SetParameter(fieldName, value);
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