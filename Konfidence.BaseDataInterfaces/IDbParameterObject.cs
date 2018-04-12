﻿using System.Data;
using Konfidence.BaseClassInterfaces;

namespace Konfidence.BaseDataInterfaces
{
    public interface IDbParameterObject : IBaseItem
    {
        string Field { get; set; }

        DbType DbType { get; set; }

        object Value { get; set; }
    }
}
