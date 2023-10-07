using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;
using System.Data;
using System.Reflection;

namespace MRS.Processor.Mrs00396
{
    public partial class ManagerSql : BusinessBase
    {
        internal List<MRS.Processor.Mrs00396.V_HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(long minTreaId, long maxTreaId)
        {
            List<MRS.Processor.Mrs00396.V_HIS_PATIENT_TYPE_ALTER> result = new List<MRS.Processor.Mrs00396.V_HIS_PATIENT_TYPE_ALTER>();
            try
            {
                string query = "select * from HIS_PATIENT_TYPE_ALTER where  treatment_id between  {0} and {1} ";
                query = string.Format(query, minTreaId, maxTreaId);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<MRS.Processor.Mrs00396.V_HIS_PATIENT_TYPE_ALTER>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<MRS.Processor.Mrs00396.V_HIS_PATIENT_TYPE_ALTER>();
            }
            return result;
        }
        internal List<MRS.Processor.Mrs00396.V_HIS_DEPARTMENT_TRAN> GetDepartmentTran(long minTreaId, long maxTreaId)
        {
            List<MRS.Processor.Mrs00396.V_HIS_DEPARTMENT_TRAN> result = new List<MRS.Processor.Mrs00396.V_HIS_DEPARTMENT_TRAN>();
            try
            {
                string query = "select * from HIS_DEPARTMENT_TRAN where  treatment_id between  {0} and {1} ";
                query = string.Format(query, minTreaId, maxTreaId);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<MRS.Processor.Mrs00396.V_HIS_DEPARTMENT_TRAN>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<MRS.Processor.Mrs00396.V_HIS_DEPARTMENT_TRAN>();
            }
            return result;
        }

        internal List<MRS.Processor.Mrs00396.V_HIS_SERE_SERV> GetHisSereServ(long minTreaId, long maxTreaId)
        {
            List<MRS.Processor.Mrs00396.V_HIS_SERE_SERV> result = new List<MRS.Processor.Mrs00396.V_HIS_SERE_SERV>();
            try
            {
                string query = "select * from HIS_SERE_SERV where is_no_execute is null and is_delete=0 and tdl_treatment_id between  {0} and {1} and tdl_service_type_id ={2} ";
                query = string.Format(query, minTreaId, maxTreaId,IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<MRS.Processor.Mrs00396.V_HIS_SERE_SERV>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<MRS.Processor.Mrs00396.V_HIS_SERE_SERV>();
            }
            return result;
        }
    }

}
