using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using Konfidence.Base;
using Konfidence.BaseData.Objects;
using Konfidence.BaseDataInterfaces;
using Ninject;

namespace Konfidence.BaseData
{
	public class BaseDataItem: BaseItem, IBaseDataItem
	{
        public const string BaseLanguage = "NL";

		public bool WithLanguage = false;

        private bool _isSelected;
        private bool _isEditing;
        private bool _isInitialized;

        private IBaseClient _client; 
		internal Dictionary<string, object> PropertyDictionary;

	    private Dictionary<string, IDbParameterObject> _autoUpdateFieldDictionary;

	    private NinjectDependencyResolver _ninject;

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
	    protected virtual IBaseClient ClientBind<TC>() where TC : IBaseClient
	    {
	        if (!Kernel.GetBindings(typeof(TC)).Any())
	        {
	            Kernel.Bind<IBaseClient>().To<TC>();
	        }

	        return Kernel.Get<TC>();
	    }

	    // TODO: internal
	    public IBaseClient Client
	    {
	        get
	        {
	            if (!_client.IsAssigned())
	            {
	                _client = ClientBind<IBaseClient>();

                    //_client = ClientFactory.GetClient(ServiceName, DatabaseName);
	            }

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

        protected IDbParameterObjectList DbParameterObjectList { get; private set; } = new DbParameterObjectList();

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

        public BaseDataItem()
	    {
		    _isSelected = false;
		    _isEditing = false;
	        _isInitialized = false;
	    }

		public  void SetId(int id)
		{
			Id = id;
		}

		public void SetProperties(Dictionary<string, object> propertyDictionary)
		{
			PropertyDictionary = propertyDictionary;

			GetData();

			PropertyDictionary = null;
		}

        public void GetProperties(IDbParameterObjectList properties)
		{
			DbParameterObjectList = properties;

			SetData();

			DbParameterObjectList = null;
		}

        public IDbParameterObjectList GetParameterObjectList()
        {
            return DbParameterObjectList;
        }

        public void GetKey()
        {
            if (AutoIdField.Length > 0)
            {
                Id = GetFieldInt32(AutoIdField);
            }
        }

        public int GetId()
        {
            return Id;
        }

        #region properties
        protected string DatabaseName { get; set; } = string.Empty;

	    protected string ServiceName { get; set; } = string.Empty;

	    public string AutoIdField { get; set; } = string.Empty;

	    protected internal string GuidIdField { get; set; } = string.Empty;

	    public Guid GuidIdValue { get; private set; } = Guid.Empty;

	    protected int Id { get; private set; }

	    public Dictionary<string, IDbParameterObject> AutoUpdateFieldDictionary
        {
            get
            {
                if (!_autoUpdateFieldDictionary.IsAssigned())
                {
                    _autoUpdateFieldDictionary = new Dictionary<string, IDbParameterObject>();
                }

                return _autoUpdateFieldDictionary;
            }
        }

        protected internal void GetAutoUpdateField(string fieldName, out short fieldValue)
        {
            fieldValue = GetAutoUpdateFieldInt16(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out int fieldValue)
        {
            fieldValue = GetAutoUpdateFieldInt32(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out Guid fieldValue)
        {
            fieldValue = GetAutoUpdateFieldGuid(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out string fieldValue)
        {
            fieldValue = GetAutoUpdateFieldString(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out bool fieldValue)
        {
            fieldValue = GetAutoUpdateFieldBool(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out DateTime fieldValue)
        {
            fieldValue = GetAutoUpdateFieldDateTime(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out TimeSpan fieldValue)
        {
            fieldValue = GetAutoUpdateFieldTimeSpan(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out decimal fieldValue)
        {
            fieldValue = GetAutoUpdateFieldDecimal(fieldName);
        }

        private short GetAutoUpdateFieldInt16(string fieldName)
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

        private int GetAutoUpdateFieldInt32(string fieldName)
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

        private Guid GetAutoUpdateFieldGuid(string fieldName)
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

        private string GetAutoUpdateFieldString(string fieldName)
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

        private bool GetAutoUpdateFieldBool(string fieldName)
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

        private DateTime GetAutoUpdateFieldDateTime(string fieldName)
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

        private TimeSpan GetAutoUpdateFieldTimeSpan(string fieldName)
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

        private decimal GetAutoUpdateFieldDecimal(string fieldName)
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

        protected internal void AddAutoUpdateField(string fieldName, DbType fieldType)
        {
            AutoUpdateFieldDictionary.Add(fieldName, new DbParameterObject(fieldName, fieldType, null));
        }

	    public string LoadStoredProcedure { get; set; } = string.Empty;

	    public string DeleteStoredProcedure { get; set; } = string.Empty;

	    public string SaveStoredProcedure { get; set; } = string.Empty;

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

		#endregion

        #region GetField Methods

        protected void GetField(string fieldName, out short field)
        {
            field = GetFieldInt16(fieldName);
        }

        protected void GetField(string fieldName, out int field)
        {
            field = GetFieldInt32(fieldName);
        }

        protected void GetField(string fieldName, out Guid field)
        {
            field = GetFieldGuid(fieldName);
        }

        protected void GetField(string fieldName, out string field)
        {
            field = GetFieldString(fieldName);
        }

        protected void GetField(string fieldName, out bool field)
        {
            field = GetFieldBool(fieldName);
        }

        protected void GetField(string fieldName, out DateTime field)
        {
            field = GetFieldDateTime(fieldName);
        }

        protected void GetField(string fieldName, out TimeSpan field)
        {
            field = GetFieldTimeSpan(fieldName);
        }

        protected void GetField(string fieldName, ref XmlDocument field)
        {
            field.LoadXml(GetFieldString(fieldName));
        }

        protected void GetField(string fieldName, out decimal field)
        {
            field = GetFieldDecimal(fieldName);
        }

        private short GetFieldInt16(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (short)PropertyDictionary[fieldName];
            }

            if (Client.IsAssigned())
            {
                return Client.GetFieldInt16(fieldName);
            }

            throw (new Exception("GetFieldInt16: client/_PropertyDictionary is not assigned"));
        }

		private int GetFieldInt32(string fieldName)
		{
			if (PropertyDictionary.IsAssigned())
			{
				return (int)PropertyDictionary[fieldName];
			}

		    if (Client.IsAssigned())
		    {
		        return Client.GetFieldInt32(fieldName);
		    }

		    throw (new Exception("GetFieldInt32: client/_PropertyDictionary is not assigned"));
		}

        private Guid GetFieldGuid(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (Guid)PropertyDictionary[fieldName];
            }

            if (Client.IsAssigned())
            {
                var fieldValue = Client.GetFieldGuid(fieldName);

                if (fieldName.Equals(GuidIdField, StringComparison.InvariantCultureIgnoreCase))
                {
                    GuidIdValue = fieldValue;
                }

                return fieldValue;
            }

            throw (new Exception("GetFieldGuid: client/_PropertyDictionary is not assigned"));
        }

		private string GetFieldString(string fieldName)
		{
			if (PropertyDictionary.IsAssigned())
			{
				return PropertyDictionary[fieldName] as string;
			}
		    
            if (Client.IsAssigned())
		    {
		        return Client.GetFieldString(fieldName);
		    }

		    throw (new Exception("GetFieldString: client/_PropertyDictionary  is not assigned"));
		}

        private bool GetFieldBool(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (bool)PropertyDictionary[fieldName];
            }
            
            if (Client.IsAssigned())
            {
                return Client.GetFieldBool(fieldName);
            }

            throw (new Exception("GetFieldBool: client/_PropertyDictionary  is not assigned"));
        }

		private DateTime GetFieldDateTime(string fieldName)
		{
			if (PropertyDictionary.IsAssigned())
			{
				return (DateTime)PropertyDictionary[fieldName];
			}
		    
            if (Client.IsAssigned())
		    {
		        return Client.GetFieldDateTime(fieldName);
		    }

		    throw (new Exception("GetFieldDateTime: client/_PropertyDictionary  is not assigned"));
		}

        private TimeSpan GetFieldTimeSpan(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (TimeSpan)PropertyDictionary[fieldName];
            }
            
            if (Client.IsAssigned())
            {
                return Client.GetFieldTimeSpan(fieldName);
            }

            throw (new Exception("GetFieldTimeSpan: client/_PropertyDictionary  is not assigned"));
        }

        private decimal GetFieldDecimal(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (decimal)PropertyDictionary[fieldName];
            }
            
            if (Client.IsAssigned())
            {
                return Client.GetFieldDecimal(fieldName);
            }

            throw (new Exception("GetFieldDecimal: client/_PropertyDictionary  is not assigned"));
        }


        #endregion

		#region SetField Methods
		protected void SetField(string fieldName, int value)
		{
		    DbParameterObjectList.SetField(fieldName, value);
		}

        protected void SetField(string fieldName, Guid value)
        {
            DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetField(string fieldName, string value)
		{
            DbParameterObjectList.SetField(fieldName, value);
		}

		protected void SetField(string fieldName, bool value)
		{
            DbParameterObjectList.SetField(fieldName, value);
		}

		protected void SetField(string fieldName, DateTime value)
		{
            DbParameterObjectList.SetField(fieldName, value);
		}

        protected void SetField(string fieldName, TimeSpan value)
        {
            DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetField(string fieldName, decimal value)
        {
            DbParameterObjectList.SetField(fieldName, value);
        }
        #endregion

		#region SetParameter Methods
		protected void SetParameter(string fieldName, int value)
		{
			SetField(fieldName, value);
		}

        protected void SetParameter(string fieldName, Guid value)
        {
            SetField(fieldName, value);
        }

        protected void SetParameter(string fieldName, string value)
		{
			SetField(fieldName, value);
		}

		protected void SetParameter(string fieldName, bool value)
		{
			SetField(fieldName, value);
		}

		protected void SetParameter(string fieldName, DateTime value)
		{
			SetField(fieldName, value);
		}

        protected void SetParameter(string fieldName, TimeSpan value)
        {
            SetField(fieldName, value);
        }
		#endregion

        public void LoadDataItem()
        {
            if (LoadStoredProcedure.IsAssigned())
            {
                GetItem(LoadStoredProcedure, Id);
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

        protected void GetItem(string storedProcedure)
		{
            InternalInitializeDataItem();

            Client.GetItem(this, storedProcedure);

            AfterGetDataItem();
        }

		protected void GetItem(string storedProcedure, int autoKeyId)
		{
            InternalInitializeDataItem();

            SetField(AutoIdField, autoKeyId);

            GetItem(storedProcedure);
		}

        protected void GetItem(string storedProcedure, Guid guidId)
        {
            InternalInitializeDataItem();

            SetField(GuidIdField, guidId);

            GetItem(storedProcedure);
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

			Client.Delete(DeleteStoredProcedure, AutoIdField, Id);

			Id = 0;

            AfterDelete();
        } 

        protected internal int ExecuteCommand(string storedProcedure, DbParameterObjectList parameterObjectList)
        {
            return Client.ExecuteCommand(storedProcedure, parameterObjectList);
        }

		protected internal int ExecuteTextCommand(string textCommand)
		{
            return Client.ExecuteTextCommand(textCommand);
		} 

		protected internal bool TableExists(string tableName)
		{
            return Client.TableExists(tableName);
		}

		protected internal bool ViewExists(string viewName)
		{
            return Client.ViewExists(viewName);
		}

        protected internal bool StoredProcedureExists(string storedProcedureName)
        {
            return Client.StoredProcedureExists(storedProcedureName);
        }

        public IDbParameterObjectList SetItemData()
		{
			SetData();

            return SetParameterData();
		}

        internal DbParameterObjectList SetParameterData()
        {
            var parameterObjectList = new DbParameterObjectList();

            foreach (var parameterObject in DbParameterObjectList)
            {
                parameterObjectList.Add(parameterObject);
            }

            DbParameterObjectList.Clear();

            return parameterObjectList;
        }

        protected virtual void GetAutoUpdateData()
        {
        }

        public virtual void GetData()
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
