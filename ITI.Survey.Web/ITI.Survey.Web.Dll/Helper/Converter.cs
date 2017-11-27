using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Helper
{
    public class Converter
    {
        public static ArrayList CommonType
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add("Int16");
                arrayList.Add("Int32");
                arrayList.Add("Int64");
                arrayList.Add("String");
                arrayList.Add("Double");
                arrayList.Add("Boolean");
                return arrayList;
            }
        }


        public static string ToXML(object obj)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            PropertyInfo[] propInfo = obj.GetType().GetProperties();
            dataSet.DataSetName = "DataSet." + obj.GetType().Name;
            dataTable.TableName = "DataTable." + obj.GetType().Name;
            for (int i = 0; i < propInfo.Length; i++)
            {
                if (CommonType.Contains(propInfo[i].PropertyType.Name))
                    dataTable.Columns.Add(propInfo[i].Name, propInfo[i].PropertyType);
                else
                    dataTable.Columns.Add(propInfo[i].Name, Type.GetType("System.String"));
            }
            DataRow dataRow = dataTable.NewRow();
            for (int i = 0; i < propInfo.Length; i++)
            {
                if (CommonType.Contains(propInfo[i].PropertyType.Name))
                {
                    object tempObject = obj;
                    try
                    {
                        object t = propInfo[i].GetValue(tempObject, null);
                        if (t != null)
                            dataRow[i] = t.ToString();
                    }
                    catch { }
                }
                else
                {
                    dataRow[i] = propInfo[i].Name;
                }
            }
            dataTable.Rows.Add(dataRow);
            dataSet.Tables.Add(dataTable);
            return dataSet.GetXml();
        }
    }
}
