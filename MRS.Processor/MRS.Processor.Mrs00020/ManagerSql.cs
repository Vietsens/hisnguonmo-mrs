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


namespace MRS.Processor.Mrs00020
{
    public class ManagerSql
    {

        public List<AccumTreatment> GetAccumTreatment(Mrs00020Filter filter)
        {

            List<AccumTreatment> result = null;
            try
            {

                string query = "";
                query += string.Format("--danh sach so luot kham tich luy\n");
                query += string.Format("select \n");

                query += string.Format("id,\n");
                query += string.Format("treatment_code,\n");
                query += string.Format("sum(1) over (partition by patient_id order by id) accum_treatment\n");

                query += string.Format("from his_treatment\n");

                query += string.Format("where 1=1\n");

                //if (filter.IS_ADD_ACCUMM_TREA!=true)
                //{
                //    query += string.Format("and 1=0\n");
                //}

                query += string.Format("and in_time between {0} and {1}\n", filter.TIME_FROM ?? filter.OUT_TIME_FROM, filter.TIME_TO ?? filter.OUT_TIME_TO);

                query += string.Format("order by patient_id,id\n");
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<AccumTreatment>(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }

    public class AccumTreatment
    {
        public long ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long ACCUM_TREATMENT { get; set; }
    }

}
