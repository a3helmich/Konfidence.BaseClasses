﻿using System;
using System.Data;
using System.Xml;
using JetBrains.Annotations;

namespace Konfidence.BaseData
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class DataReaderExtensions
    {
        private static byte _byteZero;

        static DataReaderExtensions()
        {
            _byteZero = 0;
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out byte field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? _byteZero : dataReader.GetByte(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out short field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? _byteZero : dataReader.GetInt16(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out int field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? 0 : dataReader.GetInt32(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out long field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? 0 : dataReader.GetInt64(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out decimal field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? 0 : dataReader.GetDecimal(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out bool field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = !dataReader.IsDBNull(fieldOrdinal) && dataReader.GetBoolean(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out Guid field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? Guid.Empty : dataReader.GetGuid(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out string field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? string.Empty : dataReader.GetString(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, ref XmlDocument field)
        {
            dataReader.GetField(fieldName, out string xmlString);

            field.LoadXml(xmlString);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out DateTime field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? DateTime.MinValue : dataReader.GetDateTime(fieldOrdinal);
        }

        public static void GetField(this IDataReader dataReader, string fieldName, out TimeSpan field)
        {
            var fieldOrdinal = dataReader.GetOrdinal(fieldName);

            field = dataReader.IsDBNull(fieldOrdinal) ? TimeSpan.MinValue : (TimeSpan)dataReader.GetValue(fieldOrdinal);
        }
    }
}
