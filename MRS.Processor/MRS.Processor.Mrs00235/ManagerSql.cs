using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00235
{
    internal class ManagerSql
    {
        internal List<V_HIS_EXP_MEST> GetExpMest(string query)
        {
            List<V_HIS_EXP_MEST> result = new List<V_HIS_EXP_MEST>();
            try
            {

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_EXP_MEST>(query) ?? new List<V_HIS_EXP_MEST>();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        internal List<HIS_TREATMENT> GetTreatment(string query)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query) ?? new List<HIS_TREATMENT>();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal List<Mrs00235RDO> GetDetail(Mrs00235Filter filter)
        {
            List<Mrs00235RDO> result = new List<Mrs00235RDO>();
            try
            {
                string query = "select\n";

                query += "em.EXP_MEST_CODE,\n";
                query += "dp.DEPARTMENT_NAME as REQ_DEPARTMENT_NAME,\n";
                query += "dp.DEPARTMENT_CODE as REQ_DEPARTMENT_CODE,\n";
                query += "dp.ROOM_NAME as REQ_ROOM_NAME,\n";
                query += "dp.ROOM_CODE as REQ_ROOM_CODE,\n";
                query += "ms.DEPARTMENT_NAME as IMP_DEPARTMENT_NAME,\n";
                query += "ms.DEPARTMENT_CODE as IMP_DEPARTMENT_CODE,\n";
                query += "ms.MEDI_STOCK_NAME as IMP_MEDI_STOCK_NAME,\n";
                query += "ms.MEDI_STOCK_CODE as IMP_MEDI_STOCK_CODE,\n";
                query += "exmm.EXP_TIME,\n";
                query += "trea.tdl_patient_code PATIENT_CODE,\n";
                query += "trea.tdl_patient_name PATIENT_NAME,\n";
                query += "trea.TREATMENT_CODE,\n";
                query += "ptt.PATIENT_TYPE_NAME,\n";
                query+= "pacl.patient_classify_name,\n";
                query += "sr.ICD_CODE,\n";
                query += "sr.ICD_NAME,\n";
                query += "sr.ICD_SUB_CODE,\n";
                query += "nvl(sr.INTRUCTION_TIME,0) as INTRUCTION_TIME,\n";
                query += "sr.ICD_TEXT,\n";
                query += "emt.EXP_MEST_TYPE_NAME,\n";
                query += "em.EXP_MEST_TYPE_ID,\n";
                query += "em.chms_type_id,\n";
                query += "sptt.PATIENT_TYPE_NAME as TT_PATIENT_TYPE_NAME,\n";
                query += "ops.OTHER_PAY_SOURCE_NAME,\n";
                query += "emr.EXP_MEST_REASON_NAME,\n";
                query += "exmm.MEDICINE_TYPE_CODE MEDI_MATE_CODE,\n";
                query += "exmm.MEDICINE_TYPE_NAME MEDI_MATE_NAME,\n";
                query += "exmm.SERVICE_UNIT_NAME,\n";
                query += "sum(exmm.AMOUNT) amount,\n";
                if (filter.IS_GROUP_IMP_PRICE == true)
                {
                    query += "exmm.IMP_PRICE * (1 + exmm.IMP_VAT_RATIO) PRICE,\n";
                }
                else
                {
                    query += " nvl(exmm.PRICE, 0) * (1 + nvl(exmm.VAT_RATIO,0)) PRICE,\n";
                }
                query += "sum(case when reu.is_reusabling>=1 then exmm.AMOUNT * nvl(exmm.PRICE, 0) * (1 + nvl(exmm.VAT_RATIO,0)) else 0 end) TOTAL_REUSABLE_EXP,\n";
                query += "sum(exmm.AMOUNT * nvl(exmm.PRICE, 0) * (1 + nvl(exmm.VAT_RATIO,0))) total_price,\n";
                query += "sum(exmm.IMP_PRICE * (1 + exmm.IMP_VAT_RATIO) * exmm.AMOUNT) IMP_TOTAL_PRICE,\n";
                query += "1\n";
                query += "from his_exp_mest em\n";
                query += "left join his_treatment trea on em.tdl_treatment_id = trea.id\n";
                query += "left join his_service_req sr on em.service_req_id = sr.id\n";
                query += "join V_HIS_EXP_MEST_MEDICINE exmm on em.id = exmm.exp_mest_id\n";
                //query += "left join v_his_sere_serv ss on ss.exp_mest_medicine_id = exmm.id \n";
                query += "left join his_patient_type sptt on sptt.id = exmm.patient_type_id\n";
                query += "left join his_patient_type ptt on ptt.id = trea.tdl_patient_type_id\n";
                query += "left join his_exp_mest_type emt on emt.id = em.exp_mest_type_id\n";
                query += "left join v_his_medi_stock ms on ms.id = em.imp_medi_stock_id\n";
                query += "left join his_exp_mest_reason emr on emr.id = em.exp_mest_reason_id\n";
                query += "left join v_his_room dp on dp.id = em.req_room_id\n";
                query += "left join his_other_pay_source ops on ops.id = exmm.other_pay_source_id\n";
                query += "left join his_patient_classify pacl on pacl.id = trea.tdl_patient_classify_id\n";
                query += string.Format("left join lateral (select (case when min(imp_time)>{0} then 2 else 1 end) IS_REUSABLING from v_his_imp_mest_material  where 1=1 and imp_mest_stt_id=5 and imp_mest_type_id in (17) and imp_time <{1} and material_type_id in (select id from his_material_type where is_reusable=1) and material_id=exmm.MEDICINE_id) reu on 1=1\n", filter.TIME_FROM, filter.TIME_TO);
                query += "where 1=1 and em.exp_mest_stt_id =5 and exmm.is_export=1\n";
                if (filter.REQ_LOGINNAMEs != null)
                {
                    query += string.Format("and em.req_loginname in ('{0}')\n", string.Join("','", filter.REQ_LOGINNAMEs));
                }
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_CLASSIFY_ID in ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and em.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and exmm.patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.EXP_MEST_TYPE_IDs != null)
                {
                    query += string.Format("and em.exp_mest_type_id in ({0})\n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
                }
                if (filter.EXP_MEST_REASON_IDs!=null)
                {
                    query += string.Format("and em.EXP_MEST_REASON_ID in ({0})\n",string.Join(",",filter.EXP_MEST_REASON_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and em.req_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.IS_HPKP == true)
                {
                    query += string.Format("and em.exp_mest_type_id in ({0})\n", string.Join(",", new List<long> { IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP }));
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("and em.MEDI_STOCK_ID = {0}\n", filter.MEDI_STOCK_ID);
                }
                if (filter.IS_NOI_NGOAI_TRU == false)
                {
                    query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                }
                else if (filter.IS_NOI_NGOAI_TRU == true)
                {
                    query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                }
                if (!string.IsNullOrWhiteSpace(filter.PATIENT_NAMEs))
                {
                    query += string.Format("and (em.tdl_patient_name like '%{0}%' or em.tdl_patient_code like '%{0}%')", filter.PATIENT_NAMEs);
                }
                if (filter.IS_EXP_DATE == true)
                {
                    query += string.Format("and exmm.exp_date between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("and em.finish_date between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                }
                query += "group by\n";
                query += "dp.DEPARTMENT_NAME,\n";
                query += "dp.DEPARTMENT_CODE,\n";
                query += "dp.ROOM_NAME,\n";
                query += "dp.ROOM_CODE,\n";
                query += "ms.DEPARTMENT_NAME,\n";
                query += "ms.DEPARTMENT_CODE,\n";
                query += "ms.MEDI_STOCK_NAME,\n";
                query += "ms.MEDI_STOCK_CODE,\n";
                query += "em.EXP_MEST_CODE,\n";
                query += "emr.EXP_MEST_REASON_NAME,\n";
                query += "exmm.EXP_TIME,\n";
                query += "trea.tdl_patient_code ,\n";
                query += "trea.tdl_patient_name ,\n";
                query += "trea.TREATMENT_CODE,\n";
                query += "pacl.patient_classify_name,\n";
                query += "ptt.PATIENT_TYPE_NAME,\n";
                query += "sptt.PATIENT_TYPE_NAME,\n";
                query += "ops.OTHER_PAY_SOURCE_NAME,\n";
                query += "nvl(sr.INTRUCTION_TIME,0),\n";
                query += "sr.ICD_CODE,\n";
                query += "sr.ICD_NAME,\n";
                query += "sr.ICD_SUB_CODE,\n";
                query += "sr.ICD_TEXT,\n";
                query += "emt.EXP_MEST_TYPE_NAME,\n";
                query += "exmm.MEDICINE_TYPE_CODE,\n";
                query += "exmm.MEDICINE_TYPE_NAME,\n";
                query += "exmm.SERVICE_UNIT_NAME,\n";
                if (filter.IS_GROUP_IMP_PRICE == true)
                {
                    query += "exmm.IMP_PRICE * (1 + exmm.IMP_VAT_RATIO),\n";
                }
                else
                {
                    query += " nvl(exmm.PRICE, 0) * (1 + nvl(exmm.VAT_RATIO,0)),\n";
                }
                query += "em.EXP_MEST_TYPE_ID,\n";
                query += "em.chms_type_id,\n";
                query += "1\n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result.AddRange(new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00235RDO>(query) ?? new List<Mrs00235RDO>());
                query = query.Replace("MEDICINE", "MATERIAL");
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result.AddRange(new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00235RDO>(query) ?? new List<Mrs00235RDO>());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal List<Mrs00235RDO> GetTotalExpMest(Mrs00235Filter filter)
        {
            List<Mrs00235RDO> result = new List<Mrs00235RDO>();
            try
            {
                string query = "select\n";

                query += "em.REQ_DEPARTMENT_ID,\n";
                query += "dp.DEPARTMENT_NAME as REQ_DEPARTMENT_NAME,\n";
                query += "dp.DEPARTMENT_CODE as REQ_DEPARTMENT_CODE,\n";
                query += "dp.ROOM_NAME as REQ_ROOM_NAME,\n";
                query += "dp.ROOM_CODE as REQ_ROOM_CODE,\n";
                query += "ms.DEPARTMENT_NAME as IMP_DEPARTMENT_NAME,\n";
                query += "ms.DEPARTMENT_CODE as IMP_DEPARTMENT_CODE,\n";
                query += "ms.MEDI_STOCK_NAME as IMP_MEDI_STOCK_NAME,\n";
                query += "ms.MEDI_STOCK_CODE as IMP_MEDI_STOCK_CODE,\n";
                query += "em.REQ_ROOM_ID,\n";
                query += "em.id exp_mest_id,\n";
                query += "em.exp_mest_code,\n";
                query += "sum(case when reu.is_reusabling>=1 then exmm.AMOUNT * nvl(exmm.PRICE, 0) * (1 + nvl(exmm.VAT_RATIO,0)) else 0 end) TOTAL_REUSABLE_EXP,\n";
                query += "sum(exmm.AMOUNT * nvl(exmm.PRICE, 0) * (1 + nvl(exmm.VAT_RATIO,0))) total_price,\n";
                query += "sum(exmm.IMP_PRICE * (1 + exmm.IMP_VAT_RATIO) * exmm.AMOUNT) IMP_TOTAL_PRICE,\n";
                query += "emt.EXP_MEST_TYPE_NAME,\n";
                query += "em.EXP_MEST_TYPE_ID,\n";
                query += "em.FINISH_TIME EXP_TIME,\n";
                query += "trea.tdl_patient_code PATIENT_CODE,\n";
                query += "trea.tdl_patient_name PATIENT_NAME,\n";
                query += "trea.TREATMENT_CODE,\n";
                query += "ptt.PATIENT_TYPE_NAME,\n";
                query += "sr.ICD_CODE,\n";
                query += "sr.ICD_NAME,\n";
                query += "sr.ICD_SUB_CODE,\n";
                query += "sr.ICD_TEXT,\n";
                query += "1\n";
                query += "from his_exp_mest em\n";
                query += "left join his_treatment trea on em.tdl_treatment_id = trea.id\n";
                query += "left join his_service_req sr on em.service_req_id = sr.id\n";
                query += "left join V_HIS_EXP_MEST_MEDICINE exmm on em.id = exmm.exp_mest_id\n";
                query += string.Format("left join lateral (select (case when min(imp_time)>{0} then 2 else 1 end) IS_REUSABLING from v_his_imp_mest_material  where 1=1 and imp_mest_stt_id=5 and imp_mest_type_id in (17) and imp_time <{1} and material_type_id in (select id from his_material_type where is_reusable=1) and material_id=exmm.MEDICINE_id) reu on 1=1\n", filter.TIME_FROM, filter.TIME_TO);
                query += "left join his_patient_type ptt on ptt.id = trea.tdl_patient_type_id\n";
                query += "left join his_exp_mest_type emt on emt.id = em.exp_mest_type_id\n";
                query += "left join v_his_room dp on dp.id = em.req_department_id\n";
                query += "left join v_his_medi_stock ms on ms.id = em.imp_medi_stock_id\n";

                query += "where 1=1 and em.exp_mest_stt_id =5\n";
                if (filter.REQ_LOGINNAMEs != null)
                {
                    query += string.Format("and em.req_loginname in ('{0}')\n", string.Join("','", filter.REQ_LOGINNAMEs));
                }
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_CLASSIFY_ID in ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and em.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.EXP_MEST_TYPE_IDs != null)
                {
                    query += string.Format("and em.exp_mest_type_id in ({0})\n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and em.req_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.EXP_MEST_REASON_IDs!=null)
                {
                    query += string.Format("and em.EXP_MEST_REASON_ID in ({0})\n",string.Join(",",filter.EXP_MEST_REASON_IDs));
                }
                if (filter.IS_HPKP == true)
                {
                    query += string.Format("and em.exp_mest_type_id in ({0})\n", string.Join(",", new List<long> { IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP }));
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("and em.MEDI_STOCK_ID = {0}\n", filter.MEDI_STOCK_ID);
                }
                if (filter.IS_NOI_NGOAI_TRU == false)
                {
                    query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                }
                else if (filter.IS_NOI_NGOAI_TRU == true)
                {
                    query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                }
                if (!string.IsNullOrWhiteSpace(filter.PATIENT_NAMEs))
                {
                    query += string.Format("and (em.tdl_patient_name like '%{0}%' or em.tdl_patient_code like '%{0}%')", filter.PATIENT_NAMEs);
                }
                if (filter.IS_EXP_DATE == true)
                {
                    query += string.Format("and exmm.exp_date between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("and em.finish_date between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                }
                query += "group by\n";

                query += "em.REQ_DEPARTMENT_ID,\n";
                query += "dp.DEPARTMENT_NAME,\n";
                query += "dp.DEPARTMENT_CODE,\n";
                query += "dp.ROOM_NAME,\n";
                query += "dp.ROOM_CODE,\n";
                query += "ms.DEPARTMENT_NAME,\n";
                query += "ms.DEPARTMENT_CODE,\n";
                query += "ms.MEDI_STOCK_NAME,\n";
                query += "ms.MEDI_STOCK_CODE,\n";
                query += "em.REQ_ROOM_ID,\n";
                query += "em.id,\n";
                query += "em.exp_mest_code,\n";
                query += "emt.EXP_MEST_TYPE_NAME,\n";
                query += "em.EXP_MEST_TYPE_ID,\n";
                query += "em.FINISH_TIME,\n";
                query += "trea.tdl_patient_code,\n";
                query += "trea.tdl_patient_name,\n";
                query += "trea.TREATMENT_CODE,\n";
                query += "ptt.PATIENT_TYPE_NAME,\n";
                query += "sr.ICD_CODE,\n";
                query += "sr.ICD_NAME,\n";
                query += "sr.ICD_SUB_CODE,\n";
                query += "sr.ICD_TEXT,\n";
                query += "1\n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result.AddRange(new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00235RDO>(query) ?? new List<Mrs00235RDO>());
                var ids = result.Where(o=>o.EXP_MEST_ID>0).Select(o=>o.EXP_MEST_ID??0).ToList();
                query = query.Replace("MEDICINE", "MATERIAL");
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00235RDO>(query) ?? new List<Mrs00235RDO>();
                foreach (var r in rs)
                { 
                    if(ids.Contains(r.EXP_MEST_ID??0))
                    {
                        Mrs00235RDO rdo = result.FirstOrDefault(o => o.EXP_MEST_ID == r.EXP_MEST_ID);
                        //nếu vật tư nằm trong phiếu thuốc rồi thì cộng tiền vào phiếu thuốc, ngược lại thêm phiếu vật tư vào danh sách phiếu
                        if(rdo !=null)
                        {
                            rdo.TOTAL_PRICE = (rdo.TOTAL_PRICE??0)+ (r.TOTAL_PRICE ?? 0);
                            rdo.IMP_TOTAL_PRICE = (rdo.IMP_TOTAL_PRICE ?? 0) + (r.IMP_TOTAL_PRICE ?? 0);
                        } 
                        else
                        {
                            result.Add(r);
                        }    
                    }    
                } 
              
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
