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

namespace MRS.Processor.Mrs00625
{
    public partial class Mrs00625RDOManager : BusinessBase
    {
        public List<Mrs00625RDO> GetRdo(Mrs00625Filter filter)
        {
            try
            {
                List<Mrs00625RDO> result = new List<Mrs00625RDO>();
                string query = "";
                query += "SELECT ";
                query += "SS.ID, ";
                query += "SSB.ID AS SSB_ID, ";
                query += "SSD.ID AS SSD_ID, ";
                query += "TREA.TDL_PATIENT_DOB AS AGE, ";
                query += "TREA.TREATMENT_CODE AS TREATMENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME AS TDL_PATIENT_NAME, ";
                query += "TREA.TDL_PATIENT_ADDRESS AS TDL_PATIENT_ADDRESS, ";
                query += "TREA.ICD_NAME AS ICD_NAME, ";
                query += "SS.TDL_SERVICE_NAME AS TDL_SERVICE_NAME, ";
                query += "ss.tdl_intruction_date AS TDL_FINISH_DATE_STR, ";
                query += "PG.PTTT_GROUP_NAME AS PTTT_GROUP_NAME, ";
                query += "SV.PACKAGE_PRICE, ";
                query += "NVL(SUM(CASE NVL(SS1.IS_OUT_PARENT_FEE,0) WHEN 1 THEN 0 ELSE SS1.VIR_TOTAL_PRICE_NO_EXPEND END), 0) AS TOTAL_PRICE_IN_FEE, ";
                query += "NVL(SUM(CASE NVL(SS1.IS_OUT_PARENT_FEE,0) WHEN 1 THEN 0 ELSE (CASE WHEN SS1.TDL_SERVICE_TYPE_ID =6 THEN SS1.VIR_TOTAL_PRICE_NO_EXPEND ELSE 0 END) END), 0) AS TOTAL_MEDICINE_PRICE_IN_FEE, ";
                query += "NVL(SUM(CASE NVL(SS1.IS_OUT_PARENT_FEE,0) WHEN 1 THEN 0 ELSE (CASE WHEN SS1.TDL_SERVICE_TYPE_ID =7 THEN SS1.VIR_TOTAL_PRICE_NO_EXPEND ELSE 0 END) END), 0) AS TOTAL_MATERIAL_PRICE_IN_FEE, ";
                query += "NVL(SUM(CASE NVL(SS1.IS_OUT_PARENT_FEE,0) WHEN 1 THEN 0 ELSE (CASE WHEN (SS1.TDL_SERVICE_TYPE_ID <>6 AND SS1.TDL_SERVICE_TYPE_ID <>7) THEN SS1.VIR_TOTAL_PRICE_NO_EXPEND ELSE 0 END) END), 0) AS TOTAL_OTHER_PRICE_IN_FEE, ";
                query += "NVL(SUM(CASE (CASE SS1.TDL_REQUEST_ROOM_ID WHEN SS.TDL_EXECUTE_ROOM_ID THEN NVL(SS1.IS_OUT_PARENT_FEE,0) ELSE 0 END)  ";
                query += "WHEN 1 THEN SS1.VIR_TOTAL_PRICE_NO_EXPEND ELSE 0 END), 0) AS TOTAL_PRICE_OUT_FEE ";
                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_SERE_SERV SS1 ";
                query += "ON (SS.ID=SS1.PARENT_ID AND SS.IS_NO_EXECUTE IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL AND (SS.IS_DELETE IS NULL OR SS.IS_DELETE <>1) AND (SS1.IS_DELETE IS NULL OR SS1.IS_DELETE <>1) ";
                query += "AND SS1.IS_NO_EXECUTE IS NULL AND SS1.SERVICE_REQ_ID IS NOT NULL ";
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0} ", filter.DEPARTMENT_ID);
                }
                query += ") ";
                query += "JOIN HIS_SERVICE SV ON (SV.ID=SS.SERVICE_ID ";
                if (filter.PACKAGE_ID != null)
                {
                    query += string.Format("AND SV.PACKAGE_ID = {0} ", filter.PACKAGE_ID);
                }
                query += ") ";
                query += "LEFT JOIN HIS_PTTT_GROUP PG ON PG.ID=SV.PTTT_GROUP_ID ";
                query += "JOIN HIS_TREATMENT TREA ON (TREA.ID=SS.TDL_TREATMENT_ID) ";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON (SSB.SERE_SERV_ID=SS.ID AND SSB.IS_CANCEL IS NULL) ";
                query += "LEFT JOIN HIS_TRANSACTION BILL ON (BILL.ID=SSB.BILL_ID AND BILL.IS_CANCEL IS NULL ";

                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND BILL.TRANSACTION_TIME >={0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND BILL.TRANSACTION_TIME <{0} ", filter.TRANSACTION_TIME_TO);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND BILL.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }
                query += ") ";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SSD.SERE_SERV_ID=SS.ID AND SSD.IS_CANCEL IS NULL) ";
                query += "LEFT JOIN HIS_TRANSACTION DEPOSIT ON (DEPOSIT.ID=SSD.DEPOSIT_ID AND DEPOSIT.IS_CANCEL IS NULL ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND DEPOSIT.TRANSACTION_TIME >={0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND DEPOSIT.TRANSACTION_TIME <{0} ", filter.TRANSACTION_TIME_TO);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND DEPOSIT.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }
                query += ") ";
                query += "WHERE (SSB.ID IS NOT NULL OR SSD.ID IS NOT NULL) ";
                query += "AND (BILL.ID IS NOT NULL OR DEPOSIT.ID IS NOT NULL) ";
                query += "GROUP BY ";
                query += "SS.ID, ";
                query += "SSB.ID, ";
                query += "SSD.ID, ";
                query += "TREA.TDL_PATIENT_DOB, ";
                query += "TREA.TREATMENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME, ";
                query += "TREA.TDL_PATIENT_ADDRESS, ";
                query += "TREA.ICD_NAME, ";
                query += "SS.TDL_SERVICE_NAME, ";
                query += "SS.TDL_INTRUCTION_DATE, ";
                query += "PG.PTTT_GROUP_NAME, ";
                query += "SV.PACKAGE_PRICE ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00625RDO>(query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        /*
         SELECT 
         
         */
    }
}
