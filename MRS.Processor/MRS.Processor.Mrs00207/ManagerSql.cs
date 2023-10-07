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

namespace MRS.Processor.Mrs00207
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00207RDO> GetServiceReq(Mrs00207Filter filter)
        {
            List<Mrs00207RDO> result = new List<Mrs00207RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT \n";
            query += "'THUOC' TYPE, \n";
            query += "(EXMM.AMOUNT - NVL(IMMM.TH_AMOUNT,0)) AMOUNT_TRUST, \n";
            query += "NVL(EXMM.PRICE,0)*(1+NVL(EXMM.VAT_RATIO,0))*(EXMM.AMOUNT - NVL(IMMM.TH_AMOUNT,0)) TOTAL_PRICE, \n";
            query += "trea.TDL_PATIENT_PHONE, \n";
            query += "trea.PATIENT_ID, \n";
            query += "ex.TDL_PATIENT_NAME VIR_PATIENT_NAME, \n";
            query += "ex.TDL_PATIENT_CODE tdl_patient_code, \n";
            query += "EX.TDL_PATIENT_DOB, \n";
            query += "(case when ex.tdl_patient_gender_name = 'Nam' then '02' when ex.tdl_patient_gender_name = 'Nữ' then '01' else '03' end) TDL_PATIENT_GENDER_CODE, \n";
            query += "ex.TDL_PATIENT_GENDER_NAME, \n";
            query += "SUBSTR(TREA.TDL_HEIN_CARD_NUMBER,1,2) HEAD_CARD, \n";
            query += "TREA.TDL_HEIN_CARD_NUMBER, \n";
            query += "ex.TDL_TREATMENT_CODE, \n";
            query += "SR.SERVICE_REQ_CODE, \n";
            query += "NVL(SR.INTRUCTION_TIME,EX.TDL_INTRUCTION_TIME) INTRUCTION_TIME, \n";
            query += "nvl(SR.intruction_date,ex.tdl_intruction_date) INTRUCTION_DATE, \n";
            query += "(case when ex.TDL_PRESCRIPTION_REQ_LOGINNAME is not null and ex.TDL_PRESCRIPTION_REQ_USERNAME is not null then ex.tdl_prescription_req_loginname else nvl(SR.REQUEST_LOGINNAME,'KHAC') end) request_loginname, \n";
            query += "(case when ex.TDL_PRESCRIPTION_REQ_LOGINNAME is not null and ex.TDL_PRESCRIPTION_REQ_USERNAME is not null then ex.tdl_prescription_req_username else nvl(SR.REQUEST_userNAME,'Khác') end) request_username, \n";
            query += "EMT.EXP_MEST_TYPE_CODE, \n";
            query += "EMT.EXP_MEST_TYPE_NAME, \n";
            query += "TREA.IN_TIME, \n";
            query += "TREA.OUT_TIME, \n";
            query += "TREA.TREATMENT_DAY_COUNT, \n";
            query += "nvl(ex.icd_code,nvl(sr.icd_code,TREA.ICD_CODE)) icd_code, \n";
            query += "TREA.ICD_SUB_CODE, \n";
            query += "nvl(ex.icd_name,nvl(sr.icd_name,TREA.ICD_name)) icd_name, \n";
            query += "TREA.ICD_TEXT, \n";
            query += "ex.TDL_PATIENT_ADDRESS, \n";
            query += "TMT.TREATMENT_TYPE_CODE, \n";
            query += "TMT.TREATMENT_TYPE_NAME, \n";
            query += "PTT.PATIENT_TYPE_CODE TDL_PATIENT_TYPE_CODE, \n";
            query += "PTT.PATIENT_TYPE_NAME TDL_PATIENT_TYPE_NAME, \n";
            query += "RD.BHYT_CODE REQUEST_DEPARTMENT_BHYT_CODE, \n";
            query += "RR.DEPARTMENT_CODE REQUEST_DEPARTMENT_CODE, \n";
            query += "RR.DEPARTMENT_NAME REQUEST_DEPARTMENT_NAME, \n";
            query += "RR.ROOM_CODE REQUEST_ROOM_CODE, \n";
            query += "RR.ROOM_NAME REQUEST_ROOM_NAME, \n";
            query += "MS.MEDI_STOCK_CODE, \n";
            query += "MS.MEDI_STOCK_NAME, \n";
            query += "SV.SERVICE_CODE, \n";
            query += "SV.SERVICE_NAME, \n";
            if (filter.ADD_PAR_SERVICE == true)
            {
                query += "SVSS.SS_SERVICE_NAME, \n";
            }
            if (filter.ADD_REQ_BED == true)
            {
                query += "BL.BED_CODE, \n";
                query += "BL.BED_NAME, \n";
            }
            query += "TET.TREATMENT_END_TYPE_CODE, \n";
            query += "TET.TREATMENT_END_TYPE_NAME, \n";
            query += "NVL(SS.VIR_TOTAL_HEIN_PRICE,0) VIR_TOTAL_HEIN_PRICE, \n";

            //query += "SRC.CATEGORY_CODE, \n";
            //query += "SRC.CATEGORY_NAME, \n";
            query += "ME.TDL_BID_PACKAGE_CODE, \n";
            query += "NVL(AGG.EXP_MEST_CODE,EXMM.EXP_MEST_CODE) AGGR_EXP_MEST_CODE, \n";
            query += @"EXMM.ID,
EXMM.CREATE_TIME,
EXMM.MODIFY_TIME,
EXMM.IS_ACTIVE,
EXMM.IS_DELETE,
EXMM.BK_AMOUNT,
EXMM.EXP_MEST_ID,
EXMM.MEDICINE_ID,
EXMM.TDL_MEDI_STOCK_ID,
EXMM.MEDI_STOCK_PERIOD_ID,
EXMM.TDL_MEDICINE_TYPE_ID,
EXMM.TDL_AGGR_EXP_MEST_ID,
EXMM.EXP_MEST_METY_REQ_ID,
EXMM.CK_IMP_MEST_MEDICINE_ID,
EXMM.IS_EXPORT,
EXMM.AMOUNT,
EXMM.PRICE,
EXMM.VAT_RATIO,
EXMM.DISCOUNT,
EXMM.NUM_ORDER,
EXMM.DESCRIPTION,
EXMM.APPROVAL_TIME,
EXMM.APPROVAL_DATE,
EXMM.EXP_LOGINNAME,
EXMM.EXP_USERNAME,
EXMM.EXP_TIME,
EXMM.EXP_DATE,
EXMM.TH_AMOUNT,
EXMM.PATIENT_TYPE_ID,
EXMM.SERE_SERV_PARENT_ID,
EXMM.IS_EXPEND,
EXMM.IS_OUT_PARENT_FEE,
EXMM.USE_TIME_TO,
EXMM.TUTORIAL,
EXMM.TDL_SERVICE_REQ_ID,
EXMM.TDL_TREATMENT_ID,
EXMM.IS_USE_CLIENT_PRICE,
EXMM.VIR_PRICE,
EXMM.VACCINATION_RESULT_ID,
EXMM.TDL_VACCINATION_ID,
EXMM.SPEED,
EXMM.EXPEND_TYPE_ID,
EXMM.IS_NOT_PRES,
EXMM.USE_ORIGINAL_UNIT_FOR_PRES,
EXMM.BCS_REQ_AMOUNT,
EXMM.DAY_COUNT,
EXMM.MORNING,
EXMM.NOON,
EXMM.AFTERNOON,
EXMM.EVENING,
EXMM.HTU_ID,
EXMM.BREATH_SPEED,
EXMM.BREATH_TIME,
EXMM.PREVIOUS_USING_COUNT,
EXMM.IS_USED,
EXMM.SERVICE_CONDITION_ID,
EXMM.EXP_MEST_CODE,
EXMM.EXP_MEST_TYPE_ID,
EXMM.AGGR_EXP_MEST_ID,
EXMM.MEDI_STOCK_ID,
EXMM.EXP_MEST_STT_ID,
EXMM.REQ_ROOM_ID,
EXMM.REQ_DEPARTMENT_ID,
EXMM.TDL_INTRUCTION_TIME,
EXMM.TDL_PRES_REQ_USER_TITLE,
EXMM.IMP_PRICE,
EXMM.IMP_VAT_RATIO,
EXMM.BID_ID,
EXMM.PACKAGE_NUMBER,
EXMM.EXPIRED_DATE,
EXMM.MEDICINE_TYPE_ID,
EXMM.INTERNAL_PRICE,
EXMM.IMP_TIME,
EXMM.SUPPLIER_ID,
EXMM.MEDICINE_TCY_NUM_ORDER,
EXMM.MEDICINE_BYT_NUM_ORDER,
EXMM.MEDICINE_REGISTER_NUMBER,
EXMM.ACTIVE_INGR_BHYT_CODE,
EXMM.ACTIVE_INGR_BHYT_NAME,
EXMM.CONCENTRA,
EXMM.MEDICINE_TYPE_CODE,
EXMM.MEDICINE_TYPE_NAME,
EXMM.SERVICE_ID,
EXMM.NATIONAL_NAME,
EXMM.MANUFACTURER_ID,
EXMM.MEDICINE_TYPE_NUM_ORDER,
EXMM.TCY_NUM_ORDER,
EXMM.BYT_NUM_ORDER,
EXMM.REGISTER_NUMBER,
EXMM.IS_FUNCTIONAL_FOOD,
EXMM.MEMA_GROUP_ID,
EXMM.IS_ALLOW_ODD,
EXMM.MEDICINE_GROUP_ID,
EXMM.SERVICE_UNIT_ID,
EXMM.RECORDING_TRANSACTION,
EXMM.MEDICINE_USE_FORM_ID,
EXMM.IS_OUT_HOSPITAL,
EXMM.SERVICE_UNIT_CODE,
EXMM.SERVICE_UNIT_NAME,
EXMM.MEDICINE_NUM_ORDER,
EXMM.MATERIAL_NUM_ORDER,
EXMM.CONVERT_RATIO,
EXMM.MANUFACTURER_CODE,
EXMM.MANUFACTURER_NAME,
EXMM.SUPPLIER_CODE,
EXMM.SUPPLIER_NAME,
EXMM.BID_NUMBER,
EXMM.BID_NAME,
EXMM.PATIENT_TYPE_CODE,
EXMM.PATIENT_TYPE_NAME,
EXMM.MEDICINE_GROUP_CODE,
EXMM.MEDICINE_GROUP_NAME,
EXMM.MEDICINE_GROUP_NUM_ORDER,
EXMM.MEDICINE_USE_FORM_CODE,
EXMM.MEDICINE_USE_FORM_NAME,
EXMM.MEDICINE_USE_FORM_NUM_ORDER 
";

            query += "FROM V_HIS_EXP_MEST_MEDICINE EXMM \n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=EXMM.EXP_MEST_ID \n";
            query += "JOIN HIS_MEDICINE ME ON ME.ID=EXMM.MEDICINE_ID \n";
            query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID=NVL(EX.PRESCRIPTION_ID,EX.SERVICE_REQ_ID) \n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=Ex.TDL_TREATMENT_ID \n";
            query += "LEFT JOIN HIS_TREATMENT_TYPE TMT ON TMT.ID=TREA.TDL_TREATMENT_TYPE_ID \n";
            query += "LEFT JOIN HIS_PATIENT_TYPE PTT ON PTT.ID=TREA.TDL_PATIENT_TYPE_ID \n";
            query += "LEFT JOIN HIS_MEDI_STOCK MS ON MS.ID=EX.MEDI_STOCK_ID \n";
            query += "LEFT JOIN V_HIS_ROOM RR ON RR.ID=SR.REQUEST_ROOM_ID \n";
            query += "LEFT JOIN HIS_DEPARTMENT RD ON RD.ID=SR.REQUEST_DEPARTMENT_ID \n";
            query += "LEFT JOIN HIS_TREATMENT_END_TYPE TET ON TET.ID=TREA.TREATMENT_END_TYPE_ID \n";
            query += "LEFT JOIN HIS_EXP_MEST_TYPE EMT ON EMT.ID=EX.EXP_MEST_TYPE_ID \n";
            query += "LEFT JOIN HIS_IMP_MEST IM ON EX.ID=IM.MOBA_EXP_MEST_ID \n";
            query += "LEFT JOIN HIS_EXP_MEST AGG ON AGG.ID=EXMM.AGGR_EXP_MEST_ID \n";
            query += "LEFT JOIN LATERAL (SELECT SUM(AMOUNT) TH_AMOUNT FROM V_HIS_IMP_MEST_MEDICINE WHERE TH_EXP_MEST_MEDICINE_ID =EXMM.ID AND IS_DELETE =0 AND IMP_MEST_STT_ID=5) IMMM ON 1=1 \n";
            query += "LEFT JOIN HIS_SERE_SERV SS ON (SS.EXP_MEST_MEDICINE_ID=EXMM.ID AND SS.PATIENT_TYPE_ID=1) \n";//chỉ join với ss bảo hiểm để lấy tiền bảo hiểm trả
            if (filter.ADD_PAR_SERVICE == true)
            {
                query += @"left join lateral(
select 
Listagg(pr.tdl_service_name, '; ') Within Group (Order By 1) ss_service_name 
from his_service_req prsr  
join his_sere_serv pr on pr.service_req_id=prsr.id 
where 1=1
and prsr.id=sr.parent_id
) SVSS on 1=1
";
            }
            if (filter.ADD_REQ_BED == true)
            {
                query += @"left join lateral(
select 
min(bl.bed_code) keep(dense_rank first order by bl.start_time) bed_code,
min(bl.bed_name) keep(dense_rank first order by bl.start_time) bed_name
from v_his_bed_log bl  
join his_bed_room br on br.id=bl.bed_room_id
where 1=1
and br.room_id=SR.REQUEST_ROOM_ID
and bl.treatment_id = trea.id
and bl.start_time<sr.intruction_time
) bl on 1=1
";
            }
            query += "LEFT JOIN his_service SV on SV.ID = exmm.service_id  \n";
            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 \n";

            query += string.Format("AND (sr.id is not null or ex.exp_mest_type_id ={0})\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            if (filter.INPUT_DATA_ID_TIME_TYPE !=null)
            {
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND NVL(SR.INTRUCTION_TIME,EX.TDL_INTRUCTION_TIME) BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND EX.bill_id in (select id from his_transaction where is_cancel is null and transaction_time BETWEEN {0} AND {1}) \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND ss.hein_approval_id in (select id from his_hein_approval where is_delete=0 and execute_time BETWEEN {0} AND {1}) \n", filter.TIME_FROM, filter.TIME_TO);
                }
            }
            else
            {
                if (filter.TRUE_FALSE.HasValue && !filter.TRUE_FALSE.Value)
                {
                    query += string.Format("AND NVL(SR.INTRUCTION_TIME,EX.TDL_INTRUCTION_TIME)  BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
            }
            
            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID ={0} \n", filter.DEPARTMENT_ID);
            }
            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
            }
            if (filter.EMPLOYEE_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND exists (select 1 from his_employee emp where emp.DEPARTMENT_ID IN({0}) and (ex.tdl_prescription_req_loginname is not null and emp.loginname=ex.tdl_prescription_req_loginname or ex.tdl_prescription_req_loginname is null and emp.loginname=sr.request_loginname)) \n", string.Join(",", filter.EMPLOYEE_DEPARTMENT_IDs));
            }

            if (filter.ICD_CODEs != null)
            {
                query += string.Format("AND nvl(ex.icd_code,nvl(sr.icd_code,TREA.ICD_CODE)) IN('{0}') \n", string.Join("','", filter.ICD_CODEs));
            }

            if (filter.EXAM_ROOM_ID != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID ={0} \n", filter.EXAM_ROOM_ID);
            }
            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID IN({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
            }
            if (filter.MEDICINE_TYPE_ID != null)
            {
                query += string.Format("AND EXMM.MEDICINE_TYPE_ID ={0} \n", filter.MEDICINE_TYPE_ID);
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.MEDICINE_TYPE_ID IN({0}) \n", string.Join(",", filter.MEDICINE_TYPE_IDs));
            }
            if (filter.TREATMENT_TYPE_ID != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID ={0} \n", filter.TREATMENT_TYPE_ID);
            }
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            if (filter.PATIENT_TYPE_ID != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
            }
            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }
            if (filter.MEDICINE_GROUP_IDs != null)
            {
                query += string.Format("AND EXMM.MEDICINE_GROUP_ID IN({0}) \n", string.Join(",", filter.MEDICINE_GROUP_IDs));
            }
            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.EXP_MEST_TYPE_ID != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID ={0} \n", filter.EXP_MEST_TYPE_ID);
            }
            if (filter.EXP_MEST_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
            }
            if (filter.ADD_SALE != true)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID <>{0} \n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            }
            if (filter.REPORT_TYPE_CAT_ID != null)
            {
                query += string.Format("AND EXISTS (SELECT 1 FROM HIS_SERVICE_RETY_CAT WHERE SERVICE_ID = EXMM.SERVICE_ID AND REPORT_TYPE_CAT_ID = {0})\n", filter.REPORT_TYPE_CAT_ID);
            }
            if (filter.REPORT_TYPE_CAT_IDs != null)
            {
                query += string.Format("AND EXISTS (SELECT 1 FROM HIS_SERVICE_RETY_CAT WHERE SERVICE_ID = EXMM.SERVICE_ID AND REPORT_TYPE_CAT_ID IN({0})) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
            }
            if (filter.REPORT_TYPE_CAT_IDs != null)
            {
                query += string.Format("AND EXISTS (SELECT 1 FROM HIS_SERVICE_RETY_CAT WHERE SERVICE_ID = EXMM.SERVICE_ID AND REPORT_TYPE_CAT_ID IN({0})) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                query += string.Format("AND EXISTS (SELECT 1 FROM his_medicine_type mt1 join his_medicine_type pr1 on pr1.id=mt1.parent_id WHERE mt1.id = EXMM.medicine_type_id AND pr1.service_id IN({0})) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
            }
            if (filter.DOCTOR_LOGINNAMEs != null)
            {
                query += string.Format("AND nvl(SR.REQUEST_LOGINNAME,ex.tdl_prescription_req_loginname) IN('{0}') \n", string.Join("','", filter.DOCTOR_LOGINNAMEs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00207RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00207");

            return result;
        }
        public List<Mrs00207RDO> GetServiceReqMaterial(Mrs00207Filter filter)
        {
            List<Mrs00207RDO> result = new List<Mrs00207RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT \n";
            query += "'VATTU' TYPE, \n";
            query += "(EXMM.AMOUNT - NVL(IMMM.TH_AMOUNT,0)) AMOUNT_TRUST, \n";
            query += "NVL(EXMM.PRICE,0)*(1+NVL(EXMM.VAT_RATIO,0))*(EXMM.AMOUNT - NVL(IMMM.TH_AMOUNT,0)) TOTAL_PRICE, \n";
            query += "trea.PATIENT_ID, \n";
            query += "trea.TDL_PATIENT_PHONE, \n";
            query += "ex.TDL_PATIENT_NAME VIR_PATIENT_NAME, \n";
            query += "ex.TDL_PATIENT_CODE tdl_patient_code, \n";
            query += "EX.TDL_PATIENT_DOB, \n";
            query += "(case when ex.tdl_patient_gender_name = 'Nam' then '02' when ex.tdl_patient_gender_name = 'Nữ' then '01' else '03' end) TDL_PATIENT_GENDER_CODE, \n";
            query += "ex.TDL_PATIENT_GENDER_NAME, \n";
            query += "SUBSTR(TREA.TDL_HEIN_CARD_NUMBER,1,2) HEAD_CARD, \n";
            query += "TREA.TDL_HEIN_CARD_NUMBER, \n";
            query += "ex.TDL_TREATMENT_CODE, \n";
            query += "SR.SERVICE_REQ_CODE, \n";
            query += "NVL(SR.INTRUCTION_TIME,EX.TDL_INTRUCTION_TIME) INTRUCTION_TIME, \n";
            query += "nvl(SR.intruction_date,ex.tdl_intruction_date) INTRUCTION_DATE, \n";
            query += "(case when ex.TDL_PRESCRIPTION_REQ_LOGINNAME is not null and ex.TDL_PRESCRIPTION_REQ_USERNAME is not null then ex.tdl_prescription_req_loginname else nvl(SR.REQUEST_LOGINNAME,'KHAC') end) request_loginname, \n";
            query += "(case when ex.TDL_PRESCRIPTION_REQ_LOGINNAME is not null and ex.TDL_PRESCRIPTION_REQ_USERNAME is not null then ex.tdl_prescription_req_username else nvl(SR.REQUEST_userNAME,'Khác') end) request_username, \n";
            query += "EMT.EXP_MEST_TYPE_CODE, \n";
            query += "EMT.EXP_MEST_TYPE_NAME, \n";
            query += "TREA.IN_TIME, \n";
            query += "TREA.OUT_TIME, \n";
            query += "TREA.TREATMENT_DAY_COUNT, \n";
            query += "nvl(ex.icd_code,nvl(sr.icd_code,TREA.ICD_CODE)) icd_code, \n";
            query += "TREA.ICD_SUB_CODE, \n";
            query += "nvl(ex.icd_name,nvl(sr.icd_name,TREA.ICD_name)) icd_name, \n";
            query += "TREA.ICD_TEXT, \n";
            query += "ex.TDL_PATIENT_ADDRESS, \n";
            query += "TMT.TREATMENT_TYPE_CODE, \n";
            query += "TMT.TREATMENT_TYPE_NAME, \n";
            query += "PTT.PATIENT_TYPE_CODE TDL_PATIENT_TYPE_CODE, \n";
            query += "PTT.PATIENT_TYPE_NAME TDL_PATIENT_TYPE_NAME, \n";
            query += "RD.BHYT_CODE REQUEST_DEPARTMENT_BHYT_CODE, \n";
            query += "RR.DEPARTMENT_CODE REQUEST_DEPARTMENT_CODE, \n";
            query += "RR.DEPARTMENT_NAME REQUEST_DEPARTMENT_NAME, \n";
            query += "RR.ROOM_CODE REQUEST_ROOM_CODE, \n";
            query += "RR.ROOM_NAME REQUEST_ROOM_NAME, \n";
            query += "MS.MEDI_STOCK_CODE, \n";
            query += "MS.MEDI_STOCK_NAME, \n";
            query += "SV.SERVICE_CODE, \n";
            query += "SV.SERVICE_NAME, \n";
            if (filter.ADD_PAR_SERVICE == true)
            {
                query += "SVSS.SS_SERVICE_NAME, \n";
            }
            if (filter.ADD_REQ_BED == true)
            {
                query += "BL.BED_CODE, \n";
                query += "BL.BED_NAME, \n";
            }
            query += "TET.TREATMENT_END_TYPE_CODE, \n";
            query += "TET.TREATMENT_END_TYPE_NAME, \n";
            query += "NVL(SS.VIR_TOTAL_HEIN_PRICE,0) VIR_TOTAL_HEIN_PRICE, \n";

            //query += "SRC.CATEGORY_CODE, \n";
            //query += "SRC.CATEGORY_NAME, \n";
            query += "ME.TDL_BID_PACKAGE_CODE, \n";
            query += "NVL(AGG.EXP_MEST_CODE,EXMM.EXP_MEST_CODE) AGGR_EXP_MEST_CODE, \n";
            query += @"EXMM.ID,
EXMM.CREATE_TIME,
EXMM.MODIFY_TIME,
EXMM.IS_ACTIVE,
EXMM.IS_DELETE,
EXMM.BK_AMOUNT,
EXMM.EXP_MEST_ID,
EXMM.MATERIAL_ID as MEDICINE_ID,
EXMM.TDL_MEDI_STOCK_ID,
EXMM.MEDI_STOCK_PERIOD_ID,
EXMM.TDL_MATERIAL_TYPE_ID as TDL_MEDICINE_TYPE_ID,
EXMM.TDL_AGGR_EXP_MEST_ID,
EXMM.EXP_MEST_MATY_REQ_ID as EXP_MEST_METY_REQ_ID,
EXMM.CK_IMP_MEST_MATERIAL_ID as CK_IMP_MEST_MEDICINE_ID,
EXMM.IS_EXPORT,
EXMM.AMOUNT,
EXMM.PRICE,
EXMM.VAT_RATIO,
EXMM.DISCOUNT,
EXMM.NUM_ORDER,
EXMM.DESCRIPTION,
EXMM.APPROVAL_TIME,
EXMM.APPROVAL_DATE,
EXMM.EXP_LOGINNAME,
EXMM.EXP_USERNAME,
EXMM.EXP_TIME,
EXMM.EXP_DATE,
EXMM.TH_AMOUNT,
EXMM.PATIENT_TYPE_ID,
EXMM.SERE_SERV_PARENT_ID,
EXMM.IS_EXPEND,
EXMM.IS_OUT_PARENT_FEE,
'' as USE_TIME_TO,
EXMM.TUTORIAL,
EXMM.TDL_SERVICE_REQ_ID,
EXMM.TDL_TREATMENT_ID,
EXMM.IS_USE_CLIENT_PRICE,
EXMM.VIR_PRICE,
'' as VACCINATION_RESULT_ID,
'' as TDL_VACCINATION_ID,
'' as SPEED,
EXMM.EXPEND_TYPE_ID,
EXMM.IS_NOT_PRES,
EXMM.USE_ORIGINAL_UNIT_FOR_PRES,
EXMM.BCS_REQ_AMOUNT,
'' as DAY_COUNT,
'' as MORNING,
'' as NOON,
'' as AFTERNOON,
'' as EVENING,
'' as HTU_ID,
'' as BREATH_SPEED,
'' as BREATH_TIME,
'' as PREVIOUS_USING_COUNT,
EXMM.IS_USED,
EXMM.SERVICE_CONDITION_ID,
EXMM.EXP_MEST_CODE,
EXMM.EXP_MEST_TYPE_ID,
EXMM.AGGR_EXP_MEST_ID,
EXMM.MEDI_STOCK_ID,
EXMM.EXP_MEST_STT_ID,
EXMM.REQ_ROOM_ID,
EXMM.REQ_DEPARTMENT_ID,
EXMM.TDL_INTRUCTION_TIME,
EXMM.TDL_PRES_REQ_USER_TITLE,
EXMM.IMP_PRICE,
EXMM.IMP_VAT_RATIO,
EXMM.BID_ID,
EXMM.PACKAGE_NUMBER,
EXMM.EXPIRED_DATE,
EXMM.MATERIAL_TYPE_ID as MEDICINE_TYPE_ID,
EXMM.INTERNAL_PRICE,
EXMM.IMP_TIME,
EXMM.SUPPLIER_ID,
'' as MEDICINE_TCY_NUM_ORDER,
'' as MEDICINE_BYT_NUM_ORDER,
'' as MEDICINE_REGISTER_NUMBER,
'' as ACTIVE_INGR_BHYT_CODE,
'' as ACTIVE_INGR_BHYT_NAME,
'' as CONCENTRA,
EXMM.MATERIAL_TYPE_CODE as MEDICINE_TYPE_CODE,
EXMM.MATERIAL_TYPE_NAME as MEDICINE_TYPE_NAME,
EXMM.SERVICE_ID,
EXMM.NATIONAL_NAME,
EXMM.MANUFACTURER_ID,
EXMM.MATERIAL_TYPE_NUM_ORDER as MEDICINE_TYPE_NUM_ORDER,
'' as TCY_NUM_ORDER,
'' as BYT_NUM_ORDER,
'' as REGISTER_NUMBER,
'' as IS_FUNCTIONAL_FOOD,
EXMM.MEMA_GROUP_ID,
'' as IS_ALLOW_ODD,
0 as MEDICINE_GROUP_ID,
EXMM.SERVICE_UNIT_ID,
EXMM.RECORDING_TRANSACTION,
0 as MEDICINE_USE_FORM_ID,
EXMM.IS_OUT_HOSPITAL,
EXMM.SERVICE_UNIT_CODE,
EXMM.SERVICE_UNIT_NAME,
EXMM.MEDICINE_NUM_ORDER,
EXMM.MATERIAL_NUM_ORDER,
EXMM.CONVERT_RATIO,
EXMM.MANUFACTURER_CODE,
EXMM.MANUFACTURER_NAME,
EXMM.SUPPLIER_CODE,
EXMM.SUPPLIER_NAME,
EXMM.BID_NUMBER,
EXMM.BID_NAME,
EXMM.PATIENT_TYPE_CODE,
EXMM.PATIENT_TYPE_NAME,
'' as MEDICINE_GROUP_CODE,
'' as MEDICINE_GROUP_NAME,
'' as MEDICINE_GROUP_NUM_ORDER,
'' as MEDICINE_USE_FORM_CODE,
'' as MEDICINE_USE_FORM_NAME,
'' as MEDICINE_USE_FORM_NUM_ORDER 
";

            query += "FROM V_HIS_EXP_MEST_MATERIAL EXMM \n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=EXMM.EXP_MEST_ID \n";
            query += "JOIN HIS_MATERIAL ME ON ME.ID=EXMM.MATERIAL_ID \n";
            query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID=NVL(EX.PRESCRIPTION_ID,EX.SERVICE_REQ_ID) \n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=Ex.TDL_TREATMENT_ID \n";
            query += "LEFT JOIN HIS_TREATMENT_TYPE TMT ON TMT.ID=TREA.TDL_TREATMENT_TYPE_ID \n";
            query += "LEFT JOIN HIS_PATIENT_TYPE PTT ON PTT.ID=TREA.TDL_PATIENT_TYPE_ID \n";
            query += "LEFT JOIN HIS_MEDI_STOCK MS ON MS.ID=EX.MEDI_STOCK_ID \n";
            query += "LEFT JOIN V_HIS_ROOM RR ON RR.ID=SR.REQUEST_ROOM_ID \n";
            query += "LEFT JOIN HIS_DEPARTMENT RD ON RD.ID=SR.REQUEST_DEPARTMENT_ID \n";
            query += "LEFT JOIN HIS_TREATMENT_END_TYPE TET ON TET.ID=TREA.TREATMENT_END_TYPE_ID \n";
            query += "LEFT JOIN HIS_EXP_MEST_TYPE EMT ON EMT.ID=EX.EXP_MEST_TYPE_ID \n";
            query += "LEFT JOIN HIS_IMP_MEST IM ON EX.ID=IM.MOBA_EXP_MEST_ID \n";
            query += "LEFT JOIN HIS_EXP_MEST AGG ON AGG.ID=EXMM.AGGR_EXP_MEST_ID \n";
            query += "LEFT JOIN LATERAL(SELECT SUM(AMOUNT) TH_AMOUNT FROM V_HIS_IMP_MEST_MATERIAL WHERE TH_EXP_MEST_MATERIAL_ID = EXMM.ID AND IS_DELETE =0 AND IMP_MEST_STT_ID=5) IMMM ON 1=1 \n";
            query += "LEFT JOIN HIS_SERE_SERV SS ON (SS.EXP_MEST_MEDICINE_ID=EXMM.ID AND SS.PATIENT_TYPE_ID=1) \n";//chỉ join với ss bảo hiểm để lấy tiền bảo hiểm trả

            if (filter.ADD_PAR_SERVICE == true)
            {
                query += @"left join lateral(
select 
Listagg(pr.tdl_service_name, '; ') Within Group (Order By 1) ss_service_name 
from his_service_req prsr 
join his_sere_serv pr on pr.service_req_id=prsr.id 
where 1=1
and prsr.id=sr.parent_id
) SVSS on 1=1
";
            }
            if (filter.ADD_REQ_BED == true)
            {
               query += @"left join lateral(
select 
min(bl.bed_code) keep(dense_rank first order by bl.start_time) bed_code,
min(bl.bed_name) keep(dense_rank first order by bl.start_time) bed_name
from v_his_bed_log bl  
join his_bed_room br on br.id=bl.bed_room_id
where 1=1
and br.room_id=SR.REQUEST_ROOM_ID
and bl.treatment_id = trea.id
and bl.start_time<sr.intruction_time
) bl on 1=1
";
            }
            query += "LEFT JOIN his_service SV on SV.ID = exmm.service_id  \n";
            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 \n";

            query += string.Format("AND (sr.id is not null or ex.exp_mest_type_id ={0})\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            if (filter.INPUT_DATA_ID_TIME_TYPE != null)
            {
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND NVL(SR.INTRUCTION_TIME,EX.TDL_INTRUCTION_TIME) BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND EX.bill_id in (select id from his_transaction where is_cancel is null and transaction_time BETWEEN {0} AND {1}) \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND ss.hein_approval_id in (select id from his_hein_approval where is_delete=0 and execute_time BETWEEN {0} AND {1}) \n", filter.TIME_FROM, filter.TIME_TO);
                }
            }
            else
            {
                if (filter.TRUE_FALSE.HasValue && !filter.TRUE_FALSE.Value)
                {
                    query += string.Format("AND NVL(SR.INTRUCTION_TIME,EX.TDL_INTRUCTION_TIME)  BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
            }

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID ={0} \n", filter.DEPARTMENT_ID);
            }
            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
            }
            if (filter.EMPLOYEE_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND exists (select 1 from his_employee emp where emp.DEPARTMENT_ID IN({0}) and (ex.tdl_prescription_req_loginname is not null and emp.loginname=ex.tdl_prescription_req_loginname or ex.tdl_prescription_req_loginname is null and emp.loginname=sr.request_loginname)) \n", string.Join(",", filter.EMPLOYEE_DEPARTMENT_IDs));
            }
            if (filter.EXAM_ROOM_ID != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID ={0} \n", filter.EXAM_ROOM_ID);
            }
            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID IN({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
            }
            if (filter.TREATMENT_TYPE_ID != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID ={0} \n", filter.TREATMENT_TYPE_ID);
            }
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            if (filter.PATIENT_TYPE_ID != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
            }
            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }
            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.MEDICINE_TYPE_ID != null)
            {
                query += string.Format("AND 1=0 \n", filter.MEDICINE_TYPE_ID);
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("AND 1=0 \n", string.Join(",", filter.MEDICINE_TYPE_IDs));
            }
            if (filter.MEDICINE_GROUP_IDs != null)
            {
                query += string.Format("AND 1=0 \n", string.Join(",", filter.MEDICINE_GROUP_IDs));
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.EXP_MEST_TYPE_ID != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID ={0} \n", filter.EXP_MEST_TYPE_ID);
            }
            if (filter.EXP_MEST_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
            }
            if (filter.ADD_SALE != true)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID <>{0} \n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            }
            if (filter.REPORT_TYPE_CAT_ID != null)
            {
                query += string.Format("AND EXISTS (SELECT 1 FROM HIS_SERVICE_RETY_CAT WHERE SERVICE_ID = EXMM.SERVICE_ID AND REPORT_TYPE_CAT_ID = {0})\n", filter.REPORT_TYPE_CAT_ID);
            }
            if (filter.REPORT_TYPE_CAT_IDs != null)
            {
                query += string.Format("AND EXISTS (SELECT 1 FROM HIS_SERVICE_RETY_CAT WHERE SERVICE_ID = EXMM.SERVICE_ID AND REPORT_TYPE_CAT_ID IN({0})) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
            }

            if (filter.ICD_CODEs != null)
            {
                query += string.Format("AND nvl(ex.icd_code,nvl(sr.icd_code,TREA.ICD_CODE)) IN('{0}') \n", string.Join("','", filter.ICD_CODEs));
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                query += string.Format("AND EXISTS (SELECT 1 FROM his_material_type mt1 join his_material_type pr1 on pr1.id=mt1.parent_id WHERE mt1.id = EXMM.material_type_id AND pr1.service_id IN({0})) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
            }
           
            if (filter.DOCTOR_LOGINNAMEs != null)
            {
                query += string.Format("AND nvl(SR.REQUEST_LOGINNAME,ex.tdl_prescription_req_loginname) IN('{0}') \n", string.Join("','", filter.DOCTOR_LOGINNAMEs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00207RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00207");

            return result;
        }  

    }
}
