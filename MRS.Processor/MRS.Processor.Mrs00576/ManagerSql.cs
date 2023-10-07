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
using MOS.DAO.Sql;
using System.Data;

namespace MRS.Processor.Mrs00576
{
    public partial class ManagerSql
    {

        public List<Mrs00576RDO> GetBaby(Mrs00576Filter filter)
        {
            List<Mrs00576RDO> result = new List<Mrs00576RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += " --danh sach thong tin tre so sinh\n";
            query += "select \n";
            query += "trea.tdl_patient_name,\n";
            query += "trea.tdl_patient_dob,\n";
            query += "trea.tdl_patient_ethnic_name,\n";
            query += "trea.tdl_patient_address,\n";
            query += "trea.tdl_hein_card_number,\n";
            query += "trea.tdl_patient_career_name,\n";
            query += "bp.BORN_POSITION_NAME,\n";
            query += "bb.*\n";
            
            query += "from his_treatment trea \n";
            query += "join his_baby bb on trea.id=bb.treatment_id\n";

            query += string.Format("and trea.in_date between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            query += "left join his_born_position bp on bp.id=bb.born_position_id\n";
            query += "order by trea.in_time\n";

          
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00576RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00576");
            return result;
        }

    }
}
