using System;
using System.Collections.Generic;
using System.Data;
using Konfidence.Base;
using System.Xml;
using Konfidence.BaseData.ParameterObjects;

namespace Konfidence.BaseData
{
	public class BaseDataItem: BaseItem
	{
		public const string BASE_LANGUAGE = "NL";
		public bool WithLanguage = false;

        private bool _IsSelected;
        private bool _IsEditing;
        private bool _IsInitialized;


	    private string _LoadStoredProcedure = string.Empty;
		private string _DeleteStoredProcedure = string.Empty;
		private string _SaveStoredProcedure = string.Empty;

        private BaseHost _DataHost; 
		internal Dictionary<string, object> PropertyDictionary = null;

        private int _Id;
        private string _AutoIdField = string.Empty;
        private string _GuidIdField = string.Empty;
        private Guid _GuidIdValue = Guid.Empty;

        private Dictionary<string, DbParameterObject> _AutoUpdateFieldDictionary;

		private string _ServiceName = string.Empty;

        private DbParameterObjectList _DbParameterObjectList = new DbParameterObjectList();
		private string _DataBaseName = string.Empty;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;

                IsSelectedChanged();
            }
        }

	    protected DbParameterObjectList DbParameterObjectList
	    {
            get { return _DbParameterObjectList; }
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
            get { return _IsEditing; }
            set
            {
                _IsEditing = value;
                IsEditingChanged();
            }
        }

        internal BaseHost DataHost
        {
            get
            {
                if (!IsAssigned(_DataHost))
                {
                    _DataHost = HostFactory.GetHost(_ServiceName, _DataBaseName);
                }

                return _DataHost;
            }
            set { _DataHost = value; }
        }

	    public BaseDataItem()
	    {
	        _Id = 0;
		    _IsSelected = false;
		    _IsEditing = false;
	        _IsInitialized = false;
	    }

		internal void SetId(int id)
		{
			_Id = id;
		}

		internal void SetProperties(Dictionary<string, object> propertyDictionary)
		{
			PropertyDictionary = propertyDictionary;

			GetData();

			PropertyDictionary = null;
		}

        internal void GetProperties(DbParameterObjectList properties)
		{
			_DbParameterObjectList = properties;

			SetData();

			_DbParameterObjectList = null;
		}

        internal DbParameterObjectList GetParameterObjectList()
        {
            return _DbParameterObjectList;
        }

		#region properties
		protected string DataBaseName
		{
			get { return _DataBaseName; }
			set { _DataBaseName = value; }
		}

		protected string ServiceName
		{
			get { return _ServiceName; }
			set { _ServiceName = value; }
		}

		internal protected string AutoIdField
		{
			get { return _AutoIdField; }
			set { _AutoIdField = value; }
		}

        internal protected string GuidIdField
        {
            get { return _GuidIdField; }
            set { _GuidIdField = value; }
        }

        internal Guid GuidIdValue
        {
            get { return _GuidIdValue; }
        }

		protected int Id
		{
			get { return _Id; }
		}

        internal protected Dictionary<string, DbParameterObject> AutoUpdateFieldDictionary
        {
            get
            {
                if (!IsAssigned(_AutoUpdateFieldDictionary))
                {
                    _AutoUpdateFieldDictionary = new Dictionary<string, DbParameterObject>();
                }

                return _AutoUpdateFieldDictionary;
            }
        }

        internal protected void GetAutoUpdateField(string fieldName, out Int16 fieldValue)
        {
            fieldValue = GetAutoUpdateFieldInt16(fieldName);
        }

        internal protected void GetAutoUpdateField(string fieldName, out Int32 fieldValue)
        {
            fieldValue = GetAutoUpdateFieldInt32(fieldName);
        }

        internal protected void GetAutoUpdateField(string fieldName, out Guid fieldValue)
        {
            fieldValue = GetAutoUpdateFieldGuid(fieldName);
        }

        internal protected void GetAutoUpdateField(string fieldName, out string fieldValue)
        {
            fieldValue = GetAutoUpdateFieldString(fieldName);
        }

        internal protected void GetAutoUpdateField(string fieldName, out bool fieldValue)
        {
            fieldValue = GetAutoUpdateFieldBool(fieldName);
        }

        internal protected void GetAutoUpdateField(string fieldName, out DateTime fieldValue)
        {
            fieldValue = GetAutoUpdateFieldDateTime(fieldName);
        }

        internal protected void GetAutoUpdateField(string fieldName, out TimeSpan fieldValue)
        {
            fieldValue = GetAutoUpdateFieldTimeSpan(fieldName);
        }

        internal protected void GetAutoUpdateField(string fieldName, out Decimal fieldValue)
        {
            fieldValue = GetAutoUpdateFieldDecimal(fieldName);
        }

        private Int16 GetAutoUpdateFieldInt16(string fieldName)
        {
            Int16 fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
                {
                    fieldValue = (Int16)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }
            
            return fieldValue;
        }

        private Int32 GetAutoUpdateFieldInt32(string fieldName)
        {
            var fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
                {
                    fieldValue = (Int32)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }
            
            return fieldValue;
        }

        private Guid GetAutoUpdateFieldGuid(string fieldName)
        {
            var fieldValue = Guid.Empty;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
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
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
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
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
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
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
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
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
                {
                    fieldValue = (TimeSpan)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private Decimal GetAutoUpdateFieldDecimal(string fieldName)
        {
            Decimal fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldDictionary[fieldName].Value))
                {
                    fieldValue = (Decimal)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        internal protected void AddAutoUpdateField(string fieldName, DbType fieldType)
        {
            AutoUpdateFieldDictionary.Add(fieldName, new DbParameterObject(fieldName, fieldType, null));
        }

        internal protected string LoadStoredProcedure
        {
            get { return _LoadStoredProcedure; }
            set { _LoadStoredProcedure = value; }
        }

		internal protected string DeleteStoredProcedure
		{
			get { return _DeleteStoredProcedure; }
			set { _DeleteStoredProcedure = value; }
		}

        internal protected string SaveStoredProcedure
		{
			get { return _SaveStoredProcedure; }
			set { _SaveStoredProcedure = value; }
		}

		public bool IsNew
		{
			get
			{
				if (_Id == 0)
				{
					return true;
				}
				return false;
			}
		}

		#endregion
		
		#region GetField Methods

		internal protected void GetKey()
		{
			if (_AutoIdField.Length > 0)
			{
				_Id = GetFieldInt32(_AutoIdField);
			}
		}

        internal int GetId()
        {
            return _Id;
        }

        protected void GetField(string fieldName, out Int16 field)
        {
            field = GetFieldInt16(fieldName);
        }

        protected void GetField(string fieldName, out Int32 field)
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

        protected void GetField(string fieldName, out Decimal field)
        {
            field = GetFieldDecimal(fieldName);
        }

        private Int16 GetFieldInt16(string fieldName)
        {
            if (IsAssigned(PropertyDictionary))
            {
                return (Int16)PropertyDictionary[fieldName];
            }

            if (IsAssigned(DataHost))
            {
                return DataHost.GetFieldInt16(fieldName);
            }

            throw (new Exception("GetFieldInt16: dataHost/_PropertyDictionary is not assigned"));
        }

		private Int32 GetFieldInt32(string fieldName)
		{
			if (IsAssigned(PropertyDictionary))
			{
				return (Int32)PropertyDictionary[fieldName];
			}

		    if (IsAssigned(DataHost))
		    {
		        return DataHost.GetFieldInt32(fieldName);
		    }

		    throw (new Exception("GetFieldInt32: dataHost/_PropertyDictionary is not assigned"));
		}

        private Guid GetFieldGuid(string fieldName)
        {
            if (IsAssigned(PropertyDictionary))
            {
                return (Guid)PropertyDictionary[fieldName];
            }

            if (IsAssigned(DataHost))
            {
                var fieldValue = DataHost.GetFieldGuid(fieldName);

                if (fieldName.Equals(GuidIdField, StringComparison.InvariantCultureIgnoreCase))
                {
                    _GuidIdValue = fieldValue;
                }

                return fieldValue;
            }

            throw (new Exception("GetFieldGuid: dataHost/_PropertyDictionary is not assigned"));
        }

		private string GetFieldString(string fieldName)
		{
			if (IsAssigned(PropertyDictionary))
			{
				return PropertyDictionary[fieldName] as string;
			}
		    
            if (IsAssigned(DataHost))
		    {
		        return DataHost.GetFieldString(fieldName);
		    }

		    throw (new Exception("GetFieldString: dataHost/_PropertyDictionary  is not assigned"));
		}

        private bool GetFieldBool(string fieldName)
        {
            if (IsAssigned(PropertyDictionary))
            {
                return (bool)PropertyDictionary[fieldName];
            }
            
            if (IsAssigned(DataHost))
            {
                return DataHost.GetFieldBool(fieldName);
            }

            throw (new Exception("GetFieldBool: dataHost/_PropertyDictionary  is not assigned"));
        }

		private DateTime GetFieldDateTime(string fieldName)
		{
			if (IsAssigned(PropertyDictionary))
			{
				return (DateTime)PropertyDictionary[fieldName];
			}
		    
            if (IsAssigned(DataHost))
		    {
		        return DataHost.GetFieldDateTime(fieldName);
		    }

		    throw (new Exception("GetFieldDateTime: dataHost/_PropertyDictionary  is not assigned"));
		}

        private TimeSpan GetFieldTimeSpan(string fieldName)
        {
            if (IsAssigned(PropertyDictionary))
            {
                return (TimeSpan)PropertyDictionary[fieldName];
            }
            
            if (IsAssigned(DataHost))
            {
                return DataHost.GetFieldTimeSpan(fieldName);
            }

            throw (new Exception("GetFieldTimeSpan: dataHost/_PropertyDictionary  is not assigned"));
        }

        private Decimal GetFieldDecimal(string fieldName)
        {
            if (IsAssigned(PropertyDictionary))
            {
                return (Decimal)PropertyDictionary[fieldName];
            }
            
            if (IsAssigned(DataHost))
            {
                return DataHost.GetFieldDecimal(fieldName);
            }

            throw (new Exception("GetFieldDecimal: dataHost/_PropertyDictionary  is not assigned"));
        }


        #endregion

		#region SetField Methods
		protected void SetField(string fieldName, int value)
		{
		    _DbParameterObjectList.SetField(fieldName, value);
		}

        protected void SetField(string fieldName, Guid value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetField(string fieldName, string value)
		{
            _DbParameterObjectList.SetField(fieldName, value);
		}

		protected void SetField(string fieldName, bool value)
		{
            _DbParameterObjectList.SetField(fieldName, value);
		}

		protected void SetField(string fieldName, DateTime value)
		{
            _DbParameterObjectList.SetField(fieldName, value);
		}

        protected void SetField(string fieldName, TimeSpan value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetField(string fieldName, Decimal value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
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

        internal void LoadDataItem()
        {
            if (!IsEmpty(LoadStoredProcedure))
            {
                GetItem(LoadStoredProcedure, Id);
            }
        }

        protected virtual void AfterGetDataItem()
        {
        }

		internal protected virtual void InitializeDataItem()
		{
			// NOP
		}

		protected void GetItem(string storedProcedure)
		{
		    if (!_IsInitialized)
		    {
                _IsInitialized = true;

                InitializeDataItem();
		    }

		    DataHost.GetItem(this, storedProcedure);

            AfterGetDataItem();
        }

		protected void GetItem(string storedProcedure, int autoKeyId)
		{
            if (!_IsInitialized)
            {
                _IsInitialized = true;

                InitializeDataItem();
            }


            SetField(AutoIdField, autoKeyId);

            GetItem(storedProcedure);
		}

        protected void GetItem(string storedProcedure, Guid guidId)
        {
            if (!_IsInitialized)
            {
                _IsInitialized = true;

                InitializeDataItem();
            }


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
            if (!_IsInitialized)
            {
                _IsInitialized = true;

                InitializeDataItem();
            }

            BeforeSave();

			if (!IsValidDataItem())
			{
				return;
			}

			DataHost.Save(this);

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
            if (!_IsInitialized)
            {
                _IsInitialized = true;

                InitializeDataItem();
            }

            BeforeDelete();

			DataHost.Delete(DeleteStoredProcedure, AutoIdField, _Id);

			_Id = 0;

            AfterDelete();
        } 

        protected internal int ExecuteCommand(string storedProcedure, DbParameterObjectList parameterObjectList)
        {
            return DataHost.ExecuteCommand(storedProcedure, parameterObjectList);
        }

		protected internal int ExecuteTextCommand(string textCommand)
		{
            return DataHost.ExecuteTextCommand(textCommand);
		} 

		protected internal bool TableExists(string tableName)
		{
            return DataHost.TableExists(tableName);
		}

		protected internal bool ViewExists(string viewName)
		{
            return DataHost.ViewExists(viewName);
		}

        protected internal bool StoredProcedureExists(string storedProcedureName)
        {
            return DataHost.StoredProcedureExists(storedProcedureName);
        }

        //internal void SetParameters(string storedProcedure, Database database, DbCommand dbCommand)
        //{
        //    foreach (var parameterObject in _DbParameterObjectList)
        //    {
        //        database.AddInParameter(dbCommand, parameterObject.Field, parameterObject.DbType, parameterObject.Value);
        //    }

        //    _DbParameterObjectList.Clear();
        //}

        internal DbParameterObjectList SetItemData()
		{
			SetData();

            return SetParameterData();
		}

        internal DbParameterObjectList SetParameterData()
        {
            var parameterObjectList = new DbParameterObjectList();

            foreach (var parameterObject in _DbParameterObjectList)
            {
                parameterObjectList.Add(parameterObject);
            }

            _DbParameterObjectList.Clear();

            return parameterObjectList;
        }

        protected internal virtual void GetAutoUpdateData()
		{
			// NOP
			throw new NotImplementedException();
		}

        protected internal virtual void GetData()
		{
			// NOP
			throw new NotImplementedException();
		}

		protected virtual void SetData()
		{
			// NOP
		}

        //protected virtual void SetQueryParameters(string storedProcedure)
        //{
        //    // NOP
        //}

		protected virtual bool IsValidDataItem()
		{
			return true;
		}
    }
}
