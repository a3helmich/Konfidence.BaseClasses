using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Konfidence.Base;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;

namespace Konfidence.BaseData
{
	public class BaseDataItem: BaseItem
	{
		public const string BASE_LANGUAGE = "NL";
		public bool WithLanguage = false;

        private bool _IsSelected;
        private bool _IsEditing;

	    private string _LoadStoredProcedure = string.Empty;
		private string _DeleteStoredProcedure = string.Empty;
		private string _SaveStoredProcedure = string.Empty;

        internal BaseHost DataHost = null;   // _DataHost is used by the GetFieldXXXX methods
		internal Dictionary<string, object> PropertyDictionary = null;

        private int _Id;
        private string _AutoIdField = string.Empty;
        private string _GuidIdField = string.Empty;
        private Guid _GuidIdValue = Guid.Empty;

        private Dictionary<string, DbParameterObject> _AutoUpdateFieldList;

		private string _ServiceName = string.Empty;

		private List<DbParameterObject> _ParameterObjectList = new List<DbParameterObject>();
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

	    public BaseDataItem()
	    {
	        _Id = 0;
		    _IsSelected = false;
		    _IsEditing = false;

			InitializeDataItem();
            AfterInitializeDataItem();
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

		internal void GetProperties(List<DbParameterObject> properties)
		{
			_ParameterObjectList = properties;

			SetData();

			_ParameterObjectList = null;
		}

        internal List<DbParameterObject> GetParameterObjectList()
        {
            return _ParameterObjectList;
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

        //public string Code
        //{
        //    get { return _Id.ToString(); }
        //}

        internal protected Dictionary<string, DbParameterObject> AutoUpdateFieldList
        {
            get
            {
                if (!IsAssigned(_AutoUpdateFieldList))
                {
                    _AutoUpdateFieldList = new Dictionary<string, DbParameterObject>();
                }

                return _AutoUpdateFieldList;
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

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = (Int16)AutoUpdateFieldList[fieldName].Value;
                }
            }
            
            return fieldValue;
        }

        private Int32 GetAutoUpdateFieldInt32(string fieldName)
        {
            var fieldValue = 0;

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = (Int32)AutoUpdateFieldList[fieldName].Value;
                }
            }
            
            return fieldValue;
        }

        private Guid GetAutoUpdateFieldGuid(string fieldName)
        {
            var fieldValue = Guid.Empty;

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = (Guid)AutoUpdateFieldList[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private string GetAutoUpdateFieldString(string fieldName)
        {
            var fieldValue = string.Empty;

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = AutoUpdateFieldList[fieldName].Value as string;
                }
            }

            return fieldValue;
        }

