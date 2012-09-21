using System;
using System.Collections.Generic;
using System.Data.Common;
using Konfidence.Base;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData
{
	public interface IBaseDataItemList
	{
		void SetParameters(string storedProcedure, Database database, DbCommand dbCommand);
		void AddItem(BaseHost dataHost);
		BaseDataItem GetDataItem();
		List<List<DbParameterObject>> Convert2ListOfParameterObjectList();
	}

    public class BaseDataItemList<T> : List<T>, IBaseDataItemList where T : BaseDataItem, new()
	{

		private string _GetListStoredProcedure = string.Empty;
		private string _DataBaseName = string.Empty;
		private string _ServiceName = string.Empty;

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

		protected void BuildItemList(string getListStoredProcedure)
		{
            // TODO : fix: don't forget the defaultStoredProcedure this way
            string defaultList = GetListStoredProcedure;

			GetListStoredProcedure = getListStoredProcedure;

			BuildItemList();

            GetListStoredProcedure = defaultList;
		}

        private BaseHost GetHost()
        {
            return HostFactory.GetHost(_ServiceName, _DataBaseName);
        }
        
        protected void BuildItemList()
		{
            BaseHost dataHost = GetHost();

			dataHost.BuildItemList(this, GetListStoredProcedure);

            AfterDataLoad();
		}

		protected void RebuildItemList()
		{
			Clear();

			BuildItemList();
		}

		protected static bool IsAssigned(object assignedObject)
		{
			return BaseItem.IsAssigned(assignedObject);
		}

        protected static bool IsEmpty(string assignedString)
        {
            return BaseItem.IsEmpty(assignedString);
        }

        ///// <summary>
        ///// create and return a xxxDataItem derived from BaseDataItem
        ///// </summary>
        ///// <returns></returns>
        //protected virtual T GetNewDataItem()
        //{
        //    throw new NotImplementedException(); // NOP
        //}

		public BaseDataItem GetDataItem()
		{
			T baseDataItem = new T();

			Add(baseDataItem);

			return baseDataItem;
		}

        public T FindById(int id)
        {
            foreach (T dataItem in this)
            {
                if (dataItem.Id == id)
                {
                    return dataItem;
                }
            }

            return null;
        }

        public T FindByGuidId(Guid guidId)
        {
            foreach (T dataItem in this)
            {
                if (dataItem.GuidIdValue == guidId)
                {
                    return dataItem;
                }
            }

            return null;
        }

        protected T FindByIsSelected()
        {
            foreach (T dataItem in this)
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
            foreach (T dataItem in this)
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
            T dataItem = FindByIsEditing();

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
                foreach (T dataItem in this)
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
            int id = 0;
            bool isEditing = false;

            bool.TryParse(isEditingText, out isEditing);

            if (int.TryParse(idText, out id))
            {
                SetSelected(id, isEditing);
            }
            else
            {
                Guid guidId;

                Guid.TryParse(idText, out guidId);

                SetSelected(guidId, isEditing);
            }
        }

        public void SetSelected(string idText)
        {
            int id = 0;

            if (int.TryParse(idText, out id))
            {
                SetSelected(id, false);
            }
            else
            {
                Guid guidId;

                Guid.TryParse(idText, out guidId);

                SetSelected(guidId, false);
            }
        }

        public void SetSelected(BaseDataItem dataItem)
        {
            if (IsAssigned(dataItem))
            {
                SetSelected(dataItem.Id, false);
            }
        }

        private void SetSelected(int id)
        {
            SetSelected(id, false);
        }
        
        private void SetSelected(int id, bool isEditing)
        {
            SetSelected(id, Guid.Empty, isEditing);
        }
        
        private void SetSelected(Guid guidId, bool isEditing)
        {
            SetSelected(0, guidId, isEditing);
        }

        private void SetSelected(int id, Guid guidId, bool isEditing)
        {
            if (this.Count > 0)
            {
                foreach (T dataItem in this)
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
                        dataItem = this.FindById(id);
                    }
                    else
                    {
                        dataItem = this.FindByGuidId(guidId);
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
        //public void SetIsEditing(bool isEditing)
        //{
        //    SetIsEditing(isEditing, 0);
        //}

        //public void SetIsEditing(string isEditingText, string idText)
        //{
        //    bool isEditing = false;
        //    int id = 0;

        //    if (int.TryParse(idText, out id))
        //    {
        //        if (bool.TryParse(isEditingText, out isEditing))
        //        {
        //            this.SetIsEditing(isEditing, id);
        //        }
        //    }
        //}

        //public void SetIsEditing(bool isEditing, int id)
        //{
        //    if (isEditing)
        //    {
        //        AddEditing(id);
        //    }
        //    else
        //    {
        //        RemoveEditing();
        //    }
        //}

        //private void AddEditing(int id)
        //{
        //    T dataItem = null;

        //    dataItem = this.FindById(id);

        //    if (id < 1 || !IsAssigned(dataItem))
        //    {
        //        dataItem = new T();

        //        this.Add(dataItem);
        //    }

        //    dataItem.IsEditing = true;
        //}

        //private void RemoveEditing()
        //{
        //    T dataItem = null;

        //    dataItem = this.FindByIsEditing();

        //    if (IsAssigned(dataItem))
        //    {
        //        dataItem.IsEditing = false;

        //        if (dataItem.IsNew)
        //        {
        //            this.Remove(dataItem);
        //        }
        //    }
        //}
        #endregion list editing state control

        #region list dataitem editing
        public void New()
        {
            T dataItem = FindCurrent();

            if (IsAssigned(dataItem))
            {
                dataItem.IsSelected = false;
                dataItem.IsEditing = true; // nieuw
            }

            //this.SetIsEditing(true);
        }

        public void Edit(T dataItem)
        {
            if (IsAssigned(dataItem))
            {
                dataItem.IsEditing = true; // nieuw

                //this.SetIsEditing(true, dataItem.Id);
            }
        }

        public void Save(T dataItem)
        {
            if (IsAssigned(dataItem))
            {
                dataItem.Save();

                this.SetSelected(dataItem.Id);

                dataItem.IsEditing = false; // nieuw
            }

            //this.SetIsEditing(false);
        }

        public void Cancel(T dataItem)
        {
            dataItem.IsEditing = false; // nieuw
            //this.SetIsEditing(false);
        }

        public void Delete(T dataItem)
        {
            if (IsAssigned(dataItem))
            {
                dataItem.Delete();

                int selectedIndex = this.IndexOf(dataItem);

                this.Remove(dataItem);

                if (this.Count > 0)
                {
                    if (selectedIndex < this.Count)
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

        public List<List<DbParameterObject>> Convert2ListOfParameterObjectList()
		{
			List<List<DbParameterObject>> baseDataItemListList = new List<List<DbParameterObject>>();

			foreach (BaseDataItem baseDataItem in this)
			{
				List<DbParameterObject> properties = GetProperties(baseDataItem);

				if (baseDataItem.AutoIdField.Length > 0)
				{
					DbParameterObject property = new DbParameterObject();

					property.Field = "BaseDataItem_KeyValue";
					property.Value = baseDataItem.Id;

					properties.Add(property);
				}

				baseDataItemListList.Add(properties);
			}

			return baseDataItemListList;
		}

		private static List<DbParameterObject> GetProperties(BaseDataItem baseDataItem)
		{
			List<DbParameterObject> properties = new List<DbParameterObject>();

			baseDataItem.GetProperties(properties);

			return properties;
		}

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
			T baseDataItem = new T();

			baseDataItem._DataHost = dataHost;

			if (IsAssigned(baseDataItem))
			{
				baseDataItem.GetKey();

				baseDataItem.GetData();

				Add(baseDataItem);
			}
		}

		protected int ExecuteTextCommand(string textCommand)
		{
            BaseHost dataHost = GetHost(); 

			return dataHost.ExecuteTextCommand(textCommand);
		}

		protected int ExecuteCommand(string storedProcedure, params object[] parameters)
		{
            BaseHost dataHost = GetHost();

			return dataHost.ExecuteCommand(storedProcedure, parameters);
		}

		protected bool TableExists(string tableName)
		{
            BaseHost dataHost = GetHost();

			return dataHost.TableExists(tableName);
		}

		protected bool ViewExists(string viewName)
		{
            BaseHost dataHost = GetHost();

			return dataHost.ViewExists(viewName);
		}

        protected bool StoredProcedureExists(string storedProcedureName)
        {
            BaseHost dataHost = GetHost();

            return dataHost.StoredProcedureExists(storedProcedureName);
        }
	}
}