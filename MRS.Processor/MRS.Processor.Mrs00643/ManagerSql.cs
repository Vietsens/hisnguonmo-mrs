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

namespace MRS.Processor.Mrs00643
{
    public partial class ManagerSql : BusinessBase
    {
        public Mrs00643GDO GetSereServDO(Mrs00643Filter filter)
        {
            Mrs00643GDO result = new Mrs00643GDO();
            try
            {
                string query = " -- chi phi kham suc khoe\n";
                query += "SELECT \n";
                query += "SS.* \n";
                query += "FROM HIS_SERE_SERV SS \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID\n";
                if (filter.IS_PAY == true)
                {
                    query += "JOIN HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID = SS.ID AND SSB.IS_CANCEL IS NULL\n";

                }
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";

                if (filter.INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} \n", filter.INTRUCTION_TIME_FROM);
                }
                if (filter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} \n", filter.INTRUCTION_TIME_TO);
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID);
                }

                if (filter.HAS_EXPEND.HasValue && filter.HAS_EXPEND.Value == false)
                {
                    query += string.Format("AND SS.IS_EXPEND IS NULL \n");
                }
                if (filter.IS_PAUSE != null)
                {
                    if (filter.IS_PAUSE == true)
                    {
                        query += string.Format("AND TREA.IS_PAUSE = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                    }
                    else
                    {
                        query += string.Format("AND TREA.IS_PAUSE IS NULL \n");
                    }
                }
             
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
               
                if (rs != null)
                {
                    result.listHisSereServ = rs;
                }
                // Ds HSDT
                query = " --danh sach kham suc khoe \n";
                query += "SELECT \n";
                query += "TREA.* ";
                query += "FROM HIS_SERE_SERV SS \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID\n";
                if (filter.IS_PAY == true)
                {
                    query += "JOIN HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID = SS.ID AND SSB.IS_CANCEL IS NULL\n";
                
                }
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";

                if (filter.INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} \n", filter.INTRUCTION_TIME_FROM);
                }
                if (filter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} \n", filter.INTRUCTION_TIME_TO);
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID);
                }

                if (filter.HAS_EXPEND.HasValue && filter.HAS_EXPEND.Value == false)
                {
                    query += string.Format("AND SS.IS_EXPEND IS NULL \n");
                }

                if (filter.IS_PAUSE != null)
                {
                    if (filter.IS_PAUSE == true)
                    {
                        query += string.Format("AND TREA.IS_PAUSE = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                    }
                    else
                    {
                        query += string.Format("AND TREA.IS_PAUSE IS NULL \n");
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rsTreatment = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);

                if (rsTreatment != null)
                {
                    result.listHisTreatment = rsTreatment.GroupBy(o=>o.ID).Select(p=>p.First()).ToList();
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
