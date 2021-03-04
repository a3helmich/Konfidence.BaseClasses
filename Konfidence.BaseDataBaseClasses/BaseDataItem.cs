using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.Sp;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData
{
    public abstract class BaseDataItem : IBaseDataItem
    {
        private bool _isInitialized;

        private IBaseClient _client;

        internal List<ISpParameterData> SpParameterData { get; }

        protected internal string GuidIdField { get; set; } = string.Empty;

        private int _id;

        public Dictionary<string, ISpParameterData> AutoUpdateFieldDictionary { get; }

        public string AutoIdField { get; set; } = string.Empty;

        public void SetId(int id) => _id = id;

        public int GetId() => _id;

        public List<ISpParameterData> GetParameterObjects() => SpParameterData;

        protected BaseDataItem()
        {
            _isInitialized = false;

            SpParameterData = new List<ISpParameterData>();
            AutoUpdateFieldDictionary = new Dictionary<string, ISpParameterData>();

            InternalInitializeDataItem();
        }

        protected IBaseClient Client
        {
            get
            {
                Debug.WriteLine($"get _client{GetType().FullName}");

                return _client;
            }

            set => _client = value;
        }

        public void GetKey(IDataReader dataReader)
        {
            if (AutoIdField.Length > 0)
            {
                dataReader.GetField(AutoIdField, out int id);

                _id = id;
            }
        }

        [CanBeNull]
        internal object GetAutoUpdateField([NotNull] string fieldName)
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

        public string GetByGuidStoredProcedure { get; set; } = string.Empty;

        [UsedImplicitly]
        public bool IsNew => _id == 0;

        public void LoadDataItem()
        {
            if (GetStoredProcedure.IsAssigned())
            {
                GetItem(_id);
            }
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
        }

        protected void GetItemBy([NotNull] string storedProcedure)
        {
            Client.GetItemBy(this, storedProcedure);
        }

        protected void GetItem(int autoKeyId)
        {
            this.SetField(AutoIdField, autoKeyId);

            GetItem();
        }

        [UsedImplicitly]
        protected void GetItem(Guid guidId)
        {
            this.SetField(GuidIdField, guidId);

            GetItemBy(GetByGuidStoredProcedure);
        }

        public void Save()
        {
            if (!IsValidDataItem())
            {
                return;
            }

            Client.Save(this);

            GetAutoUpdateData();
        }

        public void Delete()
        {
            Client.Delete(this);

            _id = 0;
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
