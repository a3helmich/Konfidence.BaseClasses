using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Konfidence.Base;
using Konfidence.BaseData.ParameterObjects;

namespace Konfidence.BaseData
{
	public class BaseDataItem: BaseItem, IBaseDataItem
	{
		public const string BaseLanguage = "NL";
		public bool WithLanguage = false;

        private bool _isSelected;
        private bool _isEditing;
        private bool _isInitialized;


	    private BaseHost _dataHost; 
		internal Dictionary<string, object> PropertyDictionary;

	    private Dictionary<string, DbParameterObject> _autoUpdateFieldDictionary;

	    public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                IsSelectedChanged();
            }
        }

	    protected DbParameterObjectList DbParameterObjectList { get; private set; } = new DbParameterObjectList();

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
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                IsEditingChanged();
            }
        }

        internal BaseHost DataHost
        {
            get
            {
                if (!_dataHost.IsAssigned())
                {
                    _dataHost = HostFactory.GetHost(ServiceName, DatabaseName);
                }

                return _dataHost;
            }
            set { _dataHost = value; }
        }

	    public BaseDataItem()
	    {
	        Id = 0;
		    _isSelected = false;
		    _isEditing = false;
	        _isInitialized = false;
	    }

		internal void SetId(int id)
		{
			Id = id;
		}

		internal void SetProperties(Dictionary<string, object> propertyDictionary)
		{
			PropertyDictionary = propertyDictionary;

			GetData();

			PropertyDictionary = null;
		}

        internal void GetProperties(DbParameterObjectList properties)
		{
			DbParameterObjectList = properties;

			SetData();

			DbParameterObjectList = null;
		}

        internal DbParameterObjectList GetParameterObjectList()
        {
            return DbParameterObjectList;
        }

		#region properties
		protected string DatabaseName { get; set; } = string.Empty;

	    protected string ServiceName { get; set; } = string.Empty;

	    protected internal string AutoIdField { get; set; } = string.Empty;

	    protected internal string GuidIdField { get; set; } = string.Empty;

	    internal Guid GuidIdValue { get; private set; } = Guid.Empty;

	    protected int Id { get; private set; }

	    protected internal Dictionary<string, DbParameterObject> AutoUpdateFieldDictionary
        {
            get
            {
                if (!_autoUpdateFieldDictionary.IsAssigned())
                {
                    _autoUpdateFieldDictionary = new Dictionary<string, DbParameterObject>();
                }

                return _autoUpdateFieldDictionary;
            }
        }

        protected internal void GetAutoUpdateField(string fieldName, out Int16 fieldValue)
        {
            fieldValue = GetAutoUpdateFieldInt16(fieldName);
        }

        protected internal void GetAutoUpdateField(string fieldName, out Int32 fieldValue)
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

        protected internal void GetAutoUpdateField(string fieldName, out Decimal fieldValue)
        {
            fieldValue = GetAutoUpdateFieldDecimal(fieldName);
        }

        private Int16 GetAutoUpdateFieldInt16(string fieldName)
        {
            Int16 fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
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
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
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

        private Decimal GetAutoUpdateFieldDecimal(string fieldName)
        {
            Decimal fieldValue = 0;

            if (AutoUpdateFieldDictionary.ContainsKey(fieldName))
            {
                if (AutoUpdateFieldDictionary[fieldName].Value.IsAssigned())
                {
                    fieldValue = (Decimal)AutoUpdateFieldDictionary[fieldName].Value;
                }
            }

            return fieldValue;
        }

        protected internal void AddAutoUpdateField(string fieldName, DbType fieldType)
        {
            AutoUpdateFieldDictionary.Add(fieldName, new DbParameterObject(fieldName, fieldType, null));
        }

        protected internal string LoadStoredProcedure { get; set; } = string.Empty;

	    protected internal string DeleteStoredProcedure { get; set; } = string.Empty;

	    protected internal string SaveStoredProcedure { get; set; } = string.Empty;

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

		protected internal void GetKey()
		{
			if (AutoIdField.Length > 0)
			{
				Id = GetFieldInt32(AutoIdField);
			}
		}

        internal int GetId()
        {
            return Id;
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
            if (PropertyDictionary.IsAssigned())
            {
                return (Int16)PropertyDictionary[fieldName];
            }

            if (DataHost.IsAssigned())
            {
                return DataHost.GetFieldInt16(fieldName);
            }

            throw (new Exception("GetFieldInt16: dataHost/_PropertyDictionary is not assigned"));
        }

		private Int32 GetFieldInt32(string fieldName)
		{
			if (PropertyDictionary.IsAssigned())
			{
				return (Int32)PropertyDictionary[fieldName];
			}

		    if (DataHost.IsAssigned())
		    {
		        return DataHost.GetFieldInt32(fieldName);
		    }

		    throw (new Exception("GetFieldInt32: dataHost/_PropertyDictionary is not assigned"));
		}

        private Guid GetFieldGuid(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (Guid)PropertyDictionary[fieldName];
            }

            if (DataHost.IsAssigned())
            {
                var fieldValue = DataHost.GetFieldGuid(fieldName);

                if (fieldName.Equals(GuidIdField, StringComparison.InvariantCultureIgnoreCase))
                {
                    GuidIdValue = fieldValue;
                }

                return fieldValue;
            }

            throw (new Exception("GetFieldGuid: dataHost/_PropertyDictionary is not assigned"));
        }

		private string GetFieldString(string fieldName)
		{
			if (PropertyDictionary.IsAssigned())
			{
				return PropertyDictionary[fieldName] as string;
			}
		    
            if (DataHost.IsAssigned())
		    {
		        return DataHost.GetFieldString(fieldName);
		    }

		    throw (new Exception("GetFieldString: dataHost/_PropertyDictionary  is not assigned"));
		}

        private bool GetFieldBool(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (bool)PropertyDictionary[fieldName];
            }
            
            if (DataHost.IsAssigned())
            {
                return DataHost.GetFieldBool(fieldName);
            }

            throw (new Exception("GetFieldBool: dataHost/_PropertyDictionary  is not assigned"));
        }

		private DateTime GetFieldDateTime(string fieldName)
		{
			if (PropertyDictionary.IsAssigned())
			{
				return (DateTime)PropertyDictionary[fieldName];
			}
		    
            if (DataHost.IsAssigned())
		    {
		        return DataHost.GetFieldDateTime(fieldName);
		    }

		    throw (new Exception("GetFieldDateTime: dataHost/_PropertyDictionary  is not assigned"));
		}

        private TimeSpan GetFieldTimeSpan(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (TimeSpan)PropertyDictionary[fieldName];
            }
            
            if (DataHost.IsAssigned())
            {
                return DataHost.GetFieldTimeSpan(fieldName);
            }

            throw (new Exception("GetFieldTimeSpan: dataHost/_PropertyDictionary  is not assigned"));
        }

        private Decimal GetFieldDecimal(string fieldName)
        {
            if (PropertyDictionary.IsAssigned())
            {
                return (Decimal)PropertyDictionary[fieldName];
            }
            
            if (DataHost.IsAssigned())
            {
                return DataHost.GetFieldDecimal(fieldName);
            }

            throw (new Exception("GetFieldDecimal: dataHost/_PropertyDictionary  is not assigned"));
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

        protected void SetField(string fieldName, Decimal value)
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

        internal void LoadDataItem()
        {
            if (LoadStoredProcedure.IsAssigned())
            {
                GetItem(LoadStoredProcedure, Id);
            }
        }

        protected virtual void AfterGetDataItem()
        {
        }

		protected internal virtual void InitializeDataItem()
		{
		    _isInitialized = true;
		}

		protected void GetItem(string storedProcedure)
		{
		    if (!_isInitialized)
		    {
                InitializeDataItem();
		    }

		    DataHost.GetItem(this, storedProcedure);

            AfterGetDataItem();
        }

		protected void GetItem(string storedProcedure, int autoKeyId)
		{
            if (!_isInitialized)
            {
                InitializeDataItem();
            }


            SetField(AutoIdField, autoKeyId);

            GetItem(storedProcedure);
		}

        protected void GetItem(string storedProcedure, Guid guidId)
        {
            if (!_isInitialized)
            {
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
            if (!_isInitialized)
            {
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
            if (!_isInitialized)
            {
                InitializeDataItem();
            }

            BeforeDelete();

			DataHost.Delete(DeleteStoredProcedure, AutoIdField, Id);

			Id = 0;

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

            foreach (var parameterObject in DbParameterObjectList)
            {
                parameterObjectList.Add(parameterObject);
            }

            DbParameterObjectList.Clear();

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
