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
using Inventec.Core;
using System;

namespace MRS.Processor.Mrs00718
{
    internal class ManagerSql
    {
        internal List<SERE_SERV> GetService(Mrs00718Filter filter)
        {
            List<SERE_SERV> result = new List<SERE_SERV>();
            CommonParam param = new CommonParam();
            try
            {
                string query = "";

                query += "select \n";
                query += "TREA.BRANCH_ID CHI_NHANH, \n";
                query += "PR.SERVICE_NAME DICH_VU_CHA, \n";
                query += "SV.SERVICE_NAME DICH_VU, \n";
                query += "PTT.PATIENT_TYPE_NAME DOI_TUONG_THANH_TOAN, \n";
                query += "PR.SERVICE_CODE MA_DICH_VU_CHA, \n";
                query += "SV.SERVICE_CODE MA_DICH_VU, \n";
                query += "PTT.PATIENT_TYPE_CODE MA_DOI_TUONG_THANH_TOAN, \n";
                query += "TT.TREATMENT_TYPE_CODE MA_DIEN_DIEU_TRI, \n";
                query += "COUNT(DISTINCT(SS.ID)) SO_LUONG\n";
                query += "FROM HIS_SERE_SERV SS\n";
                query += "JOIN HIS_SERE_SERV_TEIN SST ON SS.ID = SST.SERE_SERV_ID \n";
                query += "JOIN HiS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID \n";
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID \n";
                query += "JOIN HIS_SERVICE PR ON SV.PARENT_ID = PR.ID \n";
                query += "JOIN HIS_PATIENT_TYPE PTT ON SS.PATIENT_TYPE_ID = PTT.ID \n";
                query += "JOIN HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID \n";
                query += "JOIN HIS_TREATMENT_TYPE TT ON TREA.TDL_TREATMENT_TYPE_ID = TT.ID \n";
                query += "LEFT JOIN HIS_BED_ROOM BR ON BR.ROOM_ID=SR.REQUEST_ROOM_ID\n";
                query += "WHERE 1=1 \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    if (filter.TIME_FROM > 0)
                    {
                        query += string.Format("AND SST.MODIFY_TIME>={0} \n", filter.TIME_FROM);
                    }
                    if (filter.TIME_TO > 0)
                    {
                        query += string.Format("AND SST.MODIFY_TIME<={0} \n", filter.TIME_TO);
                    }
                }
                else
                {
                    if (filter.TIME_FROM > 0)
                    {
                        query += string.Format("AND SR.FINISH_TIME>={0} \n", filter.TIME_FROM);
                    }
                    if (filter.TIME_TO > 0)
                    {
                        query += string.Format("AND SR.FINISH_TIME<={0} \n", filter.TIME_TO);
                    }
                }
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("and trea.branch_id in ({0}) \n", string.Join(",", filter.BRANCH_IDs));
                }
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.EXACT_BED_ROOM_IDs != null)
                {
                    query += string.Format("AND BR.ID IN({0}) \n", string.Join(",", filter.EXACT_BED_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID IN({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("AND TREA.BRANCH_ID = {0}\n", filter.BRANCH_ID);
                }
                query += string.Format("AND SST.VALUE IS NOT NULL \n");
                query += "GROUP BY \n";
                query += "TREA.BRANCH_ID, \n";
                query += "PR.SERVICE_NAME, \n";
                query += "SV.SERVICE_NAME, \n";
                query += "PTT.PATIENT_TYPE_NAME, \n";
                query += "PR.SERVICE_CODE, \n";
                query += "SV.SERVICE_CODE, \n";
                query += "PTT.PATIENT_TYPE_CODE, \n";
                query += "TT.TREATMENT_TYPE_CODE \n";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERE_SERV>(query);
                Inventec.Common.Logging.LogSystem.Info("Result: " + result);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public class SERE_SERV
        {
            public string DICH_VU { get; set; }
            public string DICH_VU_CHA { get; set; }
            public string DOI_TUONG_THANH_TOAN { get; set; }
            public string MA_DICH_VU { get; set; }
            public string MA_DICH_VU_CHA { get; set; }
            public string MA_DOI_TUONG_THANH_TOAN { get; set; }
            public string MA_DIEN_DIEU_TRI { get; set; }
            public int SO_LUONG { get; set; }

            public long CHI_NHANH { get; set; }
        }
    }
}
