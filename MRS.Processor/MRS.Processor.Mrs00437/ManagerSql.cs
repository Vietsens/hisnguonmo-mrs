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

namespace MRS.Processor.Mrs00437
{
    public  class ManagerSql
    {
        internal List<DATA_GET> GetData(Mrs00437Filter filter)
        {
            List<DATA_GET> result = new List<DATA_GET>();
            try
            {
                string query = "";
                query += " --iss 41522\n";
                query += "--bang tong hop thu vien phi va bhyt\n";
                query += "select \n";
                query += "ss.id,\n";
                query += "ss.service_id,\n";
                query += "ss.tdl_service_type_id,\n";
                query += "ss.patient_type_id,\n";
                query += "ss.hein_card_number,\n";
                query += "trea.tdl_treatment_type_id,\n";
                query += "ss.tdl_treatment_id,\n";
                query += "ss.amount,\n";
                query += "ss.vir_total_price,\n";
                query += "ss.vir_total_patient_price,\n";
                query += "ss.vir_total_hein_price,\n";
                query += "ss.tdl_execute_room_id,\n";
                query += "tran.exemption,\n";
                query += "(case when ss.patient_type_id=1 then trea.fee_lock_time else tran.transaction_time end) report_time\n";
                query += "from his_sere_serv ss\n";
               
                query += "join his_treatment trea \n";
                query += "on trea.id=ss.tdl_treatment_id\n";

                query += "left join his_sere_serv_bill ssb on ssb.sere_serv_id=ss.id\n";
                query += "left join his_transaction tran \n";
                query += "on tran.id = ssb.bill_id\n";
                query += "and tran.is_cancel is null\n";
                query += "and ss.patient_type_id <>1\n";
                query += string.Format("and tran.transaction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                query += "where 1=1\n";
                query += "and ss.is_delete =0\n";
                query += "and ss.is_no_execute is null\n";
                query += "and ss.is_expend is null\n";
                query += "and (tran.id is not null or \n";
                query += "trea.id is not null\n";
                query += "and ss.patient_type_id = 1 \n";
                query += "and trea.is_active = 0 \n";
                query += string.Format("and trea.fee_lock_time between {0} and {1})\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.IS_BHYT != null)
                {
                    if (filter.IS_BHYT == true)
                    {
                        query += "and ss.patient_type_id = 1\n";
                    }
                    else
                    {
                        query += "and ss.patient_type_id <> 1\n";
                    }
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<DATA_GET>(query);

                if (rs != null)
                {
                    result = rs.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        internal List<SERVICE_PTTT_GROUP> GetServicePtttGroup()
        {
            List<SERVICE_PTTT_GROUP> result = new List<SERVICE_PTTT_GROUP>();
            try
            {
                string query = "";
                query += "--loai pttt\n";
                query += "select \n";
                query += "sv.id service_id,\n";
                query += "pg.pttt_group_code,\n";
                query += "pg.pttt_group_name\n";
               
                query += "from his_service sv\n";
               
                query += "join his_pttt_group pg \n";
                query += "on pg.id=sv.pttt_group_id\n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<SERVICE_PTTT_GROUP>(query);

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
