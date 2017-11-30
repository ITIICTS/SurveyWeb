using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        private static void CreateColumnFromProperty(DataTable dataTable, PropertyInfo[] propertyInfo)
        {
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (CommonType.Contains(propertyInfo[i].PropertyType.Name))
                    dataTable.Columns.Add(propertyInfo[i].Name, propertyInfo[i].PropertyType);
                else
                    dataTable.Columns.Add(propertyInfo[i].Name, Type.GetType("System.String"));
            }
        }

        private static void MappingDataRowFromObject(DataRow dataRow, PropertyInfo[] propertyInfo, object data)
        {
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (CommonType.Contains(propertyInfo[i].PropertyType.Name))
                {
                    try
                    {
                        object result = propertyInfo[i].GetValue(data, null);
                        if (result != null)
                        {
                            dataRow[i] = result.ToString();
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    dataRow[i] = propertyInfo[i].Name;
                }
            }
        }

        /// <summary>
        /// Object To XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToXML(object theObject)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet.DataSetName = "DataSet." + theObject.GetType().Name;
            dataTable.TableName = "DataTable." + theObject.GetType().Name;

            // Create Column
            PropertyInfo[] propertyInfo = theObject.GetType().GetProperties();
            CreateColumnFromProperty(dataTable, propertyInfo);

            // Mapping Data
            DataRow dataRow = dataTable.NewRow();
            MappingDataRowFromObject(dataRow, propertyInfo, theObject);
            dataTable.Rows.Add(dataRow);
            dataSet.Tables.Add(dataTable);
            return dataSet.GetXml();
        }

        /// <summary>
        /// List to XML
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ConvertListToXML(IList list)
        {
            DataSet dataset = new DataSet();
            dataset.DataSetName = list[0].GetType().Name;
            DataTable dataTable = new DataTable();
            dataTable.TableName = list[0].GetType().Name;

            // Create Column
            PropertyInfo[] propertyInfo = list[0].GetType().GetProperties();
            CreateColumnFromProperty(dataTable, propertyInfo);

            // Mapping Data
            DataRow dataRow;
            for (int row = 0; row < list.Count; row++)
            {
                dataRow = dataTable.NewRow();
                MappingDataRowFromObject(dataRow, propertyInfo, list[row]);
                dataTable.Rows.Add(dataRow);
            }

            dataset.Tables.Add(dataTable);
            return dataset.GetXml();
        }

        public static DataTable ConvertXmlToDataTable(string xml)
        {           
            DataSet dataSet = new DataSet();
            StringReader stringReader = new StringReader(xml);
            dataSet.ReadXml(stringReader);
            return dataSet.Tables[0];
        }
    }
}
