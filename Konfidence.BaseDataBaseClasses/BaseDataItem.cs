using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.Sp;
using Konfidence.DataBaseInterface;

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

        private List<ISpParameterData> SpParameterData { get;  }

        public Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; }

        public string AutoIdField { get; set; } = string.Empty;

        public string GuidIdField { get; set; } = string.Empty;

        public Guid GuidIdValue { get; private set; } = Guid.Empty;

        public int Id { get; private set; }

        public void SetId(int id) => Id = id;

        public int GetId() => Id;

        public List<ISpParameterData> GetParameterObjects() => SpParameterData;

        protected BaseDataItem()
	    {
	        WithLanguage = false;
	        _isSelected = false;
	        _isEditing = false;
	        _isInitialized = false;

	        SpParameterData = new List<ISpParameterData>();
            AutoUpdateFieldDictionary = new Dictionary<string, ISpParameterData>();

            InternalInitializeDataItem();
        }

        // TODO: internal
        public IBaseClient Client
        {
            get
            {
                Debug.WriteLine($"get _client{GetType().FullName}");

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

        public void GetKey(IDataReader dataReader)
        {
            if (AutoIdField.Length > 0)
            {
                dataReader.GetField(AutoIdField,  out int id);

                Id = id;
            }
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
	    protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out byte field)
	    {
            dataReader.GetField(fieldName, out field);
	    }

        [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out short field)
        {
            dataReader.GetField(fieldName, out field);
        }

        [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out int field)
        {
            dataReader.GetField(fieldName, out field);
        }

        [UsedImplicitly]
	    protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out long field)
	    {
            dataReader.GetField(fieldName, out field);
	    }

        [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out decimal field)
        {
            dataReader.GetField(fieldName, out field);
        }

        [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out bool field)
        {
            dataReader.GetField(fieldName, out field);
        }

        [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out Guid field)
        {
            dataReader.GetField(fieldName, out field);

            if (fieldName.Equals(GuidIdField, StringComparison.InvariantCultureIgnoreCase))
            {
                GuidIdValue = field;
            }
        }

        [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out string field)
        {
            dataReader.GetField(fieldName, out field);
        }

        [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out DateTime field)
        {
            dataReader.GetField(fieldName, out field);
        }

	    [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, out TimeSpan field)
        {
            dataReader.GetField(fieldName, out field);
        }

	    [UsedImplicitly]
        protected void GetField([NotNull] string fieldName, [NotNull] IDataReader dataReader, [NotNull] ref XmlDocument field)
        {
            dataReader.GetField(fieldName, out string xmlString);

            field.LoadXml(xmlString);
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
            Client.GetItem(this);

            AfterGetDataItem();
        }

        protected void GetItemBy([NotNull] string storedProcedure)
        {
            Client.GetItemBy(this, storedProcedure);

            AfterGetDataItem();
        }

        protected void GetItem(int autoKeyId)
		{
            SetField(AutoIdField, autoKeyId);

            GetItem();
		}

	    [UsedImplicitly]
        protected void GetItem([NotNull] string guidStoredProcedure, Guid guidId)
        {
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
