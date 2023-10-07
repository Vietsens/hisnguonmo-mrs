using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00728
{
    internal class ManagerSql
    {
        public class Data
        {
            public long TDL_REQUEST_ROOM_ID { get; set; }
            public string TDL_REQUEST_LOGINNAME { get; set; }
            public string TDL_REQUEST_USERNAME { get; set; }
            public long TDL_INTRUCTION_DATE { get; set; }
            public long? TDL_MEDICINE_TYPE_ID { get; set; }
            public string MEDICINE_TYPE_CODE { get; set; }
            public string MEDICINE_TYPE_NAME { get; set; }
            public string MANUFACTURER_CODE { get; set; }
            public string MANUFACTURER_NAME { get; set; }
            public string SERVICE_UNIT_NAME { get; set; }
            public long? EXP_TIME { get; set; }
            public decimal? PRICE { get; set; }
            public decimal AMOUNT { get; set; }
        }

        internal List<Data> Get(Mrs00728Filter filter)
        {
            List<Data> result = new List<Data>();
            try
            {
                string query = "";
                query += "select ss.tdl_request_room_id, ss.tdl_request_loginname, ss.tdl_request_username, ss.tdl_intruction_date, emm.tdl_medicine_type_id, emm.medicine_type_code, emm.medicine_type_name, emm.manufacturer_code, emm.manufacturer_name, emm.service_unit_name, emm.exp_time, nvl(emm.price,0)*(1+nvl(emm.vat_ratio,0)) price, emm.amount\n";
                query += "from v_his_exp_mest_medicine emm join his_sere_serv ss on ss.exp_mest_medicine_id = emm.id \n";
                
                query += "where 1=1 \n";
                query += "and (ss.is_no_execute = 0 or ss.is_no_execute is null) and (ss.is_delete = 0 or ss.is_delete is null)\n";
                query += "and emm.is_export = 1 \n";
                query += string.Format("and ss.tdl_intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.REQUEST_LOGINNAMEs != null)
                {
                    query += string.Format("and ss.tdl_request_loginname in ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAMEs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.tdl_request_department_id in ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("and ss.tdl_request_room_id in ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.MEDICINE_TYPE_IDs != null)
                {
                    query += string.Format("and emm.tdl_medicine_type_id in ({0})\n", string.Join(",", filter.MEDICINE_TYPE_IDs));
                }
                else
                {
                    query += string.Format("and ss.tdl_service_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);
                }
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Data>(query);
                LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }
    }
}
