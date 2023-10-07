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
using MRS.MANAGER.Config;
using SDA.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00711
{
    internal class ManagerSql
    {

        internal List<Mrs00711RDO> GetCountTreatment(Mrs00711Filter filter, List<string> IcdCodeDttts, List<string> IcdCodeMos, List<string> IcdCodeQus, List<string> IcdCodeGls, List<V_SDA_DISTRICT> listDistrict)
        {
            List<Mrs00711RDO> result = new List<Mrs00711RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "-- so luong benh nhan mo theo huyen\n";
            query += "select\n";
            query += "trea.tdl_patient_district_code,\n";
            query += "count(1) count_treatment,\n";
            query += string.Format("sum(case when trea.icd_code in ('{0}') then 1 else 0 end) count_treatment_dttt,\n", string.Join("','", IcdCodeDttts));
            query += string.Format("sum(case when trea.icd_code in ('{0}') then 1 else 0 end) count_treatment_mo,\n", string.Join("','", IcdCodeMos));
            query += string.Format("sum(case when trea.icd_code in ('{0}') then 1 else 0 end) count_treatment_qu,\n", string.Join("','", IcdCodeQus));
            query += string.Format("sum(case when trea.icd_code in ('{0}') then 1 else 0 end) count_treatment_gl\n", string.Join("','", IcdCodeGls));
            query += "from his_treatment trea\n";
            query += "join \n";
            query += "(\n";
            query += "select \n";
            query += "ss.tdl_treatment_id\n";
            query += "from his_sere_serv ss\n";
            query += "where 1=1\n";
            query += "and ss.is_no_execute is null\n";
            query += "and ss.is_delete=0\n";
            query += string.Format("and ss.tdl_service_type_id in ({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
            query += string.Format("and ss.tdl_intruction_time between {0} and {1}\n",filter.TIME_FROM,filter.TIME_TO);
            query += "group by\n";
            query += "ss.tdl_treatment_id\n";
            query += ") ss on ss.tdl_treatment_id=trea.id\n";
            query += "where 1=1\n";
            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.tdl_patient_type_id in ({0})", string.Join(",", filter.PATIENT_TYPE_IDs));
            }
            if (filter.DISTRICT_IDs != null)
            {
                List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID)&&!string.IsNullOrWhiteSpace(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                query += string.Format("and trea.tdl_patient_district_code in ('{0}')", string.Join("','", districtCodes));
            }
            query += "group by\n";
            query += "trea.tdl_patient_district_code\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00711RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00711");

            return result;
        }
    }
}
