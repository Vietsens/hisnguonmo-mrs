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
using System.Data;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00173
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_SERE_SERV> GetSereServ(Mrs00173Filter filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {

                string query = "--GetSereServ\r\n";
                query += "SELECT ";
                query += "ss.* ";

                query += "from his_sere_serv ss ";
                query += "join his_service_req sr on sr.id=ss.service_req_id ";

                query += "where ss.is_delete =0 ";
                query += "and ss.is_expend is null ";
                query += "and ss.is_no_execute is null ";
                query += "and sr.is_delete =0 ";
                //query += "and sr.is_expend is null ";
                query += "and sr.is_no_execute is null ";
                query += "and sr.service_req_type_id in (4,10) ";
                /*
                    EXECUTE_ROOM_IDs = CastFilter.EXECUTE_ROOM_IDs,
                 EXECUTE_DEPARTMENT_ID = CastFilter.EXECUTE_DEPARTMENT_ID,
                    CREATE_TIME_FROM = CastFilter.DATE_FROM,
                    CREATE_TIME_TO = CastFilter.DATE_TO,
                    INTRUCTION_TIME_FROM = CastFilter.INTRUCTION_TIME_FROM,
                    INTRUCTION_TIME_TO = CastFilter.INTRUCTION_TIME_TO,
                    FINISH_TIME_FROM = CastFilter.FINISH_TIME_FROM,
                    FINISH_TIME_TO = CastFilter.FINISH_TIME_TO,
                    SERVICE_REQ_TYPE_IDs = new List<long>(){IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT}
                 */
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND sr.EXECUTE_DEPARTMENT_ID = {0} ", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND sr.EXECUTE_ROOM_ID IN ({0}) ", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                if (filter.DATE_FROM != null)
                {
                    query += string.Format("AND sr.CREATE_TIME >= {0} ", filter.DATE_FROM);
                }
                if (filter.DATE_TO != null)
                {
                    query += string.Format("AND sr.CREATE_TIME < {0} ", filter.DATE_TO);
                }

                if (filter.INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND sr.INTRUCTION_TIME >= {0} ", filter.INTRUCTION_TIME_FROM);
                }
                if (filter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND sr.INTRUCTION_TIME < {0} ", filter.INTRUCTION_TIME_TO);
                }

                if (filter.FINISH_TIME_FROM != null)
                {
                    query += string.Format("AND sr.FINISH_TIME >= {0} ", filter.FINISH_TIME_FROM);
                }
                if (filter.FINISH_TIME_TO != null)
                {
                    query += string.Format("AND sr.FINISH_TIME < {0} ", filter.FINISH_TIME_TO);
                }

                //loai pt tt theo danh muc
                if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                       
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                       
                    }
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
                result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<HIS_SERE_SERV_EXT> GetSereServExt(Mrs00173Filter filter)
        {
            List<HIS_SERE_SERV_EXT> result = null;
            try
            {

                string query = "--GetSereServExt\r\n";
                query += "SELECT ";
                query += "sse.* ";

                query += "from his_sere_serv ss ";
                query += "join his_service_req sr on sr.id=ss.service_req_id ";
                query += "join his_sere_serv_ext sse on ss.id=sse.sere_serv_id ";

                query += "where ss.is_delete =0 ";
                query += "and ss.is_expend is null ";
                query += "and ss.is_no_execute is null ";
                query += "and sr.is_delete =0 ";
                //query += "and sr.is_expend is null ";
                query += "and sr.is_no_execute is null ";
                query += "and sr.service_req_type_id in (4,10) ";
                /*
                    EXECUTE_ROOM_IDs = CastFilter.EXECUTE_ROOM_IDs,
                 EXECUTE_DEPARTMENT_ID = CastFilter.EXECUTE_DEPARTMENT_ID,
                    CREATE_TIME_FROM = CastFilter.DATE_FROM,
                    CREATE_TIME_TO = CastFilter.DATE_TO,
                    INTRUCTION_TIME_FROM = CastFilter.INTRUCTION_TIME_FROM,
                    INTRUCTION_TIME_TO = CastFilter.INTRUCTION_TIME_TO,
                    FINISH_TIME_FROM = CastFilter.FINISH_TIME_FROM,
                    FINISH_TIME_TO = CastFilter.FINISH_TIME_TO,
                    SERVICE_REQ_TYPE_IDs = new List<long>(){IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT}
                 */
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND sr.EXECUTE_DEPARTMENT_ID = {0} ", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND sr.EXECUTE_ROOM_ID IN ({0}) ", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                if (filter.DATE_FROM != null)
                {
                    query += string.Format("AND sr.CREATE_TIME >= {0} ", filter.DATE_FROM);
                }
                if (filter.DATE_TO != null)
                {
                    query += string.Format("AND sr.CREATE_TIME < {0} ", filter.DATE_TO);
                }

                if (filter.INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND sr.INTRUCTION_TIME >= {0} ", filter.INTRUCTION_TIME_FROM);
                }
                if (filter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND sr.INTRUCTION_TIME < {0} ", filter.INTRUCTION_TIME_TO);
                }

                if (filter.FINISH_TIME_FROM != null)
                {
                    query += string.Format("AND sr.FINISH_TIME >= {0} ", filter.FINISH_TIME_FROM);
                }
                if (filter.FINISH_TIME_TO != null)
                {
                    query += string.Format("AND sr.FINISH_TIME < {0} ", filter.FINISH_TIME_TO);
                }

                //loai pt tt theo danh muc
                if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);

                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);

                    }
                }


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV_EXT>(query);
                result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();

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
