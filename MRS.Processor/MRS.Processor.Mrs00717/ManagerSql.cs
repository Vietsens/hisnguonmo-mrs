using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using System.Data;
namespace MRS.Processor.Mrs00717
{
    internal class ManagerSql
    {
        internal List<TREATMENT> GetTreatment(Mrs00717Filter filter)
        {
            List<TREATMENT> result = new List<TREATMENT>();
            CommonParam paramGet = new CommonParam();
            try
            {
                string query = "";

                query += "select \n";
                query += "TREA.OUT_TIME,\n";
                query += "TREA.IN_TIME,\n";
                query += "TREA.TDL_PATIENT_CODE,\n";
                query += "TREA.TDL_PATIENT_NAME,\n";
                query += "TREA.TDL_PATIENT_DOB ,\n";
                query += "TREA.ICD_NAME,\n";
                query += "SESE.SERVICE_ID,\n";
                query += "TREA.TREATMENT_CODE,\n";
                query += "SERV.SERVICE_NAME,\n";
                query += "SETY.SERVICE_TYPE_NAME,\n";
                //query += "SRC.CATEGORY_CODE,\n";
                query += "SESE.TDL_SERVICE_TYPE_ID,\n";
                query += "TREA.LAST_DEPARTMENT_ID,\n";
                query += "DP.DEPARTMENT_CODE LAST_DEPARTMENT_CODE,\n";
                query += "DP.DEPARTMENT_NAME\n";
                query += "FROM HIS_SERE_SERV SESE \n";
                query += "JOIN  HIS_TREATMENT TREA ON SESE.TDL_TREATMENT_ID=TREA.ID\n";
                query += "JOIN HIS_SERVICE SERV ON SESE.SERVICE_ID=SERV.ID\n";
                query += "JOIN HIS_SERVICE_TYPE SETY ON SESE.TDL_SERVICE_TYPE_ID = SETY.ID\n";
                query += "JOIN HIS_DEPARTMENT DP ON TREA.LAST_DEPARTMENT_ID=DP.ID\n";

                query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SESE.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00717') \n";           
                query += "WHERE 1=1\n";

                if (filter.TIME_FROM > 0)
                {
                    query += string.Format("AND TREA.OUT_TIME>={0}\n", filter.TIME_FROM);
                }
                if (filter.TIME_TO > 0)
                {
                    query += string.Format("AND TREA.OUT_TIME<={0}\n", filter.TIME_TO);
                }
                //if (filter.REPORT_TYPE_CAT_IDs != null)
                //{
                //    query += string.Format("AND (SRC.REPORT_TYPE_CAT_ID in({0}) or '''' in ({0}||''''))", string.Join(",",filter.REPORT_TYPE_CAT_IDs));
                //}

              
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00717");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        internal List<V_HIS_SERVICE_RETY_CAT> GetServiceRetyCat()
        {
            List<V_HIS_SERVICE_RETY_CAT> result = new List<V_HIS_SERVICE_RETY_CAT>();
            CommonParam paramGet = new CommonParam();
            try 
            {
                string query = "";

                query += "select * from V_HIS_SERVICE_RETY_CAT where REPORT_TYPE_CODE='MRS00717'\n";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_SERVICE_RETY_CAT>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00717");
                     
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }



        
        public class TREATMENT
        {
            public long? OUT_TIME { get; set; }
            public long IN_TIME { get; set; }
            public long TDL_PATIENT_DOB { get; set; }
            public string TDL_PATIENT_CODE { get; set; }
            public string TDL_PATIENT_NAME { get; set; }
            public string ICD_NAME { get; set; }
            public string TREATMENT_CODE { get; set; }
            public long SERVICE_ID { get; set; }
            public string SERVICE_NAME { get; set; }
            public string SERVICE_TYPE_NAME { get; set; }
            public long TDL_SERVICE_TYPE_ID { get; set; }
            public long? LAST_DEPARTMENT_ID { get; set; }
            public string DEPARTMENT_NAME { get; set; }
            public string LAST_DEPARTMENT_CODE { get; set; }
            public string CATEGORY_CODE { get; set; }

        }
    }
       }
    
