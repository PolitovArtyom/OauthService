using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OauthService.Model;

namespace OauthService.DataAccess.SqLite.Queries
{
    public class SqliteQueryBuilder<T> : ISqlQueryBuilder<T> where T : IDbObject
    {
        public virtual string Select(long id)
        {
            return $"SELECT * FROM {typeof(T).GetTableName()} WHERE Id = {id}";
        }

        public virtual string SelectAll()
        {
            return $"SELECT * FROM {typeof(T).GetTableName()}";
        }

        public virtual string Delete(long id)
        {
            return $"DELETE FROM {typeof(T).GetTableName()} WHERE Id = {id}";
        }

        public virtual string Insert(T obj)
        {
            var objectProperties = GetObjectProperties(obj).ToList();
            var propNames = GetInsertString(objectProperties.Select(op => op.Item1));
            var propValues = GetInsertString(objectProperties.Select(op => op.Item2));

            return $"INSERT INTO {typeof(T).GetTableName()} {propNames} VALUES {propValues}; SELECT last_insert_rowid()";
        }

        public virtual string Update(T obj)
        {
            var objectProperties = GetObjectProperties(obj).ToList();
            var updateValues = GetUpdateString(objectProperties);

            return $"UPDATE {typeof(T).GetTableName()} SET {updateValues} WHERE Id={obj.Id}";
        }

        protected IEnumerable<Tuple<string, string>> GetObjectProperties(T obj)
        {
            var type = obj.GetType();
            foreach (var prop in type.GetProperties().Where(p => p.Name != "Id"))
            {
                var propertyName = prop.Name;
                var propertyValue = prop.GetValue(obj);

                var stringValue = "null";
                if (propertyValue != null)
                {
                    stringValue = Convert.ChangeType(prop.GetValue(obj), prop.PropertyType).ToString();

                    if (!prop.PropertyType.IsNumericType())
                    {
                        stringValue = $"'{stringValue}'";
                    }

                    if (prop.PropertyType.IsEnum)
                    {
                        stringValue = Convert.ChangeType(prop.GetValue(obj), typeof(int)).ToString(); ;
                    }
                }
              
                yield return new Tuple<string, string>(propertyName, stringValue);
            }
        }

        protected string GetInsertString(IEnumerable<string> values)
        {
            var stringBuilder = new StringBuilder("(");
            foreach (var value in values)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(",");
            }

            stringBuilder.Length--; //Last comma
            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }


        protected string GetUpdateString(IEnumerable<Tuple<string, string>> props)
        {
            var stringBuilder = new StringBuilder();
            foreach (var value in props)
            {
                stringBuilder.Append(value.Item1);
                stringBuilder.Append("=");
                stringBuilder.Append(value.Item2);
                stringBuilder.Append(",");
            }

            stringBuilder.Length--; //Last comma
            return stringBuilder.ToString();
        }

    }
}
