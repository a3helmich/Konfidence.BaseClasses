using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.Objects;
using Konfidence.DataBaseInterface;
using Ninject;
using Ninject.Parameters;

namespace Konfidence.BaseData
{
	public abstract class BaseDataItem: IBaseDataItem
	{
	    [UsedImplicitly]
        public const string BaseLanguage = "NL";

		public bool WithLanguage;

        private bool _isSelected;
        private bool _isEditing;
        private bool _isInitialized;

        private IBaseClient _client;

        private NinjectDependencyResolver _ninject;

        private List<ISpParameterData> SpParameterData { get;  }

        public Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; }

        protected string ConnectionName { get; set; } = string.Empty;

        public string AutoIdField { get; set; } = string.Empty;

        public string GuidIdField { get; set; } = string.Empty;

        public Guid GuidIdValue { get; private set; } = Guid.Empty;

        public int Id { get; private set; }

        protected BaseDataItem()
	    {
	        WithLanguage = false;
	        _isSelected = false;
	        _isEditing = false;
	        _isInitialized = false;

	        SpParameterData = new List<ISpParameterData>();
            AutoUpdateFieldDictionary = new Dictionary<string, ISpParameterData>();
        }

        private IKernel Kernel
	    {
	        get
	        {
	            if (!_ninject.IsAssigned())
	            {
	                _ninject = new NinjectDependencyResolver();
	            }

	            return _ninject.Kernel;
	        }
	    }

	    [NotNull]
        [UsedImplicitly]
        public virtual IBaseClient ClientBind<TC>() where TC : IBaseClient
	    {
            lock (BaseDataItemList<IBaseDataItem>.KernelLocker)
            {
                var databaseNameParam = new ConstructorArgument("connectionName", ConnectionName);

                if (!Kernel.GetBindings(typeof(TC)).Any())
                {
                    //Log.Debug($"Ninject Binding: ClientBind {typeof(TC).FullName} - 69 - TOCH NOOIT");
                    Kernel.Bind<IBaseClient>().To<TC>();
                }

                return Kernel.Get<TC>(databaseNameParam);
            }
        }

        protected abstract IBaseClient ClientBind();

	    // TODO: internal
	    public IBaseClient Client
	    {
	        get
	        {
                if (_client.IsAssigned())
                {
                    return _client;
                }

                Debug.WriteLine($"get _client{GetType().FullName}");

                _client = ClientBind();

                return _client;
	        }
	        set => _client = value;
	    }

        public bool IsSelected
        {
            get => _isSelected;
	        set
            {
                _isSelected = value;

                IsSelectedChanged();
            }
        }

	    protected virtual void IsSelectedChanged()
        {
            // nop
        }

