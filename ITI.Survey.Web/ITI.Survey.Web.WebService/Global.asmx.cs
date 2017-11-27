using ITI.Survey.Web.Dll.DAL;
using ITI.Survey.Web.Dll.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ITI.Survey.Web.WebService
{
    /// <summary>
    /// Summary description for Global
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Global : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetConfigXML(string activeUser)
        {
            DataSet dataSetWebServiceGlobal = new DataSet();
            dataSetWebServiceGlobal.DataSetName = "ds";
            int i = 1;

            #region Load from Web.config

            string[] stringArray = {
                              ConfigurationManager.AppSettings["HeavyEquipmentList"],
                              ConfigurationManager.AppSettings["ContainerSize"],
                              ConfigurationManager.AppSettings["ContainerType"]
                          };
            foreach (string stringData in stringArray)
            {
                string[] stringArraySplit = stringData.Split(',');
                DataTable dataTableWebConfig = new DataTable();
                dataTableWebConfig.TableName = "dt" + i.ToString();
                dataTableWebConfig.Columns.Add("Key");
                foreach (string data in stringArraySplit)
                {
                    DataRow dataRow = dataTableWebConfig.NewRow();
                    dataRow[0] = data.ToString();
                    dataTableWebConfig.Rows.Add(dataRow);
                }
                dataSetWebServiceGlobal.Tables.Add(dataTableWebConfig);
                i++;
            }

            #endregion

            #region Load Customer Code
            DataTable dataTableCustomer = new DataTable();
            dataTableCustomer.TableName = "dt" + i.ToString();
            dataTableCustomer.Columns.Add("Key");

            AppPrincipal.LoginForService(activeUser);
            GlobalWebServiceDAL globalWebServiceDAL = new GlobalWebServiceDAL();
            List<string> listCustomerCode = globalWebServiceDAL.GetAllCustomerCode();
            listCustomerCode.Add(" ");
            foreach (string customerCode in listCustomerCode)
            {
                DataRow dataRow = dataTableCustomer.NewRow();
                dataRow[0] = customerCode;
                dataTableCustomer.Rows.Add(dataRow);
            }
            dataSetWebServiceGlobal.Tables.Add(dataTableCustomer);
            i++; // for next use

            #endregion

            #region Load Operator Name

            DataTable dataTableOperatorName = new DataTable();
            dataTableOperatorName.TableName = "dt" + i.ToString();
            dataTableOperatorName.Columns.Add("Key");

            List<string> listOperatorName = globalWebServiceDAL.GetAllOperatorName();
            listOperatorName.Add(" ");
            foreach (string operatorName in listOperatorName)
            {
                DataRow dataRow = dataTableOperatorName.NewRow();
                dataRow[0] = operatorName;
                dataTableOperatorName.Rows.Add(dataRow);
            }
            dataSetWebServiceGlobal.Tables.Add(dataTableOperatorName);
            i++; // for next use

            #endregion
            return dataSetWebServiceGlobal.GetXml();
        }
    }
}
