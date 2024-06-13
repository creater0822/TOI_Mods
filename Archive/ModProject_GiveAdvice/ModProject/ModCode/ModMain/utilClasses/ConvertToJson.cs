using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace MOD_LE2lAt
{
    public class ConvertToJson
    {
        private static string StringToJson(string s)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '"':
                        stringBuilder.Append("\\\"");
                        break;
                    case '\\':
                        stringBuilder.Append("\\\\");
                        break;
                    case '/':
                        stringBuilder.Append("\\/");
                        break;
                    case '\b':
                        stringBuilder.Append("\\b");
                        break;
                    case '\f':
                        stringBuilder.Append("\\f");
                        break;
                    case '\n':
                        stringBuilder.Append("\\n");
                        break;
                    case '\r':
                        stringBuilder.Append("\\r");
                        break;
                    case '\t':
                        stringBuilder.Append("\\t");
                        break;
                    default:
                        stringBuilder.Append(c);
                        break;
                }
            }
            return stringBuilder.ToString();
        }

        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = StringToJson(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson(list, obj.GetType().Name);
        }

        private static string ListToJson<T>(IList<T> list, string JsonName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(JsonName))
            {
                JsonName = list[0].GetType().Name;
            }
            stringBuilder.Append("{\"" + JsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    PropertyInfo[] properties = Activator.CreateInstance<T>().GetType().GetProperties();
                    stringBuilder.Append("{");
                    for (int j = 0; j < properties.Length; j++)
                    {
                        Type type = properties[j].GetValue(list[i], null).GetType();
                        stringBuilder.Append("\"" + properties[j].Name.ToString() + "\":" + StringFormat(properties[j].GetValue(list[i], null).ToString(), type));
                        if (j < properties.Length - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }
                    stringBuilder.Append("}");
                    if (i < properties.Length - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
            }
            stringBuilder.Append("]}");
            return stringBuilder.ToString();
        }

        public static string ToJson(object jsonObject)
        {
            string text = "{";
            PropertyInfo[] properties = jsonObject.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                object obj = properties[i].GetGetMethod().Invoke(jsonObject, null);
                string empty = string.Empty;
                empty = ((!(obj is DateTime) && !(obj is Guid) && !(obj is TimeSpan)) ? ((!(obj is string)) ? ((!(obj is IEnumerable)) ? ToJson(obj.ToString()) : ToJson((IEnumerable)obj)) : ("'" + ToJson(obj.ToString()) + "'")) : ("'" + obj.ToString() + "'"));
                text = text + "\"" + ToJson(properties[i].Name) + "\":" + empty + ",";
            }
            text.Remove(text.Length - 1, text.Length);
            return text + "}";
        }

        public static string ToJson(IEnumerable array)
        {
            string text = "[";
            foreach (object item in array)
            {
                text = text + ToJson(item) + ",";
            }
            text.Remove(text.Length - 1, text.Length);
            return text + "]";
        }

        public static string ToArrayString(IEnumerable array)
        {
            string text = "[";
            foreach (object item in array)
            {
                text = ToJson(item.ToString()) + ",";
            }
            text.Remove(text.Length - 1, text.Length);
            return text + "]";
        }

        public static string ToJson(DataSet dataSet)
        {
            string text = "[";
            foreach (DataTable table in dataSet.Tables)
            {
                text = text + "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            text = text.TrimEnd(',');
            return text + "]";
        }

        public static string ToJson(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            DataRowCollection rows = dt.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                stringBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string columnName = dt.Columns[j].ColumnName;
                    string str = rows[i][j].ToString();
                    Type dataType = dt.Columns[j].DataType;
                    stringBuilder.Append("\"" + columnName + "\":");
                    str = StringFormat(str, dataType);
                    if (j < dt.Columns.Count - 1)
                    {
                        stringBuilder.Append(str + ",");
                    }
                    else
                    {
                        stringBuilder.Append(str);
                    }
                }
                stringBuilder.Append("}");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, stringBuilder.Length);
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
            {
                jsonName = dt.TableName;
            }
            stringBuilder.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stringBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        stringBuilder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (i < dt.Rows.Count - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }
                    stringBuilder.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
            }
            stringBuilder.Append("]}");
            return stringBuilder.ToString();
        }

        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            while (dataReader.Read())
            {
                stringBuilder.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type fieldType = dataReader.GetFieldType(i);
                    string name = dataReader.GetName(i);
                    string str = dataReader[i].ToString();
                    stringBuilder.Append("\"" + name + "\":");
                    str = StringFormat(str, fieldType);
                    if (i < dataReader.FieldCount - 1)
                    {
                        stringBuilder.Append(str + ",");
                    }
                    else
                    {
                        stringBuilder.Append(str);
                    }
                }
                stringBuilder.Append("}");
            }
            dataReader.Close();
            stringBuilder.Remove(stringBuilder.Length - 1, stringBuilder.Length);
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
    }
}