        protected virtual void IsEditingChanged()
        {
            // nop
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                IsEditingChanged();
            }
        }

		public  void SetId(int id)
		{
			Id = id;
		}

        public List<ISpParameterData> GetParameterObjects()
        {
            return SpParameterData;
        }

        public void GetKey(IDataReader dataReader)
        {
            if (AutoIdField.Length > 0)
            {
                GetFieldInternal(AutoIdField, dataReader, out int id);

                Id = id;
            }
        }

        public int GetId()
        {
            return Id;
        }

        [UsedImplicitly]
	    protected internal void GetAutoUpdateField([NotNull] string fieldName, out byte fieldValue)
        {
            fieldValue = (byte) (GetAutoUpdateField(fieldName) ?? 0);
        }

	    [UsedImplicitly]
	    protected internal void GetAutoUpdateField([NotNull] string fieldName, out short fieldValue)
        {
            fieldValue = (short) (GetAutoUpdateField(fieldName) ?? 0);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out int fieldValue)
        {
            fieldValue = (int) (GetAutoUpdateField(fieldName) ?? 0);
        }

	    [UsedImplicitly]
	    protected internal void GetAutoUpdateField([NotNull] string fieldName, out long fieldValue)
        {
            fieldValue = (long) (GetAutoUpdateField(fieldName) ?? 0);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out Guid fieldValue)
        {
            fieldValue = (Guid) (GetAutoUpdateField(fieldName) ?? Guid.Empty);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, [CanBeNull] out string fieldValue)
        {
            fieldValue = (GetAutoUpdateField(fieldName) ?? string.Empty) as string;
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out bool fieldValue)
        {
            fieldValue = (bool) (GetAutoUpdateField(fieldName) ?? false);
        }

        [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out DateTime fieldValue)
        {
            fieldValue = (DateTime) (GetAutoUpdateField(fieldName) ?? DateTime.MinValue);
        }

        [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out TimeSpan fieldValue)
        {
            fieldValue = (TimeSpan) (GetAutoUpdateField(fieldName) ?? TimeSpan.MinValue);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out decimal fieldValue)
        {
            fieldValue = (decimal) (GetAutoUpdateField(fieldName) ?? 0);
        }

        [CanBeNull]
        private object GetAutoUpdateField([NotNull] string fieldName)
        {
            if (AutoUpdateFieldDictionary.TryGetValue(fieldName, out var parameterData) && parameterData.IsAssigned())
            {
                return parameterData.Value;
            }

            return null;
        }

        [UsedImplicitly]
        protected internal void AddAutoUpdateField([NotNull] string fieldName, DbType fieldType)
        {
            if (!AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                AutoUpdateFieldDictionary.Add(fieldName, new SpParameter(fieldName, fieldType, null));
            }
        }

	    public string GetStoredProcedure { get; set; } = string.Empty;

	    public string DeleteStoredProcedure { get; set; } = string.Empty;

	    public string SaveStoredProcedure { get; set; } = string.Empty;

        [UsedImplicitly]
	    public bool IsNew => Id == 0;

        [UsedImplicitly]
	    protected void GetField(string fieldName, IDataReader dataReader, out byte field)
	    {
	        GetFieldInternal(fieldName, dataReader, out field);
	    }

        [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, out short field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

        [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, out int field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

        [UsedImplicitly]
	    protected void GetField(string fieldName, IDataReader dataReader, out long field)
	    {
	        GetFieldInternal(fieldName, dataReader, out field);
	    }

	    [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, IDataReader dataReader, out Guid field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

        [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, out string field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

        [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, out bool field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

        [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, out DateTime field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

	    [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, out TimeSpan field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

	    [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, [NotNull] ref XmlDocument field)
        {
            GetFieldInternal(fieldName, dataReader, out string xmlString);

            field.LoadXml(xmlString);
        }

	    [UsedImplicitly]
        protected void GetField(string fieldName, IDataReader dataReader, out decimal field)
        {
            GetFieldInternal(fieldName, dataReader, out field);
        }

        private void GetFieldInternal(string fieldName, IDataReader dataReader, out byte field)
        {
            if (Client.IsAssigned())
            {
                Client.GetField(fieldName, out field, dataReader);

                return;
            }

            throw (new Exception("GetFieldInt8: client is not assigned"));
        }

	    private void GetFieldInternal(string fieldName, IDataReader dataReader, out short field)
	    {
	        if (Client.IsAssigned())
	        {
	            Client.GetField(fieldName, out field, dataReader);

                return;
            }

	        throw (new Exception("GetFieldInt16: client is not assigned"));
	    }

        private void GetFieldInternal(string fieldName, IDataReader dataReader, out int field)
		{
		    if (Client.IsAssigned())
		    {
		        Client.GetField(fieldName, out field, dataReader);

                return;
            }

		    throw (new Exception("GetFieldInt32: client is not assigned"));
		}

	    private void GetFieldInternal(string fieldName, IDataReader dataReader, out long field)
	    {
	        if (Client.IsAssigned())
	        {
	            Client.GetField(fieldName, out field, dataReader);

                return;
            }

	        throw (new Exception("GetFieldInt64: client is not assigned"));
	    }

        private void GetFieldInternal([NotNull] string fieldName, IDataReader dataReader, out Guid field)
        {
            if (Client.IsAssigned())
            {
                var fieldValue = Client.GetFieldGuid(fieldName, dataReader);

                if (fieldName.Equals(GuidIdField, StringComparison.InvariantCultureIgnoreCase))
                {
                    GuidIdValue = fieldValue;
                }

                field = fieldValue;

                return;
            }

            throw (new Exception("GetFieldGuid: client is not assigned"));
        }

		private void GetFieldInternal(string fieldName, IDataReader dataReader, out string field)
		{
            if (Client.IsAssigned())
		    {
		        field = Client.GetFieldString(fieldName, dataReader);

                return;
            }

		    throw (new Exception("GetFieldString: client is not assigned"));
		}

        private void GetFieldInternal(string fieldName, IDataReader dataReader, out bool field)
        {
            if (Client.IsAssigned())
            {
                field = Client.GetFieldBool(fieldName, dataReader);

                return;
            }

            throw (new Exception("GetFieldBool: client is not assigned"));
        }

		private void GetFieldInternal(string fieldName, IDataReader dataReader, out DateTime field)
		{
            if (Client.IsAssigned())
		    {
		        field = Client.GetFieldDateTime(fieldName, dataReader);

                return;
            }

		    throw (new Exception("GetFieldDateTime: client is not assigned"));
		}

        private void GetFieldInternal(string fieldName, IDataReader dataReader, out TimeSpan field)
        {
            if (Client.IsAssigned())
            {
                field = Client.GetFieldTimeSpan(fieldName, dataReader);

                return;
            }

            throw (new Exception("GetFieldTimeSpan: client is not assigned"));
        }

        private void GetFieldInternal(string fieldName, IDataReader dataReader, out decimal field)
        {
            if (Client.IsAssigned())
            {
                field = Client.GetFieldDecimal(fieldName, dataReader);

                return;
            }

            throw (new Exception("GetFieldDecimal: client is not assigned"));
        }

        protected void SetField(string fieldName, int value)
		{
		    SpParameterData.SetParameter(fieldName, value);
		}

	    protected void SetField(string fieldName, byte value)
	    {
	        SpParameterData.SetParameter(fieldName, value);
	    }

        protected void SetField(string fieldName, short value)
	    {
	        SpParameterData.SetParameter(fieldName, value);
	    }

	    protected void SetField(string fieldName, long value)
	    {
	        SpParameterData.SetParameter(fieldName, value);
	    }

        protected void SetField(string fieldName, Guid value)
        {
            SpParameterData.SetParameter(fieldName, value);
        }

        protected void SetField(string fieldName, string value)
		{
            SpParameterData.SetParameter(fieldName, value);
		}

		protected void SetField(string fieldName, bool value)
		{
            SpParameterData.SetParameter(fieldName, value);
		}

		protected void SetField(string fieldName, DateTime value)
		{
            SpParameterData.SetParameter(fieldName, value);
		}

        protected void SetField(string fieldName, TimeSpan value)
        {
            SpParameterData.SetParameter(fieldName, value);
        }

        protected void SetField(string fieldName, decimal value)
        {
            SpParameterData.SetParameter(fieldName, value);
        }

        [UsedImplicitly]
	    protected void SetParameter(string fieldName, byte value)
	    {
	        SetField(fieldName, value);
	    }

        [UsedImplicitly]
	    protected void SetParameter(string fieldName, int value)
		{
			SetField(fieldName, value);
		}

	    [UsedImplicitly]
	    protected void SetParameter(string fieldName, short value)
	    {
	        SetField(fieldName, value);
	    }

	    [UsedImplicitly]
	    protected void SetParameter(string fieldName, long value)
	    {
	        SetField(fieldName, value);
	    }

	    [UsedImplicitly]
	    protected void SetParameter(string fieldName, decimal value)
	    {
	        SetField(fieldName, value);
	    }

	    [UsedImplicitly]
        protected void SetParameter(string fieldName, Guid value)
        {
            SetField(fieldName, value);
        }

        [UsedImplicitly]
        protected void SetParameter(string fieldName, string value)
		{
			SetField(fieldName, value);
		}

	    [UsedImplicitly]
		protected void SetParameter(string fieldName, bool value)
		{
			SetField(fieldName, value);
		}

	    [UsedImplicitly]
		protected void SetParameter(string fieldName, DateTime value)
		{
			SetField(fieldName, value);
		}

	    [UsedImplicitly]
        protected void SetParameter(string fieldName, TimeSpan value)
        {
            SetField(fieldName, value);
        }

        public void LoadDataItem()
        {
            if (GetStoredProcedure.IsAssigned())
            {
                GetItem(Id);
            }
        }

        protected virtual void AfterGetDataItem()
        {
        }

	    private void InternalInitializeDataItem()
	    {
	        if (!_isInitialized)
	        {
	            InitializeDataItem();

	            _isInitialized = true;
	        }
	    }


        public virtual void InitializeDataItem()
		{
        }

        protected void GetItem()
		{
            InternalInitializeDataItem();

            Client.GetItem(this);

            AfterGetDataItem();
        }

        protected void GetItemBy([NotNull] string storedProcedure)
        {
            InternalInitializeDataItem();

            Client.GetItemBy(this, storedProcedure);

            AfterGetDataItem();
        }

        protected void GetItem(int autoKeyId)
		{
            InternalInitializeDataItem();

            SetField(AutoIdField, autoKeyId);

            GetItem();
		}

	    [UsedImplicitly]
        protected void GetItem([NotNull] string guidStoredProcedure, Guid guidId)
        {
            InternalInitializeDataItem();

            SetField(GuidIdField, guidId);

            GetItemBy(guidStoredProcedure);
        }

        protected virtual void BeforeSave()
        {
            // NOP
        }

        protected virtual void AfterSave()
        {
            // NOP
        }

		public void Save()
		{
            InternalInitializeDataItem();

            BeforeSave();

			if (!IsValidDataItem())
			{
				return;
			}

			Client.Save(this);

            GetAutoUpdateData();

            AfterSave();
		}

        protected virtual void BeforeDelete()
        {
            // NOP
        }

        protected virtual void AfterDelete()
        {
            // NOP
        }

		public void Delete()
		{
            InternalInitializeDataItem();

            BeforeDelete();

			Client.Delete(this);

			Id = 0;

            AfterDelete();
        }

        [NotNull]
        public List<ISpParameterData> SetItemData()
		{
			SetData();

            return SetParameterData();
		}

        [NotNull]
        internal List<ISpParameterData> SetParameterData()
        {
            var parameterObjectList = new List<ISpParameterData>();

            foreach (var parameterObject in SpParameterData)
            {
                parameterObjectList.Add(parameterObject);
            }

            SpParameterData.Clear();

            return parameterObjectList;
        }

        protected virtual void GetAutoUpdateData()
        {
        }

        public virtual void GetData(IDataReader dataReader)
		{
        }

        protected virtual void SetData()
		{
        }

        protected virtual bool IsValidDataItem()
		{
			return true;
		}
    }
}
