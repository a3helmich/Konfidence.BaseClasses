using System;
using System.Collections.Generic;
using Konfidence.BaseData.Objects;

namespace Konfidence.BaseDataInterfaces
{
    public interface IDbParameterObjectList: IList<DbParameterObject>
    {
        void SetField(string fieldName, int value);

        void SetField(string fieldName, Guid value);

        void SetField(string fieldName, string value);

        void SetField(string fieldName, bool value);

        void SetField(string fieldName, DateTime value);

        void SetField(string fieldName, TimeSpan value);

        void SetField(string fieldName, decimal value);
    }
}
