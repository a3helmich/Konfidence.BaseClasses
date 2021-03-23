using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;

namespace DbMenuClasses
{
    public partial class Dl
    {
        public class AccountDataItem : BaseDataItem
        {
            // field definitions
            private const string ID = "Id";
            private const string ACCOUNTID = "AccountId";
            private const string NAME = "Name";
            private const string USERNAME = "Username";
            private const string EMAIL = "Email";
            private const string SYSINSERTTIME = "SysInsertTime";
            private const string SYSUPDATETIME = "SysUpdateTime";
            private const string SYSLOCK = "SysLock";

            // stored procedures
            private const string ACCOUNT_GETROW = "gen_Account_GetRow";
            private const string ACCOUNT_GETROWBYGUID = "gen_Account_GetRowByGuid";
            private const string ACCOUNT_SAVEROW = "gen_Account_SaveRow";
            private const string ACCOUNT_DELETEROW = "gen_Account_DeleteRow";
            private const string ACCOUNT_GETLIST = "gen_Account_GetList";

            // property storage
            private Guid _AccountId = Guid.NewGuid();
            private string _Name = string.Empty;
            private string _Username = string.Empty;
            private string _Email = string.Empty;
            private DateTime _SysInsertTime = DateTime.MinValue;
            private DateTime _SysUpdateTime = DateTime.MinValue;
            private string _SysLock = string.Empty;

            private static IBaseClient _client;

            // generated properties

            public Guid AccountId { get => _AccountId; set => _AccountId = value; }

            public string Name { get => _Name; set => _Name = value; }

            public string Username { get => _Username; set => _Username = value; }

            public string Email { get => _Email; set => _Email = value; }

            public DateTime SysInsertTime => _SysInsertTime;

            public DateTime SysUpdateTime => _SysUpdateTime;

            public string SysLock { get => _SysLock; set => _SysLock = value; }

            static AccountDataItem()
            {
                _client = _serviceProvider.GetService<IBaseClient>();
            }

            public AccountDataItem()
            {
                Client = _client;
            }

            public AccountDataItem(int id) : this()
            {
                GetItem(id);
            }

            public AccountDataItem(Guid accountId) : this()
            {
                GetItem(accountId);
            }

            public override void InitializeDataItem()
            {
                AutoIdField = ID;
                GuidIdField = ACCOUNTID;

                AddAutoUpdateField(SYSINSERTTIME, DbType.DateTime);
                AddAutoUpdateField(SYSUPDATETIME, DbType.DateTime);

                GetStoredProcedure = ACCOUNT_GETROW;
                DeleteStoredProcedure = ACCOUNT_DELETEROW;
                SaveStoredProcedure = ACCOUNT_SAVEROW;
                GetByGuidStoredProcedure = ACCOUNT_GETROWBYGUID;

                base.InitializeDataItem();
            }

            protected override void GetAutoUpdateData()
            {
                this.GetAutoUpdateField(SYSINSERTTIME, ref _SysInsertTime);
                this.GetAutoUpdateField(SYSUPDATETIME, ref _SysUpdateTime);
            }

            public override void GetData(IDataReader dataReader)
            {
                dataReader.GetField(ACCOUNTID, out _AccountId);
                dataReader.GetField(NAME, out _Name);
                dataReader.GetField(USERNAME, out _Username);
                dataReader.GetField(EMAIL, out _Email);
                dataReader.GetField(SYSINSERTTIME, out _SysInsertTime);
                dataReader.GetField(SYSUPDATETIME, out _SysUpdateTime);
                dataReader.GetField(SYSLOCK, out _SysLock);
            }

            protected override void SetData()
            {
                base.SetData();

                this.SetField(ACCOUNTID, _AccountId);
                this.SetField(NAME, _Name);
                this.SetField(USERNAME, _Username);
                this.SetField(EMAIL, _Email);
                this.SetField(SYSLOCK, _SysLock);
            }

            public static List<AccountDataItem> GetList()
            {
                var accountList = new List<AccountDataItem>();

                _client.BuildItemList(accountList, ACCOUNT_GETLIST);

                return accountList;
            }
        }
    }
}
