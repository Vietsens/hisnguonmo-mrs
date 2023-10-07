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
using MOS.DAO.Sql;
using System.Data;

namespace MRS.Processor.Mrs00208
{
    public partial class ManagerSql : BusinessBase
    {

        public List<Mrs00208RDO> Get(Mrs00208Filter filter)
        {
            List<Mrs00208RDO> result = new List<Mrs00208RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT \n";
            query += "(EXMM.AMOUNT - NVL(IMMM.TH_AMOUNT,0)) AMOUNT_TRUST, \n";
            query += "NVL(EXMM.PRICE,0)*(1+NVL(EXMM.VAT_RATIO,0))*(EXMM.AMOUNT - NVL(IMMM.TH_AMOUNT,0)) TOTAL_PRICE, \n";
            query += "SR.TDL_PATIENT_ID PATIENT_ID, \n";
            query += "SR.TDL_PATIENT_NAME VIR_PATIENT_NAME, \n";
            query += "SR.TDL_PATIENT_CODE, \n";
            query += "SR.TDL_TREATMENT_CODE, \n";
            query += "SR.SERVICE_REQ_CODE, \n";
            query += "SR.INTRUCTION_TIME, \n";
            query += "SR.INTRUCTION_DATE, \n";
            query += "SR.REQUEST_LOGINNAME, \n";
            query += "SR.REQUEST_USERNAME, \n";
            //query += "EMT.EXP_MEST_TYPE_CODE, \n";
            //query += "EMT.EXP_MEST_TYPE_NAME, \n";
            query += "TREA.OUT_TIME, \n";
            query += "TREA.TDL_PATIENT_ADDRESS, \n";
            query += "nvl(TREA.TDL_TREATMENT_TYPE_ID,0) TREATMENT_TYPE_ID, \n";
            query += "nvl(TREA.TDL_PATIENT_TYPE_ID,0) TDL_PATIENT_TYPE_ID, \n";
            query += "nvl(TREA.TREATMENT_END_TYPE_ID,0) TREATMENT_END_TYPE_ID, \n";
            //query += "TMT.TREATMENT_TYPE_CODE, \n";
            //query += "TMT.TREATMENT_TYPE_NAME, \n";
            //query += "PTT.PATIENT_TYPE_CODE TDL_PATIENT_TYPE_CODE, \n";
            //query += "PTT.PATIENT_TYPE_NAME TDL_PATIENT_TYPE_NAME, \n";
            //query += "RR.DEPARTMENT_CODE REQUEST_DEPARTMENT_CODE, \n";
            //query += "RR.DEPARTMENT_NAME REQUEST_DEPARTMENT_NAME, \n";
            //query += "RR.ROOM_CODE REQUEST_ROOM_CODE, \n";
            //query += "RR.ROOM_NAME REQUEST_ROOM_NAME, \n";
            //query += "MS.MEDI_STOCK_CODE, \n";
            //query += "MS.MEDI_STOCK_NAME, \n";
            //query += "TET.TREATMENT_END_TYPE_CODE, \n";
            //query += "TET.TREATMENT_END_TYPE_NAME, \n";
            query += "SRC.CATEGORY_CODE, \n";
            query += "SRC.CATEGORY_NAME, \n";
            query += "EXMM.*  \n";

            query += "FROM V_HIS_EXP_MEST_MATERIAL EXMM \n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=EXMM.EXP_MEST_ID \n";
            query += "JOIN HIS_SERVICE_REQ SR ON SR.ID=NVL(EX.PRESCRIPTION_ID,EX.SERVICE_REQ_ID) \n";
            query += "JOIN lateral(select 1 from HIS_TREATMENT TREA where TREA.ID=SR.TREATMENT_ID ) trea on TREA.ID=SR.TREATMENT_ID \n";
            //query += "LEFT JOIN HIS_TREATMENT_TYPE TMT ON TMT.ID=TREA.TDL_TREATMENT_TYPE_ID \n";
            //query += "LEFT JOIN HIS_PATIENT_TYPE PTT ON PTT.ID=TREA.TDL_PATIENT_TYPE_ID \n";
            //query += "LEFT JOIN HIS_MEDI_STOCK MS ON MS.ID=EX.MEDI_STOCK_ID \n";
            //query += "LEFT JOIN V_HIS_ROOM RR ON RR.ID=SR.REQUEST_ROOM_ID \n";
            //query += "LEFT JOIN HIS_TREATMENT_END_TYPE TET ON TET.ID=TREA.TREATMENT_END_TYPE_ID \n";
            //query += "LEFT JOIN HIS_EXP_MEST_TYPE EMT ON EMT.ID=EX.EXP_MEST_TYPE_ID \n";
            query += "LEFT JOIN HIS_IMP_MEST IM ON EX.ID=IM.MOBA_EXP_MEST_ID \n";
            query += "LEFT JOIN LATERAL (SELECT SUM(AMOUNT) TH_AMOUNT FROM V_HIS_IMP_MEST_MATERIAL WHERE TH_EXP_MEST_MATERIAL_ID =EXMM.ID AND IS_DELETE =0 AND IMP_MEST_STT_ID=5) IMMM ON 1=1 \n";
            query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=EXMM.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00208') \n";

            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 \n";
            
            if (filter.TRUE_FALSE.HasValue && !filter.TRUE_FALSE.Value)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else
            {
                query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID ={0} \n", filter.DEPARTMENT_ID);
            }
            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
            }
            if (filter.EXAM_ROOM_ID != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID ={0} \n", filter.EXAM_ROOM_ID);
            }
            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID IN({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
            }
            if (filter.MATERIAL_TYPE_ID != null)
            {
                query += string.Format("AND EXMM.MATERIAL_TYPE_ID ={0} \n", filter.MATERIAL_TYPE_ID);
            }
            if (filter.MATERIAL_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.MATERIAL_TYPE_ID IN({0}) \n", string.Join(",", filter.MATERIAL_TYPE_IDs));
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
                query += string.Format("AND SRC.REPORT_TYPE_CAT_ID ={0} \n", filter.REPORT_TYPE_CAT_ID);
            }
            if (filter.REPORT_TYPE_CAT_IDs != null)
            {
                query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN({0}) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00208RDO>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00208");

            return result;
        }

      
       
    }
}
