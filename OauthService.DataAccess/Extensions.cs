using System;
using System.Collections.Generic;
using OauthService.Model;

namespace OauthService.DataAccess
{
    internal static class Extensions
    {
        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static string GetTableName(this Type type)
        {
            if (TypeTableNames.TryGetValue(type.ToString(), out var tableName))
            {
                return tableName;
            }

            throw new KeyNotFoundException($"You forgot add table name for type {type}");
        }

        private static readonly Dictionary<string, string> TypeTableNames = new Dictionary<string, string>()
        {
            {typeof(AuthResource).ToString(), "AuthResources"},
            {typeof(Client).ToString(), "Clients"},

        };
    }
}
