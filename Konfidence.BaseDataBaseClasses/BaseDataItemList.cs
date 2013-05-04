using System;
using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData.ParameterObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Diagnostics;

namespace Konfidence.BaseData
{
    public class BaseDataItemList<T> : List<T>, IBaseDataItemList where T : BaseDataItem, new()
	{
		private string _GetListStoredProcedure = string.Empty;
		private string _DataBaseName = string.Empty;
		private string _ServiceName = string.Empty;

        private readonly DbParameterObjectList _DbParameterObjectList = new DbParameterObjectList();

        protected virtual void AfterDataLoad()
        {
            //
        }

		#region properties

		protected string DataBaseName
		{
			get { return _DataBaseName; }
			set { _DataBaseName = value; }
		}

		protected string GetListStoredProcedure
		{
			get { return _GetListStoredProcedure; }
			set { _GetListStoredProcedure = value; }
		}

		protected string ServiceName
		{
			get { return _ServiceName; }
			set { _ServiceName = value; }
		}

		#endregion

		public BaseDataItemList()
		{
			InitializeDataItemList();
		}

        private BaseHost GetHost()
        {
            return HostFactory.GetHost(_ServiceName, _DataBaseName);
        }

        protected void BuildItemList()
        {
            BuildItemList(GetListStoredProcedure);
        }

		protected void BuildItemList(string getListStoredProcedure)
		{
            var dataHost = GetHost();

            dataHost.BuildItemList(this, getListStoredProcedure);

            AfterDataLoad();
        }

        protected void BuildItemList(IBaseDataItemList relatedDataItemList, IBaseDataItemList childDataItemList)
        {
            var dataHost = GetHost();

            dataHost.BuildItemList(this, relatedDataItemList, childDataItemList, GetListStoredProcedure);

            AfterDataLoad();
        }

		protected void RebuildItemList()
		{
			Clear();

			BuildItemList();
		}

        [ContractAnnotation("assignedObject:null => false")]
        protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

        protected static bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }

		public BaseDataItem GetDataItem()
		{
			var baseDataItem = new T();

			Add(baseDataItem);

			return baseDataItem;
		}

        public T FindById(string textId)
        {
            Guid guidId;
            int id;

            if (Guid.TryParse(textId, out guidId))
            {
                return FindById(guidId);
            }

            if (int.TryParse(textId, out id))
            {
                if (Debugger.IsAttached || BaseItem.UnitTest)
                {
                    throw new Exception("id is niet toegestaan om reocrds op te halen(BaseDataItemList.FindById(..))");
                }

                return FindById(id);
            }

            return null;
        }

        public T FindById(int id)
        {
            foreach (var dataItem in this)
            {
                if (dataItem.GetId() == id)
                {
                    return dataItem;
                }
            }

            return null;
        }

        public T FindById(Guid guidId)
        {
            foreach (var dataItem in this)
            {
                if (dataItem.GuidIdValue == guidId)
                {
                    return dataItem;
                }
            }

            return null;
        }

        private T FindByIsSelected()
        {
            foreach (var dataItem in this)
            {
                if (dataItem.IsSelected)
                {
                    return dataItem;
                }
            }

            if (Count > 0)
            {
                this[0].IsSelected = true;

                return this[0];
            }

            return null;
        }

        protected T FindByIsEditing()
        {
            foreach (var dataItem in this)
            {
                if (dataItem.IsEditing)
                {
                    return dataItem;
                }
            }

            return null;
        }

        public T FindCurrent()
        {
            var dataItem = FindByIsEditing();

            if (!IsAssigned(dataItem))
            {
                dataItem = FindByIsSelected();
            }

            return dataItem;
        }

