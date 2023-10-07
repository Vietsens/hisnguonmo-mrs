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
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00619
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00619RDO> GetSereServDO(Mrs00619Filter filter)
        {
            List<Mrs00619RDO> result = new List<Mrs00619RDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.ID, ";
                query += "SR.INTRUCTION_TIME, ";
                query += "SR.FINISH_TIME, ";
                query += "SR.START_TIME, ";
                query += "SR.EXECUTE_USERNAME, ";
                query += "SR.TDL_PATIENT_NAME AS PATIENT_NAME, ";
                query += "SR.TDL_PATIENT_CODE AS PATIENT_CODE, ";
                query += "SR.TDL_PATIENT_ADDRESS AS VIR_ADDRESS, ";
                query += "SR.REQUEST_ROOM_ID, ";
                query += "SR.REQUEST_DEPARTMENT_ID, ";
                query += "SR.EXECUTE_ROOM_ID, ";
                query += "SR.ICD_NAME || '; '||SR.ICD_TEXT AS ICD_NAME, ";
                query += "SS.HEIN_CARD_NUMBER, ";
                query += "SS.VIR_PRICE, ";
                query += "SS.AMOUNT, ";
                query += "SS.VIR_TOTAL_PRICE, ";
                query += "SR.REQUEST_USERNAME, ";
                query += "TREA.TREATMENT_CODE, ";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, ";
                query += "SS.PATIENT_TYPE_ID, ";
                query += "CASE  WHEN NVL(TREA.IS_PAUSE,0)=0 THEN 'Đang điều trị' WHEN TREA.IS_LOCK_HEIN = 1 THEN 'Đã duyệt khóa bảo hiểm' WHEN TREA.IS_ACTIVE = 0 THEN 'Đã duyệt khóa viện phí' ELSE 'Đã kết thúc điều trị' END AS STATUS_TREATMENT ";
                query += "FROM HIS_SERE_SERV SS ";
                query += "LEFT JOIN HIS_SERVICE_MACHINE SM ON (SM.SERVICE_ID = SS.SERVICE_ID), ";
                query += "HIS_SERVICE_REQ SR, ";
                query += "V_HIS_SERVICE_RETY_CAT SRC, ";
                query += "HIS_TREATMENT TREA ";
                query += "WHERE SS.IS_DELETE = 0 and sr.is_delete=0 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.SERVICE_REQ_ID = SR.ID ";
                query += "AND SS.SERVICE_ID = SRC.SERVICE_ID ";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID ";

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID);
                }

                if (filter.FINISH_TIME_FROM != null)
                {
                    query += string.Format("AND SR.FINISH_TIME >= {0} ", filter.FINISH_TIME_FROM);
                }
                if (filter.FINISH_TIME_TO != null)
                {
                    query += string.Format("AND SR.FINISH_TIME < {0} ", filter.FINISH_TIME_TO);
                }

                query += "AND SR.SERVICE_REQ_STT_ID = 3 ";
                query += "AND SR.IS_NO_EXECUTE IS NULL ";

                query += "AND SRC.REPORT_TYPE_CODE = 'MRS00619' ";

                if (filter.REPORT_TYPE_CAT_ID != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} ", filter.REPORT_TYPE_CAT_ID);
                }

                if (filter.MACHINE_ID != null)
                {
                    query += string.Format("AND SM.MACHINE_ID = {0} ", filter.MACHINE_ID);
                }

                if (filter.MACHINE_IDs != null)
                {
                    query += string.Format("AND SM.MACHINE_ID IN ({0}) ", string.Join(",", filter.MACHINE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00619RDO>(query);
               
                if (rs != null)
                {
                    result = rs.GroupBy(o=>o.ID).Select(p=>p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
    }
}
