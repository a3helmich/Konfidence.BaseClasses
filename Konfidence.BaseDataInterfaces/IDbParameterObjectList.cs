using System;
using System.Collections.Generic;

namespace Konfidence.BaseDataInterfaces
{
    public interface IDbParameterObjectList: IList<IDbParameterObject>
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