        private bool GetAutoUpdateFieldBool(string fieldName)
        {
            var fieldValue = false;

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = (bool)AutoUpdateFieldList[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private DateTime GetAutoUpdateFieldDateTime(string fieldName)
        {
            var fieldValue = DateTime.MinValue;

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = (DateTime)AutoUpdateFieldList[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private TimeSpan GetAutoUpdateFieldTimeSpan(string fieldName)
        {
            var fieldValue = TimeSpan.MinValue;

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = (TimeSpan)AutoUpdateFieldList[fieldName].Value;
                }
            }

            return fieldValue;
        }

        private Decimal GetAutoUpdateFieldDecimal(string fieldName)
        {
            Decimal fieldValue = 0;

            if (AutoUpdateFieldList.ContainsKey(fieldName))
            {
                if (IsAssigned(AutoUpdateFieldList[fieldName].Value))
                {
                    fieldValue = (Decimal)AutoUpdateFieldList[fieldName].Value;
                }
            }

            return fieldValue;
        }

        internal protected void AddAutoUpdateField(string fieldName, DbType fieldType)
        {
            AutoUpdateFieldList.Add(fieldName, new DbParameterObject(fieldName, fieldType, null));
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

        //line = "_" + columnDataItem.Name + ".LoadXml(GetField" + columnDataItem.DbDataType + "(" + columnDataItem.Name.ToUpper() + "));";

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
            AddInParameter(fieldName, DbType.Int32, value);
		}

        protected void SetField(string fieldName, Guid value)
        {
            if (Guid.Empty.Equals(value))
            {
                AddInParameter(fieldName, DbType.Guid, null);
            }
            else
            {
                AddInParameter(fieldName, DbType.Guid, value);
            }
        }

        protected void SetField(string fieldName, string value)
		{
            AddInParameter(fieldName, DbType.String, value);
		}

		protected void SetField(string fieldName, bool value)
		{
            AddInParameter(fieldName, DbType.Boolean, value);
		}

		protected void SetField(string fieldName, DateTime value)
		{
			if (value > DateTime.MinValue)
			{
                AddInParameter(fieldName, DbType.DateTime, value);
			}
			else
			{
                AddInParameter(fieldName, DbType.DateTime, null);
			}
		}

        protected void SetField(string fieldName, TimeSpan value)
        {
            if (value > TimeSpan.MinValue)
            {
                var inbetween = DateTime.Now;

                inbetween = new DateTime(inbetween.Year, inbetween.Month, inbetween.Day, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);

                AddInParameter(fieldName, DbType.Time, inbetween);
            }
            else
            {
                AddInParameter(fieldName, DbType.Time, null);
            }
        }

        protected void SetField(string fieldName, Decimal value)
        {
            AddInParameter(fieldName, DbType.Decimal, value);
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

        protected void SetParameterList(List<DbParameterObject> parameterObjectList)
        {
            _ParameterObjectList = parameterObjectList;
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

		protected virtual void InitializeDataItem()
		{
			// NOP
		}

        protected virtual void AfterInitializeDataItem()
        {
            // NOP
        }

        internal protected BaseHost GetHost()
        {
            return HostFactory.GetHost(_ServiceName, _DataBaseName);
        }

		protected void GetItem(string storedProcedure)
		{
            var dataHost = GetHost();

            DataHost = dataHost;  // _DataHost is used by the GetFieldXXXX methods

            dataHost.GetItem(this, storedProcedure);

            AfterGetDataItem();

            DataHost = null;
        }

		protected void GetItem(string storedProcedure, int autoKeyId)
		{
            SetField(AutoIdField, autoKeyId);

            GetItem(storedProcedure);
		}

        protected void GetItem(string storedProcedure, Guid guidId)
        {
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
            BeforeSave();

			if (!IsValidDataItem())
			{
				return;
			}

            var dataHost = GetHost();

			dataHost.Save(this);

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
            var dataHost = GetHost();

            BeforeDelete();

			dataHost.Delete(DeleteStoredProcedure, AutoIdField, _Id);

			_Id = 0;

            AfterDelete();
        }
		  
        //protected internal int ExecuteCommand(string storedProcedure, params object[] parameters)
        //{
        //    var dataHost = GetHost();

        //    return dataHost.ExecuteCommand(storedProcedure, parameters);
        //}

		protected internal int ExecuteTextCommand(string textCommand)
		{
            var dataHost = GetHost();

			return dataHost.ExecuteTextCommand(textCommand);
		}

		protected internal bool TableExists(string tableName)
		{
            var dataHost = GetHost();

			return dataHost.TableExists(tableName);
		}

		protected internal bool ViewExists(string viewName)
		{
            var dataHost = GetHost();

			return dataHost.ViewExists(viewName);
		}

        protected internal bool StoredProcedureExists(string storedProcedureName)
        {
            var dataHost = GetHost();

            return dataHost.StoredProcedureExists(storedProcedureName);
        }

        //internal void SetParameters(string storedProcedure, Database database, DbCommand dbCommand)
        //{
        //    foreach (var parameterObject in _ParameterObjectList)
        //    {
        //        database.AddInParameter(dbCommand, parameterObject.Field, parameterObject.DbType, parameterObject.Value);
        //    }

        //    _ParameterObjectList.Clear();
        //}

		internal List<DbParameterObject> SetItemData()
		{
			SetData();

            return SetParameterData();
		}

        internal List<DbParameterObject> SetParameterData()
        {
            var parameterObjectList = new List<DbParameterObject>();

            foreach (var parameterObject in _ParameterObjectList)
            {
                parameterObjectList.Add(parameterObject);
            }

            _ParameterObjectList.Clear();

            return parameterObjectList;
        }

        private void AddInParameter(string field, DbType dbType, object value)
		{
			_ParameterObjectList.Add(new DbParameterObject(field, dbType , value));
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
