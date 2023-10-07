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

namespace MRS.Processor.Mrs00512
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_SERE_SERV> GetSereServDO(Mrs00512Filter filter)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "SS.* \n";
                query += "FROM HIS_SERE_SERV SS, \n";
                query += "HIS_TREATMENT TREA \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID\n ";
                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND EXISTS (SELECT 1 FROM V_HIS_SERVICE_RETY_CAT WHERE SERVICE_ID=SS.SERVICE_ID AND REPORT_TYPE_CODE = 'MRS00512' AND REPORT_TYPE_CAT_ID IN ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }
                else
                {
                    query += string.Format("AND EXISTS (SELECT 1 FROM V_HIS_SERVICE_RETY_CAT WHERE SERVICE_ID=SS.SERVICE_ID AND REPORT_TYPE_CODE = 'MRS00512')\n");
                }

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} \n", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} \n", filter.TIME_TO);
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

             
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
               
                if (rs != null)
                {
                    result = rs;
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
        public List<HIS_SERVICE_REQ> GetServiceReqDO(Mrs00512Filter filter)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SR.* ";
                query += "FROM HIS_SERE_SERV SS, ";
                query += "HIS_SERVICE_REQ SR, ";
                query += "HIS_TREATMENT TREA ";
                query += "WHERE SS.IS_DELETE = 0 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL and sr.is_delete =0 ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID ";
                query += "AND SR.ID = SS.SERVICE_REQ_ID ";
                query += "AND EXISTS (SELECT 1 FROM V_HIS_SERVICE_RETY_CAT WHERE SERVICE_ID=SS.SERVICE_ID AND REPORT_TYPE_CODE = 'MRS00512') ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) ", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0}) ", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_REQ>(query);

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
