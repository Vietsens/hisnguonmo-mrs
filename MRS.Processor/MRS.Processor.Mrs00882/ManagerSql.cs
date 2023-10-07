using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00882
{
    class ManagerSql
    {
        internal List<Mrs00882RDO> GetList(Mrs00882Filter filter)
        {
            List<Mrs00882RDO> result = new List<Mrs00882RDO>();
            try
            {
                string query = "select \n";
                query += "sum(case when b.born_result_id = 1 then 1 else 0 end) as BORN_ALIVE_TOTAL,\n";
                query += "sum(case when b.born_result_id = 1 and b.gender_name = 'Nam' then 1 else 0 end) as BORN_ALIVE_MALE,\n";
                query += "sum(case when b.born_result_id = 1 and b.gender_name = 'Nữ' then 1 else 0 end) as BORN_ALIVE_FEMALE,\n";
                query += "sum(case when b.born_result_id = 1 and c.care_type_name like '%EENC%' then 1 else 0 end) as BORN_ALIVE_EENC,\n";
                query += "sum(case when b.born_result_id = 1 and b.born_type_id = 2 then 1 else 0 end) as BORN_ALIVE_PREMATURE,";
                query += "0 as BORN_ALIVE_NGAT,\n";
                query += "sum(case when b.weight > 0 then 1 else 0 end) as BORN_WEIGHT_TOTAL,\n";
                query += "sum(case when b.weight > 0 and b.weight < 2500 then 1 else 0 end) as BORN_WEIGHT_LESS_THAN_2500,\n";
                query += "sum(case when b.weight > 4000 then 1 else 0 end) as BORN_WEIGHT_MORE_THAN_4000,\n";
                query += "sum(case when b.is_inject_k1 = 1 then 1 else 0 end) as BORN_K1,\n";
                query += "sum(case when(t.icd_code like 'B%' or t.icd_sub_code like '%B%') then 1 else 0 end) as BORN_MOTHER_WITH_HIV,\n";
                query += "sum(case when b.week_count >= 22 then 1 else 0 end) as BORN_FROM_22WEEK_AND_MORE,\n";
                query += "round(b.born_time,-8) month\n";
                query += "from v_his_baby b\n";
                query += "join v_his_treatment t on b.treatment_id = t.id\n";
                query += "left join lateral ( \n";
                query += "select\n";
                query += "listagg(t.care_type_name,',') within group(order by 1) care_type_name\n";
                query += "from his_care c\n";
                query += "join his_care_detail cd on cd.care_id = c.id \n";
                query += "join his_care_type t on cd.care_type_id = cd.id \n";
                query += "where b.treatment_id = c.treatment_id\n";
                query += ") c on 1=1\n";
                query += "where 1=1 \n";
                query += string.Format("and b.born_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                query += "group by round(b.born_time,-8)";
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00882RDO>(query);
            }
            catch(Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }
    }
}
