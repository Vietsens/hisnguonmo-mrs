using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00658
{
    class ManagerSql
    {
        internal List<HIS_SERE_SERV> GetSS(Mrs00658Filter filter)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT SS.* FROM HIS_SERE_SERV SS \n";
                query += "JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID \n";
                query += "JOIN HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID = SS.ID \n";
                query += "JOIN V_HIS_SERVICE S ON S.ID = SS.SERVICE_ID \n";
                query += "JOIN HIS_PTTT_GROUP PG ON PG.ID = S.PTTT_GROUP_ID \n";
                query += "JOIN HIS_DEPARTMENT D ON SS.TDL_REQUEST_DEPARTMENT_ID = D.ID \n";
                query += "WHERE SS.IS_DELETE = 0 AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND S.PTTT_GROUP_ID IS NOT NULL \n";
                query += "AND SS.EKIP_ID IS NOT NULL\n ";

                if (filter.IS_NOT_FEE.HasValue)
                {
                    if (filter.IS_NOT_FEE == 1)
                    {
                        query += "AND (SSE.IS_FEE IS NULL OR SSE.IS_FEE <> 1) \n";
                    }
                    else
                    {
                        query += "AND SSE.IS_FEE = 1 \n";
                    }
                }

                if (filter.IS_NOT_GATHER_DATA.HasValue)
                {
                    if (filter.IS_NOT_GATHER_DATA == 1)
                    {
                        query += "AND (SSE.IS_GATHER_DATA IS NULL OR SSE.IS_GATHER_DATA <> 1) \n";
                    }
                    else
                    {
                        query += "AND SSE.IS_GATHER_DATA = 1 \n";
                    }
                }

                //loai pt tt theo danh muc
                if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                        }
                        query += string.Format(") \n");
                    }
                }

                if (filter.IS_NT_NGT_0.HasValue)
                {
                    if (filter.IS_NT_NGT_0.Value == 0)
                    {
                        query += string.Format("AND (SR.TREATMENT_TYPE_ID <> {0} or SR.TREATMENT_TYPE_ID is null) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                    else if (filter.IS_NT_NGT_0.Value == 1)
                    {
                        query += string.Format("AND SR.TREATMENT_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                }
                //noi tru ngoai tru theo khoa chi dinh
                //Dien dieu tri duoc dung khi tinh cong PTTT doi voi khoa chi dinh dich vu
                if (filter.IS_NT_NGT.HasValue)
                {
                    if (filter.IS_NT_NGT.Value == 0)
                    {
                        query += string.Format("AND D.REQ_SURG_TREATMENT_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    }
                    else if (filter.IS_NT_NGT.Value == 1)
                    {
                        query += string.Format("AND D.REQ_SURG_TREATMENT_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                }

                if (filter.TIME_TO.HasValue)
                {
                    query += string.Format("AND SSE.END_TIME <= {0} \n", filter.TIME_TO);
                }
                if (filter.TIME_FROM.HasValue)
                {
                    query += string.Format("AND SSE.END_TIME >= {0} \n", filter.TIME_FROM);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
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
