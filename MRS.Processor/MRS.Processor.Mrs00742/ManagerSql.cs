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
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00742
{
    internal partial class ManagerSql : BusinessBase
    {
        internal List<HIS_TREATMENT> getTreatment(Mrs00742Filter CastFilter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                string query = "";
                query += "select trea.* from his_treatment trea join his_service_req sr on sr.treatment_id = trea.id and sr.service_req_type_id=1\n";
                query += string.Format("where sr.intruction_time between {0} and {1}\n", CastFilter.TIME_FROM, CastFilter.TIME_TO);

                Inventec.Common.Logging.LogSystem.Info(query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_TREATMENT>();
            }
            return result;

        }
        internal List<HIS_TREATMENT> getTreatmentRightRoute(Mrs00742Filter CastFilter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                string query = "";
                query += "select trea.* from his_treatment trea join his_service_req sr on sr.treatment_id = trea.id and sr.service_req_type_id=1 join his_patient_type_alter pta on pta.treatment_id=trea.id and pta.right_route_code='TT'\n";
                query += string.Format("where sr.intruction_time between {0} and {1}\n", CastFilter.TIME_FROM, CastFilter.TIME_TO);

                Inventec.Common.Logging.LogSystem.Info(query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_TREATMENT>();
            }
            return result;

        }
    }
}
