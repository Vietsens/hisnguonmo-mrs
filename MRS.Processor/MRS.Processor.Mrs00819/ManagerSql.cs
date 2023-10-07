using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00819
{
    public partial class ManagerSql : BusinessBase
    {
        public List<V_HIS_TREATMENT> GetTreatmentAll(Mrs00819Filter filter)
        {
            List<V_HIS_TREATMENT> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach ho so dieu tri co nhap vien\n");
                query += string.Format("select\n");
                query += string.Format("trea.*\n");

                query += string.Format("from v_his_treatment trea\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);

                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT>(query);
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
