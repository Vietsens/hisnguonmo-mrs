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

namespace MRS.Processor.Mrs00201
{
    public partial class ManagerSql
    {
        internal List<SereServRdo> GetSereServ(Mrs00201Filter filter)
        {
            List<SereServRdo> result = new List<SereServRdo>();
            try
            {
                string query = "";
                query += "SELECT\n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                query += "TREA.TDL_PATIENT_CODE, \n";
                query += "TREA.TDL_PATIENT_NAME, \n";
                query += "TREA.TDL_PATIENT_GENDER_ID, \n";
                query += "SS.* \n";
             
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "WHERE 1=1 \n";
                query += "AND SS.IS_DELETE = 0 \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";
                if (filter.CHOOSE_TIME == true)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} AND {1}\n", filter.DATE_FROM, filter.DATE_TO);
                }
                else
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1}\n", filter.DATE_FROM, filter.DATE_TO);
                }

                if (filter.IS_NOT_TAKE_MEMA ==true)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID NOT IN (6,7)\n");
                }

                if (filter.KSK_CONTRACT_ID != null)
                {
                    query += string.Format("AND TREA.TDL_KSK_CONTRACT_ID = {0}\n", filter.KSK_CONTRACT_ID);
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    if (filter.SERVICE_REQ_STT_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT) && filter.ALLOW_HAS_CONCLUDE == true)
                    {
                        query += string.Format("AND (SR.SERVICE_REQ_STT_ID IN ({0}) or exists (select 1 from his_sere_serv_ext sse where sse.sere_serv_id = ss.id and sse.conclude is not null) or exists (select 1 from his_sere_serv_tein sst where sst.sere_serv_id = ss.id and sst.value is not null))\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                    }
                    else
                    {
                        query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<SereServRdo>(query);

                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

    }
}
