﻿using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.Proccessor.Mrs00578;

namespace MRS.Processor.Mrs00578
{
    public partial class Mrs00578RDOManager : BusinessBase
    {
        public List<Mrs00578RDO> GetMrs00578RDOByTransactionTime(string ParentServiceCode, long TRANSACTION_TIME_FROM, long TRANSACTION_TIME_TO,long? REQUEST_DEPARTMENT_ID)
        {
            try
            {
                if (ParentServiceCode == null) return null;
                List<Mrs00578RDO> result = new List<Mrs00578RDO>();
                string sql = "SELECT TRS.TRANSACTION_TIME,NULL AS TRANSACTION_TIME_STR,SESE.TDL_TREATMENT_CODE AS TREATMENT_CODE,TRS.TDL_PATIENT_NAME AS PATIENT_NAME,TRS.TDL_PATIENT_DOB AS DOB,NULL AS DOB_STR,SESE.HEIN_CARD_NUMBER,SV.SERVICE_NAME,SV.PTTT_GROUP_CODE,SV.PACKAGE_PRICE,(NVL(SUM(CASE NVL(SE1.IS_OUT_PARENT_FEE,0) WHEN 1 THEN 0 ELSE SE1.VIR_TOTAL_PRICE END), 0)+NVL(SUM(CASE NVL(SE1.IS_EXPEND,0)WHEN 1 THEN SE1.VIR_PRICE_NO_EXPEND * SE1.AMOUNT ELSE 0 END), 0)) AS TOTAL_PRICE_IN_FEE,(NVL(SUM(SE1.VIR_TOTAL_PRICE), 0)+NVL(SUM(CASE NVL(SE1.IS_EXPEND,0) WHEN 1 THEN SE1.VIR_PRICE_NO_EXPEND * SE1.AMOUNT ELSE 0 END), 0)) AS TOTAL_PRICE_FEE FROM HIS_SERE_SERV SESE JOIN HIS_SERE_SERV SE1 ON SE1.PARENT_ID = SESE.ID JOIN HIS_SERE_SERV_BILL SBIL ON SESE.ID = SBIL.SERE_SERV_ID JOIN HIS_TRANSACTION TRS ON SBIL.BILL_ID = TRS.ID JOIN V_HIS_SERVICE_1 SV ON SESE.SERVICE_ID = SV.ID and SV.SERVICE_TYPE_ID = 11 JOIN HIS_SERVICE SV1 ON SV.PARENT_ID = SV1.ID and SV1.SERVICE_CODE='{0}' WHERE SESE.IS_NO_EXECUTE IS NULL AND SE1.IS_NO_EXECUTE IS NULL AND SESE.IS_DELETE=0 AND SE1.IS_DELETE=0 AND TRS.TRANSACTION_TIME BETWEEN {1} AND {2} {3} AND (TRS.IS_CANCEL IS NULL OR TRS.IS_CANCEL <>1) GROUP BY TRS.TRANSACTION_TIME, SESE.TDL_TREATMENT_CODE, TRS.TDL_PATIENT_NAME, TRS.TDL_PATIENT_DOB, SESE.HEIN_CARD_NUMBER, SV.SERVICE_NAME, SV.PACKAGE_PRICE, SV.PTTT_GROUP_CODE, SESE.ID ORDER BY TRS.TRANSACTION_TIME";
                string query = string.Format(sql, ParentServiceCode, TRANSACTION_TIME_FROM, TRANSACTION_TIME_TO,  (REQUEST_DEPARTMENT_ID != null) ? string.Format("AND SESE.TDL_REQUEST_DEPARTMENT_ID ={0}", REQUEST_DEPARTMENT_ID) : ""); 
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                    result = DAOWorker.SqlDAO.GetSql<Mrs00578RDO>(query);
                    return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        public List<Mrs00578RDO> GetMrs00578RDOByIntructionTime(string ParentServiceCode, long TRANSACTION_TIME_FROM, long TRANSACTION_TIME_TO, long? REQUEST_DEPARTMENT_ID)
        {
            try
            {
                if (ParentServiceCode == null) return null;
                List<Mrs00578RDO> result = new List<Mrs00578RDO>();
                string sql = "SELECT SESE.TDL_INTRUCTION_TIME AS TRANSACTION_TIME, NULL AS TRANSACTION_TIME_STR, SESE.TDL_TREATMENT_CODE AS TREATMENT_CODE, TREA.TDL_PATIENT_NAME  AS PATIENT_NAME, TREA.TDL_PATIENT_DOB AS DOB, NULL AS DOB_STR, SESE.HEIN_CARD_NUMBER, SV.SERVICE_NAME, SV.PTTT_GROUP_CODE, SV.PACKAGE_PRICE, (NVL(SUM( CASE NVL(SE1.IS_OUT_PARENT_FEE,0) WHEN 1 THEN 0 ELSE SE1.VIR_TOTAL_PRICE END), 0)+NVL(SUM( CASE NVL(SE1.IS_EXPEND,0) WHEN 1 THEN SE1.VIR_PRICE_NO_EXPEND * SE1.AMOUNT ELSE 0 END), 0)) AS TOTAL_PRICE_IN_FEE, (NVL(SUM(SE1.VIR_TOTAL_PRICE), 0)+NVL(SUM( CASE NVL(SE1.IS_EXPEND,0) WHEN 1 THEN SE1.VIR_PRICE_NO_EXPEND * SE1.AMOUNT ELSE 0 END), 0)) AS TOTAL_PRICE_FEE FROM HIS_SERE_SERV SESE JOIN HIS_SERE_SERV SE1 ON SE1.PARENT_ID = SESE.ID JOIN HIS_TREATMENT TREA ON TREA.ID = SESE.TDL_TREATMENT_ID  JOIN V_HIS_SERVICE_1 SV ON SESE.SERVICE_ID = SV.ID AND SV.SERVICE_TYPE_ID = 11 JOIN HIS_SERVICE SV1 ON SV.PARENT_ID   = SV1.ID AND SV1.SERVICE_CODE  ='{0}' WHERE SESE.IS_NO_EXECUTE IS NULL AND SE1.IS_NO_EXECUTE  IS NULL  AND SESE.IS_DELETE=0 AND SE1.IS_DELETE=0 AND SESE.TDL_INTRUCTION_TIME BETWEEN {1} AND {2} {3} GROUP BY SESE.TDL_INTRUCTION_TIME, SESE.TDL_TREATMENT_CODE, TREA.TDL_PATIENT_NAME, TREA.TDL_PATIENT_DOB, SESE.HEIN_CARD_NUMBER, SV.SERVICE_NAME, SV.PACKAGE_PRICE, SV.PTTT_GROUP_CODE, SESE.ID ORDER BY SESE.TDL_INTRUCTION_TIME";

                string query = string.Format(sql, ParentServiceCode, TRANSACTION_TIME_FROM, TRANSACTION_TIME_TO, (REQUEST_DEPARTMENT_ID != null) ? string.Format("AND SESE.TDL_REQUEST_DEPARTMENT_ID ={0}", REQUEST_DEPARTMENT_ID) : ""); 
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = DAOWorker.SqlDAO.GetSql<Mrs00578RDO>(query);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
