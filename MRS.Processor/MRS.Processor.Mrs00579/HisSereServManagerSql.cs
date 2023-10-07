using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.Proccessor.Mrs00579;

namespace MRS.Processor.Mrs00579
{
    public partial class Mrs00579RDOManager : BusinessBase
    {
        public List<Mrs00579RDO> GetMrs00579RDOByCreateTime(long? ExecuteRoomId, long? CREATE_TIME_FROM, long? CREATE_TIME_TO,List<long> listRequestRoomId )
        {
            try
            {
                List<Mrs00579RDO> result = new List<Mrs00579RDO>();
                string query = "";
                query += "SELECT S.ID, S.TDL_INTRUCTION_TIME AS INTRUCTION_TIME,NULL AS INTRUCTION_TIME_STR,T.TREATMENT_CODE, T.TDL_PATIENT_NAME AS PATIENT_NAME, ";
                query += "T.TDL_PATIENT_DOB AS DOB,NULL AS DOB_STR,T.TDL_PATIENT_GENDER_NAME AS GENDER_NAME,T.TDL_HEIN_CARD_NUMBER AS HEIN_CARD_NUMBER, ";
                query += "T.TDL_PATIENT_ADDRESS AS ADDRESS,T.ICD_NAME,R1.ROOM_NAME AS REQUEST_ROOM_NAME,S.TDL_SERVICE_NAME AS SERVICE_NAME, S.AMOUNT, ";
                query += "S.VIR_PRICE AS PRICE, S.VIR_TOTAL_PRICE AS TOTAL_PRICE, \n";
                query += "T.VACCINE_ID, T.VACINATION_ORDER,SU.SERVICE_UNIT_NAME,METY.MEDICINE_TYPE_NAME,METY.NATIONAL_NAME \n";
                query += "FROM HIS_SERE_SERV S ";
                query += "JOIN HIS_TREATMENT T ON T.ID = S.TDL_TREATMENT_ID ";
                query += "JOIN V_HIS_ROOM R1 ON R1.ID = S.TDL_REQUEST_ROOM_ID ";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON S.ID = SSB.SERE_SERV_ID ";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON S.ID = SSD.SERE_SERV_ID ";
                query += "JOIN HIS_SERVICE_UNIT SU ON SU.ID = S.TDL_SERVICE_UNIT_ID ";
                query += "LEFT JOIN HIS_MEDICINE_TYPE METY ON METY.ID = T.VACCINE_ID ";
                query += "WHERE S.IS_NO_EXECUTE IS NULL ";
                query += "AND S.IS_DELETE = 0 ";
                query += "AND S.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND ( ";
                query += "( SSB.SERE_SERV_ID = S.ID AND SSB.IS_CANCEL IS NULL) ";
                query += "OR (SSD.SERE_SERV_ID = S.ID AND SSD.IS_CANCEL IS NULL) ";
                query += ") ";
                if (ExecuteRoomId != null)
                {
                    query += string.Format("AND S.TDL_EXECUTE_ROOM_ID = {0} ", ExecuteRoomId);
                }
                if (CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND S.CREATE_TIME >= {0} ", CREATE_TIME_FROM);
                }
                if (CREATE_TIME_TO != null)
                {
                    query += string.Format("AND S.CREATE_TIME < {0} ", CREATE_TIME_TO);
                }
                if (listRequestRoomId != null)
                {
                    query += string.Format("AND S.TDL_REQUEST_ROOM_ID IN ({0}) ", string.Join(",", listRequestRoomId));
                }
                query += "ORDER BY T.ID ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                    result = DAOWorker.SqlDAO.GetSql<Mrs00579RDO>(query);
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
