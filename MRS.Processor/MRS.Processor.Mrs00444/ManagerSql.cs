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

namespace MRS.Processor.Mrs00444
{
    public partial class ManagerSql : BusinessBase
    {
        internal List<HIS_TRAN_PATI_TECH> GetTranPatiTech()
        {
            List<HIS_TRAN_PATI_TECH> result = new List<HIS_TRAN_PATI_TECH>();
            try
            {
                string query = "";
                query += "SELECT\n";
                query += "* \n";
                query += "FROM HIS_RS.HIS_TRAN_PATI_TECH TPT \n";
                query += "WHERE 1=1 \n";
                query += "AND TPT.IS_ACTIVE = 1 \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRAN_PATI_TECH>(query);

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
        internal List<SereServRdo> GetSereServ(Mrs00444Filter filter)
        {
            List<SereServRdo> result = new List<SereServRdo>();
            try
            {
                string query = "";
                query += "SELECT\n";
                query += "SS.*, \n";
                query += "SR.START_TIME, \n";
                query += "SR.FINISH_TIME, \n";
                query += "SV.PTTT_GROUP_ID \n";
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID \n";
                query += "JOIN HIS_RS.HIS_SERVICE SV ON SV.ID = SS.SERVICE_ID \n";
                query += "WHERE 1=1 \n";
                query += "AND SS.IS_DELETE = 0 \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";
                query += string.Format("AND SS.TDL_INTRUCTION_TIME BETWEEN {0} AND {1}\n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0},{1},{2},{3},{4})\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA);

                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0}\n", filter.DEPARTMENT_ID);
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
