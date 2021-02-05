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

        private List<IDbParameterData> DbParameterObjects { get;  }

        public Dictionary<string, IDbParameterData> AutoUpdateFieldDictionary { get; }

        protected BaseDataItem()
	    {
	        WithLanguage = false;
	        _isSelected = false;
	        _isEditing = false;
	        _isInitialized = false;

	        DbParameterObjects = new List<IDbParameterData>();
            AutoUpdateFieldDictionary = new Dictionary<string, IDbParameterData>();
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

        public List<IDbParameterData> GetParameterObjects()
        {
            return DbParameterObjects;
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

        protected string ConnectionName { get; set; } = string.Empty;

        [UsedImplicitly]
	    protected string ServiceName { get; set; } = string.Empty;

	    public string AutoIdField { get; set; } = string.Empty;

	    protected internal string GuidIdField { get; set; } = string.Empty;

	    public Guid GuidIdValue { get; private set; } = Guid.Empty;

	    protected int Id { get; private set; }

        [UsedImplicitly]
	    protected internal void GetAutoUpdateField([NotNull] string fieldName, out byte fieldValue)
	    {
	        fieldValue = GetAutoUpdateFieldInt8(fieldName);
	    }

	    [UsedImplicitly]
	    protected internal void GetAutoUpdateField([NotNull] string fieldName, out short fieldValue)
	    {
	        fieldValue = GetAutoUpdateFieldInt16(fieldName);
	    }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out int fieldValue)
        {
            fieldValue = GetAutoUpdateFieldInt32(fieldName);
        }

	    [UsedImplicitly]
	    protected internal void GetAutoUpdateField([NotNull] string fieldName, out long fieldValue)
	    {
	        fieldValue = GetAutoUpdateFieldInt64(fieldName);
	    }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out Guid fieldValue)
        {
            fieldValue = GetAutoUpdateFieldGuid(fieldName);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, [CanBeNull] out string fieldValue)
        {
            fieldValue = GetAutoUpdateFieldString(fieldName);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out bool fieldValue)
        {
            fieldValue = GetAutoUpdateFieldBool(fieldName);
        }

        [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out DateTime fieldValue)
        {
            fieldValue = GetAutoUpdateFieldDateTime(fieldName);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out TimeSpan fieldValue)
        {
            fieldValue = GetAutoUpdateFieldTimeSpan(fieldName);
        }

	    [UsedImplicitly]
        protected internal void GetAutoUpdateField([NotNull] string fieldName, out decimal fieldValue)
        {
            fieldValue = GetAutoUpdateFieldDecimal(fieldName);
        }

        private byte GetAutoUpdateFieldInt8([NotNull] string fieldName)
        {
            byte fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (byte)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }
            
            return fieldValue;
        }

	    private short GetAutoUpdateFieldInt16([NotNull] string fieldName)
	    {
	        short fieldValue = 0;

	        if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
	        {
	            if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
	            {
	                fieldValue = (short)AutoUpdateFieldDictionary[fieldName].Value;
	            }
	        }

	        return fieldValue;
	    }

        private int GetAutoUpdateFieldInt32([NotNull] string fieldName)
        {
            var fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (int)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }
            
            return fieldValue;
        }

	    private long GetAutoUpdateFieldInt64([NotNull] string fieldName)
	    {
	        long fieldValue = 0;

	        if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
	        {
	            if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
	            {
	                fieldValue = (long)AutoUpdateFieldDictionary[fieldName].Value;
	            }
	        }

	        return fieldValue;
	    }

        private Guid GetAutoUpdateFieldGuid([NotNull] string fieldName)
        {
            var fieldValue = Guid.Empty;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (Guid)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        [CanBeNull]
        private string GetAutoUpdateFieldString([NotNull] string fieldName)
        {
            var fieldValue = string.Empty;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = AutoUpdateFieldDictionary[fieldName].Value as string;
                }
            }

            return fieldValue;
        }

        private bool GetAutoUpdateFieldBool([NotNull] string fieldName)
        {
            var fieldValue = false;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (bool)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private DateTime GetAutoUpdateFieldDateTime([NotNull] string fieldName)
        {
            var fieldValue = DateTime.MinValue;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (DateTime)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private TimeSpan GetAutoUpdateFieldTimeSpan([NotNull] string fieldName)
        {
            var fieldValue = TimeSpan.MinValue;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (TimeSpan)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private decimal GetAutoUpdateFieldDecimal([NotNull] string fieldName)
        {
            decimal fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (decimal)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        [UsedImplicitly]
        protected internal void AddAutoUpdateField([NotNull] string fieldName, DbType fieldType)
        {
            if (!AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                AutoUpdateFieldDictionary.Add(fieldName, new DbParameterData(fieldName, fieldType, null));
            }
        }

	    public string GetStoredProcedure { get; set; } = string.Empty;

	    public string DeleteStoredProcedure { get; set; } = string.Empty;

	    public string SaveStoredProcedure { get; set; } = string.Empty;

        [UsedImplicitly]
	    public bool IsNew
		{
			get
			{
				if (Id == 0)
				{
					return true;
				}
				return false;
			}
		}

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
                field = Client.GetFieldInt8(fieldName, dataReader);

                return;
            }

            throw (new Exception("GetFieldInt8: client is not assigned"));
        }

	    private void GetFieldInternal(string fieldName, IDataReader dataReader, out short field)
	    {
	        if (Client.IsAssigned())
	        {
	            field = Client.GetFieldInt16(fieldName, dataReader);

                return;
            }

	        throw (new Exception("GetFieldInt16: client is not assigned"));
	    }

        private void GetFieldInternal(string fieldName, IDataReader dataReader, out int field)
		{
		    if (Client.IsAssigned())
		    {
		        field = Client.GetFieldInt32(fieldName, dataReader);

                return;
            }

		    throw (new Exception("GetFieldInt32: client is not assigned"));
		}

	    private void GetFieldInternal(string fieldName, IDataReader dataReader, out long field)
	    {
	        if (Client.IsAssigned())
	        {
	            field =  Client.GetFieldInt64(fieldName, dataReader);

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
		    DbParameterObjects.SetParameter(fieldName, value);
		}

	    protected void SetField(string fieldName, byte value)
	    {
	        DbParameterObjects.SetParameter(fieldName, value);
	    }

        protected void SetField(string fieldName, short value)
	    {
	        DbParameterObjects.SetParameter(fieldName, value);
	    }

	    protected void SetField(string fieldName, long value)
	    {
	        DbParameterObjects.SetParameter(fieldName, value);
	    }

        protected void SetField(string fieldName, Guid value)
        {
            DbParameterObjects.SetParameter(fieldName, value);
        }

        protected void SetField(string fieldName, string value)
		{
            DbParameterObjects.SetParameter(fieldName, value);
		}

		protected void SetField(string fieldName, bool value)
		{
            DbParameterObjects.SetParameter(fieldName, value);
		}

		protected void SetField(string fieldName, DateTime value)
		{
            DbParameterObjects.SetParameter(fieldName, value);
		}

        protected void SetField(string fieldName, TimeSpan value)
        {
            DbParameterObjects.SetParameter(fieldName, value);
        }

        protected void SetField(string fieldName, decimal value)
        {
            DbParameterObjects.SetParameter(fieldName, value);
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
        public List<IDbParameterData> SetItemData()
		{
			SetData();

            return SetParameterData();
		}

        [NotNull]
        internal List<IDbParameterData> SetParameterData()
        {
            var parameterObjectList = new List<IDbParameterData>();

            foreach (var parameterObject in DbParameterObjects)
            {
                parameterObjectList.Add(parameterObject);
            }

            DbParameterObjects.Clear();

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
