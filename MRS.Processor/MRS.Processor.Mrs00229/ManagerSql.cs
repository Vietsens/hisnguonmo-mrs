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

namespace MRS.Processor.Mrs00229
{
    public class ManagerSql
    {

        public List<Mrs00229RDO> Get(Mrs00229Filter filter)
        {
            List<Mrs00229RDO> result = new List<Mrs00229RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("-- dien dieu tri cua phieu xuat\n");
            query += string.Format("select\n");
            query += string.Format("ex.tdl_patient_id,\n");
            query += string.Format("ex.tdl_treatment_id,\n");
            query += string.Format("ex.tdl_treatment_code,\n");
            query += string.Format("ex.tdl_patient_name,\n");
            query += string.Format("exmm.exp_mest_id,\n");
            query += string.Format("exmm.medicine_id,\n");
            query += string.Format("exmm.medicine_type_name,\n");
            query += string.Format("exmm.medicine_type_code,\n");
            query += string.Format("exmm.price,\n");
            query += string.Format("ex.exp_mest_code,\n");
            query += string.Format("ex.tdl_patient_code,\n");
            query += string.Format("ex.tdl_intruction_date,\n");
            query += string.Format("exmm.active_ingr_bhyt_code,\n");
            query += string.Format("exmm.active_ingr_bhyt_name,\n");
            query += string.Format("exmm.concentra,\n");
            query += string.Format("exmm.service_unit_name,\n");
            query += string.Format("exmm.medicine_use_form_name,\n");
            query += string.Format("ex.tdl_patient_dob,\n");
            query += string.Format("exmm.tutorial,\n");
            query += string.Format("sr.request_username,\n");
            query += string.Format("sr.icd_code,\n");
            query += string.Format("sr.icd_name,\n");

            query += string.Format("trea.tdl_patient_type_id,\n");

            query += string.Format("trea.tdl_hein_card_number,\n");

            query += string.Format("trea.tdl_treatment_type_id,\n");
            query += string.Format("sum(EXMM.AMOUNT - NVL(IMMM.TH_AMOUNT,0)) amount\n");

            query += string.Format("from v_his_exp_mest_medicine exmm\n");
            query += string.Format("join his_exp_mest ex on ex.id=exmm.exp_mest_id\n");
            query += string.Format("join his_treatment trea on trea.id=ex.tdl_treatment_id\n");
            query += string.Format("join his_service_req sr on {0}\n", filter.ADD_SALE == true ? "SR.ID=NVL(EX.PRESCRIPTION_ID,EX.SERVICE_REQ_ID)" : "sr.id=ex.service_req_id");
            query += "LEFT JOIN (SELECT TH_EXP_MEST_MEDICINE_ID,SUM(AMOUNT) TH_AMOUNT FROM V_HIS_IMP_MEST_MEDICINE WHERE TH_EXP_MEST_MEDICINE_ID IS NOT NULL AND IS_DELETE =0 AND IMP_MEST_STT_ID=5 GROUP BY TH_EXP_MEST_MEDICINE_ID) IMMM ON EXMM.ID=IMMM.TH_EXP_MEST_MEDICINE_ID \n";
            query += string.Format("where 1=1\n");

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("and sr.request_department_id= {0}\n", filter.DEPARTMENT_ID);
            }

            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("and ex.medi_stock_id = {0}\n", filter.MEDI_STOCK_ID);
            }

            if (filter.EXP_MEST_TYPE_IDs != null)
            {
                query += string.Format("and ex.exp_mest_type_id in ({0})\n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
            }
            query += string.Format("and ex.exp_mest_type_id in ({0},{1},{2},{3})\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);

            if (filter.EXP_MEST_STT_IDs != null)
            {
                query += string.Format("and ex.exp_mest_stt_id in ({0})\n", string.Join(",", filter.EXP_MEST_STT_IDs));
                query += string.Format("and ex.create_time between {0} and {1}\n", filter.EXP_TIME_FROM, filter.EXP_TIME_TO);
            }
            else
            {
                query += string.Format("and ex.exp_mest_stt_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE);
                query += string.Format("and ex.finish_time between {0} and {1}\n", filter.EXP_TIME_FROM, filter.EXP_TIME_TO);
            }
            if (filter.TREATMENT_TYPE_ID != null)
            {
                query += string.Format("and trea.tdl_treatment_type_id = {0}\n", filter.TREATMENT_TYPE_ID);
            }
            query += string.Format("group by\n");
            query += string.Format("ex.tdl_patient_id,\n");
            query += string.Format("ex.tdl_treatment_id,\n");
            query += string.Format("ex.tdl_treatment_code,\n");
            query += string.Format("ex.tdl_patient_name,\n");
            query += string.Format("exmm.exp_mest_id,\n");
            query += string.Format("exmm.medicine_id,\n");
            query += string.Format("exmm.medicine_type_name,\n");
            query += string.Format("exmm.medicine_type_code,\n");
            query += string.Format("exmm.price,\n");
            query += string.Format("ex.exp_mest_code,\n");
            query += string.Format("ex.tdl_patient_code,\n");
            query += string.Format("ex.tdl_intruction_date,\n");
            query += string.Format("exmm.active_ingr_bhyt_code,\n");
            query += string.Format("exmm.active_ingr_bhyt_name,\n");
            query += string.Format("exmm.concentra,\n");
            query += string.Format("exmm.service_unit_name,\n");
            query += string.Format("exmm.medicine_use_form_name,\n");
            query += string.Format("ex.tdl_patient_dob,\n");
            query += string.Format("exmm.tutorial,\n");
            query += string.Format("sr.request_username,\n");
            query += string.Format("sr.icd_code,\n");
            query += string.Format("sr.icd_name,\n");

            query += string.Format("trea.tdl_patient_type_id,\n");

            query += string.Format("trea.tdl_hein_card_number,\n");

            query += string.Format("trea.tdl_treatment_type_id\n");
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00229RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00229");

            return result;
        }
    }
}