        public bool HasCurrent
        {
            get
            {
                foreach (var dataItem in this)
                {
                    if (dataItem.IsEditing || dataItem.IsSelected)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #region list selecting state control

        public void SetSelected(string idText, string isEditingText)
        {
            Guid guidId;
            bool isEditing;

            bool.TryParse(isEditingText, out isEditing);

            if (Guid.TryParse(idText, out guidId))
            {
                SetSelected(guidId, isEditing);
            }
            else
            {
                int id;

                if (int.TryParse(idText, out id))
                {
                    if (Debugger.IsAttached || BaseItem.UnitTest)
                    {
                        throw new Exception("id is niet toegestaan om reocrds op te halen(BaseDataItemList.SetSelected(..))");
                    }

                    SetSelected(id, isEditing);
                }
            }
        }

        public void SetSelected(string idText)
        {
            Guid guidId;

            if (Guid.TryParse(idText, out guidId))
            {
                SetSelected(guidId, false);
            }
            else
            {
                int id;

                if (int.TryParse(idText, out id))
                {
                    if (Debugger.IsAttached || BaseItem.UnitTest)
                    {
                        throw new Exception("id is niet toegestaan om reocrds op te halen(BaseDataItemList.SetSelected(..))");
                    }

                    SetSelected(id);
                }
            }
        }

        public void SetSelected(BaseDataItem dataItem)
        {
            if (IsAssigned(dataItem))
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

                if (id < 1 && Guid.Empty.Equals(guidId))
                {
                    this[0].IsSelected = true;
                }
                else
                {
                    T dataItem;

                    if (id > 0)
                    {
                        dataItem = FindById(id);
                    }
                    else
                    {
                        dataItem = FindById(guidId);
                    }

                    if (IsAssigned(dataItem))
                    {
                        dataItem.IsSelected = true;
                        dataItem.IsEditing = isEditing;
                    }
                }
            }
        }
        #endregion list selecting state control

        #region list editing state control

        public void New()
        {
            var dataItem = FindCurrent();

            if (IsAssigned(dataItem))
            {
                dataItem.IsSelected = false;
                dataItem.IsEditing = true; // nieuw
            }
        }

        public void Edit(T dataItem)
        {
            if (IsAssigned(dataItem))
            {
                dataItem.IsEditing = true; // nieuw
            }
        }

        public void Save(T dataItem)
        {
            if (IsAssigned(dataItem))
            {
                dataItem.Save();

                SetSelected(dataItem.GetId());

                dataItem.IsEditing = false; // nieuw
            }
        }

        public void Cancel(T dataItem)
        {
            dataItem.LoadDataItem();

            dataItem.IsEditing = false;
        }

        public void Delete(T dataItem)
        {
            if (IsAssigned(dataItem))
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

        public List<DbParameterObjectList> Convert2ListOfParameterObjectList()
		{
            var baseDataItemListList = new List<DbParameterObjectList>();

			foreach (var baseDataItem in this)
			{
				var properties = GetProperties(baseDataItem);

				if (baseDataItem.AutoIdField.Length > 0)
				{
					var property = new DbParameterObject {Field = "BaseDataItem_KeyValue", Value = baseDataItem.GetId()};

				    properties.Add(property);
				}

				baseDataItemListList.Add(properties);
			}

			return baseDataItemListList;
		}

        private static DbParameterObjectList GetProperties(BaseDataItem baseDataItem)
		{
            var properties = new DbParameterObjectList();

			baseDataItem.GetProperties(properties);

			return properties;
		}

        internal DbParameterObjectList GetParameterObjectList()
        {
            return _DbParameterObjectList;
        }

        #region SetParameter Methods
        protected void SetParameter(string fieldName, int value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetParameter(string fieldName, Guid value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetParameter(string fieldName, string value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetParameter(string fieldName, bool value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetParameter(string fieldName, DateTime value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }

        protected void SetParameter(string fieldName, TimeSpan value)
        {
            _DbParameterObjectList.SetField(fieldName, value);
        }
        #endregion

		/// <summary>
		/// Add parameters for filtering
		/// </summary>
		/// <returns></returns>
		public virtual void SetParameters(string storedProcedure, Database database, DbCommand dbCommand)
		{
			// NOP
		}

		protected virtual void InitializeDataItemList()
		{
			// NOP
		}

		public void AddItem(BaseHost dataHost)
		{
			var baseDataItem = new T {DataHost = dataHost};

		    if (IsAssigned(baseDataItem))
			{
				baseDataItem.GetKey();

				baseDataItem.GetData();

				Add(baseDataItem);
			}
		}

		protected int ExecuteTextCommand(string textCommand)
		{
            var dataHost = GetHost(); 

			return dataHost.ExecuteTextCommand(textCommand);
		}

        //protected int ExecuteCommand(string storedProcedure, params object[] parameters)
        //{
        //    BaseHost dataHost = GetHost();

        //    return dataHost.ExecuteCommand(storedProcedure, parameters);
        //}

		protected bool TableExists(string tableName)
		{
            var dataHost = GetHost();

			return dataHost.TableExists(tableName);
		}

		protected bool ViewExists(string viewName)
		{
            var dataHost = GetHost();

			return dataHost.ViewExists(viewName);
		}

        protected bool StoredProcedureExists(string storedProcedureName)
        {
            var dataHost = GetHost();

            return dataHost.StoredProcedureExists(storedProcedureName);
        }
	}
}