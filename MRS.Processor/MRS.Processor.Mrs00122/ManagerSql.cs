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
using MRS.Filter;

namespace MRS.Processor.Mrs00122
{
    public partial class ManagerSql : BusinessBase
    {


        public DataTable GetChms(Mrs00122Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select * from( ";
                query += "select ms.medi_stock_name,imm.Medicine_type_code,imm.medicine_type_name,imm.medicine_id,imm.concentra,imm.service_unit_name,imm.imp_price,sum(imm.amount) amount,sum(imm.imp_price*imm.amount) as TOTAL_PRICE from his_rs.v_his_imp_mest_medicine imm join his_rs.his_imp_mest im on im.id = imm.imp_mest_id join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.EXP_TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.EXP_TIME_FROM);
                }
                if (filter.EXP_TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.EXP_TIME_TO);
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND ms.ID ='{0}' ", filter.MEDI_STOCK_ID);
                }
                if (filter.IMP_MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ='{0}' ", filter.IMP_MEDI_STOCK_ID);
                }

                query += "group by ms.medi_stock_name,imm.Medicine_type_code,imm.medicine_type_name,imm.medicine_id,imm.concentra,imm.service_unit_name,imm.imp_price  ";
                query += "union all ";
                query += "select ms.medi_stock_name,imm.material_type_code as medicine_type_code,imm.material_type_name as medicine_type_name,imm.material_id as medicine_id,null concentra,imm.service_unit_name,imm.imp_price,sum(imm.amount) amount,sum(imm.imp_price*imm.amount) as TOTAL_PRICE from his_rs.v_his_imp_mest_material imm join his_rs.his_imp_mest im on im.id = imm.imp_mest_id join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.EXP_TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.EXP_TIME_FROM);
                }
                if (filter.EXP_TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.EXP_TIME_TO);
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND ms.ID ='{0}' ", filter.MEDI_STOCK_ID);
                }
                if (filter.IMP_MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ='{0}' ", filter.IMP_MEDI_STOCK_ID);
                }

                query += "group by ms.medi_stock_name,imm.material_type_code,imm.material_type_name,imm.material_id,imm.service_unit_name,imm.imp_price  ";

                query += ") order by medi_stock_name,medicine_type_name ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<TREATMENT> GetTreatment(Mrs00122Filter filter)
        {
            List<TREATMENT> result = null;
            try
            {
                string query = "";
                query += string.Format("select\n");
                query += string.Format("trea.id,\n");
                query += string.Format("substr(trea.tdl_hein_card_number,1,2) head_card\n");
                query += string.Format("from his_treatment trea\n");
                query += string.Format("join his_exp_mest ex on ex.tdl_treatment_id=trea.id where ex.exp_mest_stt_id=5\n");
                query += string.Format("and ex.finish_time between {0} and {1}\n", filter.EXP_TIME_FROM, filter.EXP_TIME_TO);
                query += string.Format("and substr(trea.tdl_hein_card_number,1,2) in ('TE','HN','CN')\n");

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

    }
    public class TREATMENT
    {
        public long ID { get; set; }
        public string HEAD_CARD { get; set; }


    }
    public class MOBA_IMP_MEST_DETAIL_ID
    {
        public string TYPE { get; set; }
        public long? ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }


    }
}
