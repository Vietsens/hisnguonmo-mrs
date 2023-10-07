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

namespace MRS.Processor.Mrs00312
{
    public partial class ManagerSql : BusinessBase
    {
        public List<DataGet> GetExpMestMe(Mrs00312Filter filter)
        {
            List<DataGet> result = new List<DataGet>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "1 TYPE,\n";
            query += "EXMM.AMOUNT,\n";
            query += "EXMM.BID_ID,\n";
            query += "EXMM.EXP_MEST_ID,\n";
            query += "EXMM.EXP_MEST_TYPE_ID,\n";
            query += "EXMM.EXP_TIME,\n";
            query += "EXMM.ID,\n";
            query += "EXMM.IMP_PRICE,\n";
            query += "EXMM.IMP_VAT_RATIO,\n";
            query += "EXMM.MEDI_STOCK_ID,\n";
            query += "EXMM.MEDICINE_ID MEMA_ID,\n";
            query += "EXMM.MEDICINE_TYPE_ID MEMA_TYPE_ID,\n";
            query += "EXMM.PACKAGE_NUMBER,\n";
            query += "NVL(IMPS.DEPARTMENT_ID,EXMM.REQ_DEPARTMENT_ID) REQ_DEPARTMENT_ID,\n";
            query += "EXMM.REQ_ROOM_ID,\n";
            query += "EXMM.SERVICE_UNIT_ID,\n";
            query += "EXMM.SUPPLIER_ID,\n";
            query += "EX.imp_medi_stock_id chms_medi_stock_id,\n";
            query += "EX.EXP_MEST_REASON_ID,\n";
            query += "EXMM.EXPIRED_DATE,\n";
            query += "EXMM.MEDICINE_GROUP_ID,\n";
            query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
            if (filter.TRUE_FALSE == true)
            {
                query += "IMMM.TH_AMOUNT,\n";
                query += "IMMM.MOBA_AMOUNT,\n";
            }
            else
            {
                query += "0 TH_AMOUNT,\n";
                query += "0 MOBA_AMOUNT,\n";
            }
            query += "nvl(EXMM.PRICE,0) price,\n";
            query += "nvl(EXMM.VAT_RATIO,0) vat_ratio\n";
            query += "FROM V_HIS_EXP_MEST_MEDICINE EXMM\n";
            query += "join HIS_MEDICINE M on m.id=EXMM.medicine_id\n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=EXMM.EXP_MEST_ID\n";
            query += "LEFT JOIN V_HIS_MEDI_STOCK IMPS ON IMPS.ID=EX.IMP_MEDI_STOCK_ID\n";
            query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID=NVL(EX.PRESCRIPTION_ID,EX.SERVICE_REQ_ID)\n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=EXMM.TDL_TREATMENT_ID\n";
            query += "LEFT JOIN HIS_BED_ROOM BR ON BR.ROOM_ID=SR.REQUEST_ROOM_ID\n";
            query += "LEFT JOIN HIS_BED_ROOM BR1 ON BR1.ROOM_ID=EX.REQ_ROOM_ID\n";
            if (filter.TRUE_FALSE == true)
            {
                query += string.Format("LEFT JOIN LATERAL (SELECT SUM(case when imp_mest_stt_id ={4} and imp_time between {0} and {1} then 0 else amount end) TH_AMOUNT,SUM(case when imp_mest_stt_id ={4} and imp_time between {0} and {1} then amount else 0 end) MOBA_AMOUNT FROM V_HIS_IMP_MEST_MEDICINE IMMM WHERE TH_EXP_MEST_MEDICINE_ID IS NOT NULL AND IS_DELETE =0 AND IMP_MEST_STT_ID in({2},{3},{4}) AND EXMM.ID=IMMM.TH_EXP_MEST_MEDICINE_ID) IMMM  on 1=1 \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__REQUEST, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__APPROVAL, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT);
            }
            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0\n";
            if (filter.TIME_TRUE_FALSE.HasValue && !filter.TIME_TRUE_FALSE.Value)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.REQ_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID IN({0}) \n", string.Join(",", filter.REQ_ROOM_IDs));
                }
                if (filter.EXACT_BED_ROOM_IDs != null)
                {
                    query += string.Format("AND BR.ID IN({0}) \n", string.Join(",", filter.EXACT_BED_ROOM_IDs));
                }
            }
            else
            {
                query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND EX.REQ_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.REQ_ROOM_IDs != null)
                {
                    query += string.Format("AND EX.REQ_ROOM_ID IN({0}) \n", string.Join(",", filter.REQ_ROOM_IDs));
                }
                if (filter.EXACT_BED_ROOM_IDs != null)
                {
                    query += string.Format("AND BR1.ID IN({0}) \n", string.Join(",", filter.EXACT_BED_ROOM_IDs));
                }
            }
            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND exists (select 1 from v_his_medi_stock ms join his_department dp on ms.department_id=dp.id where ms.id=exmm.medi_stock_id and dp.branch_id in ({0})) \n", string.Join(",", filter.BRANCH_IDs));
            }
            if (filter.IMP_SOURCE_IDs != null)
            {
                //query += string.Format("AND M.IMP_SOURCE_ID IN({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.MEDICINE_TYPE_ID IN ({0}) \n", string.Join(",", filter.MEDICINE_TYPE_IDs));
            }
            if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID NOT IN({0}) \n", string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
            }
            if (filter.IS_NOT_LOCAL_EXP == true)
            {
                string strAdd = "AND (EXMM.EXP_MEST_TYPE_ID NOT IN({0},{1},{2}) OR EX.IMP_MEDI_STOCK_ID IN ({3})) \n";
                if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
                }
                else
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, "''");
                }
            }

            if (filter.EXP_MEST_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
            }

            if (filter.EXP_MEST_REASON_IDs != null)
            {
                query += string.Format("AND ex.EXP_MEST_REASON_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_REASON_IDs));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<DataGet>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00312");

            return result;
        }

        public List<DataGet> GetExpMestMa(Mrs00312Filter filter)
        {
            List<DataGet> result = new List<DataGet>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "2 TYPE,\n";
            query += "EXMM.AMOUNT,\n";
            query += "EXMM.BID_ID,\n";
            query += "EXMM.EXP_MEST_ID,\n";
            query += "EXMM.EXP_MEST_TYPE_ID,\n";
            query += "EXMM.EXP_TIME,\n";
            query += "EXMM.ID,\n";
            query += "EXMM.IMP_PRICE,\n";
            query += "EXMM.IMP_VAT_RATIO,\n";
            query += "EXMM.MEDI_STOCK_ID,\n";
            query += "EXMM.MATERIAL_ID MEMA_ID,\n";
            query += "EXMM.MATERIAL_TYPE_ID MEMA_TYPE_ID,\n";
            query += "EXMM.PACKAGE_NUMBER,\n";
            query += "NVL(IMPS.DEPARTMENT_ID,EXMM.REQ_DEPARTMENT_ID) REQ_DEPARTMENT_ID,\n";
            query += "EXMM.REQ_ROOM_ID,\n";
            query += "EXMM.SERVICE_UNIT_ID,\n";
            query += "EXMM.SUPPLIER_ID,\n";
            query += "EX.imp_medi_stock_id chms_medi_stock_id,\n";
            query += "EX.EXP_MEST_REASON_ID,\n";
            query += "null MEDICINE_GROUP_ID,\n";
            query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
            query += "EXMM.EXPIRED_DATE,\n";
            if (filter.TRUE_FALSE == true)
            {
                query += "IMMM.TH_AMOUNT,\n";
                query += "IMMM.MOBA_AMOUNT,\n";
            }
            else
            {
                query += "0 TH_AMOUNT,\n";
                query += "0 MOBA_AMOUNT,\n";
            }
            query += "nvl(EXMM.PRICE,0) price,\n";
            query += "nvl(EXMM.VAT_RATIO,0) vat_ratio\n";
            query += "FROM V_HIS_EXP_MEST_MATERIAL EXMM\n";
            query += "join HIS_MATERIAL M on m.id=EXMM.material_id\n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=EXMM.EXP_MEST_ID\n";
            query += "LEFT JOIN V_HIS_MEDI_STOCK IMPS ON IMPS.ID=EX.IMP_MEDI_STOCK_ID\n";
            query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID=NVL(EX.PRESCRIPTION_ID,EX.SERVICE_REQ_ID)\n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=EXMM.TDL_TREATMENT_ID\n";
            query += "LEFT JOIN HIS_BED_ROOM BR ON BR.ROOM_ID=SR.REQUEST_ROOM_ID\n";
            query += "LEFT JOIN HIS_BED_ROOM BR1 ON BR1.ROOM_ID=EX.REQ_ROOM_ID\n";
            if (filter.TRUE_FALSE == true)
            {
                query += string.Format("LEFT JOIN LATERAL (SELECT SUM(case when imp_mest_stt_id ={4} and imp_time between {0} and {1} then 0 else amount end) TH_AMOUNT,SUM(case when imp_mest_stt_id ={4} and imp_time between {0} and {1} then amount else 0 end) MOBA_AMOUNT FROM V_HIS_IMP_MEST_MATERIAL IMMM WHERE TH_EXP_MEST_MATERIAL_ID IS NOT NULL AND IS_DELETE =0 AND IMP_MEST_STT_ID in({2},{3},{4}) AND EXMM.ID=IMMM.TH_EXP_MEST_MATERIAL_ID ) IMMM on 1=1\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__REQUEST, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__APPROVAL, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT);
            }
            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0\n";
            if (filter.TIME_TRUE_FALSE.HasValue && !filter.TIME_TRUE_FALSE.Value)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.REQ_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID IN({0}) \n", string.Join(",", filter.REQ_ROOM_IDs));
                }
                if (filter.EXACT_BED_ROOM_IDs != null)
                {
                    query += string.Format("AND BR.ID IN({0}) \n", string.Join(",", filter.EXACT_BED_ROOM_IDs));
                }
            }
            else
            {
                query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND EX.REQ_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.REQ_ROOM_IDs != null)
                {
                    query += string.Format("AND EX.REQ_ROOM_ID IN({0}) \n", string.Join(",", filter.REQ_ROOM_IDs));
                }
                if (filter.EXACT_BED_ROOM_IDs != null)
                {
                    query += string.Format("AND BR1.ID IN({0}) \n", string.Join(",", filter.EXACT_BED_ROOM_IDs));
                }
            }
            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.IMP_SOURCE_IDs != null)
            {
                //query += string.Format("AND M.IMP_SOURCE_ID IN({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND exists (select 1 from v_his_medi_stock ms join his_department dp on ms.department_id=dp.id where ms.id=exmm.medi_stock_id and dp.branch_id in ({0})) \n", string.Join(",", filter.BRANCH_IDs));
            }
            if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID NOT IN({0}) \n", string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
            }
            if (filter.IS_NOT_LOCAL_EXP == true)
            {
                string strAdd = "AND (EXMM.EXP_MEST_TYPE_ID NOT IN({0},{1},{2}) OR EX.IMP_MEDI_STOCK_ID IN ({3})) \n";
                if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
                }
                else
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, "''");
                }
            }

            if (filter.EXP_MEST_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
            }

            if (filter.EXP_MEST_REASON_IDs != null)
            {
                query += string.Format("AND ex.EXP_MEST_REASON_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_REASON_IDs));
            }
            query += string.Format("AND EXMM.IS_CHEMICAL_SUBSTANCE is null ");

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<DataGet>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00312");

            return result;
        }
        public List<DataGet> GetExpMestChem(Mrs00312Filter filter)
        {
            List<DataGet> result = new List<DataGet>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "3 TYPE,\n";
            query += "EXMM.AMOUNT,\n";
            query += "EXMM.BID_ID,\n";
            query += "EXMM.EXP_MEST_ID,\n";
            query += "EXMM.EXP_MEST_TYPE_ID,\n";
            query += "EXMM.EXP_TIME,\n";
            query += "EXMM.ID,\n";
            query += "EXMM.IMP_PRICE,\n";
            query += "EXMM.IMP_VAT_RATIO,\n";
            query += "EXMM.MEDI_STOCK_ID,\n";
            query += "EXMM.MATERIAL_ID MEMA_ID,\n";
            query += "EXMM.MATERIAL_TYPE_ID MEMA_TYPE_ID,\n";
            query += "EXMM.PACKAGE_NUMBER,\n";
            query += "NVL(IMPS.DEPARTMENT_ID,EXMM.REQ_DEPARTMENT_ID) REQ_DEPARTMENT_ID,\n";
            query += "EXMM.REQ_ROOM_ID,\n";
            query += "EXMM.SERVICE_UNIT_ID,\n";
            query += "EXMM.SUPPLIER_ID,\n";
            query += "EX.imp_medi_stock_id chms_medi_stock_id,\n";
            query += "EX.EXP_MEST_REASON_ID,\n";
            query += "null MEDICINE_GROUP_ID,\n";
            query += "EXMM.EXPIRED_DATE,\n";
            query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
            if (filter.TRUE_FALSE == true)
            {
                query += "IMMM.TH_AMOUNT,\n";
                query += "IMMM.MOBA_AMOUNT,\n";
            }
            else
            {
                query += "0 TH_AMOUNT,\n";
                query += "0 MOBA_AMOUNT,\n";
            }
            query += "nvl(EXMM.PRICE,0) price,\n";
            query += "nvl(EXMM.VAT_RATIO,0) vat_ratio\n";
            query += "FROM V_HIS_EXP_MEST_MATERIAL EXMM\n";
            query += "join HIS_MATERIAL M on m.id=EXMM.material_id\n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=EXMM.EXP_MEST_ID\n";
            query += "LEFT JOIN V_HIS_MEDI_STOCK IMPS ON IMPS.ID=EX.IMP_MEDI_STOCK_ID\n";
            query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID=NVL(EX.PRESCRIPTION_ID,EX.SERVICE_REQ_ID)\n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=EXMM.TDL_TREATMENT_ID\n";
            query += "LEFT JOIN HIS_BED_ROOM BR ON BR.ROOM_ID=SR.REQUEST_ROOM_ID\n";
            query += "LEFT JOIN HIS_BED_ROOM BR1 ON BR1.ROOM_ID=EX.REQ_ROOM_ID\n";
            if (filter.TRUE_FALSE == true)
            {
                query += string.Format("LEFT JOIN LATERAL (SELECT SUM(case when imp_mest_stt_id ={4} and imp_time between {0} and {1} then 0 else amount end) TH_AMOUNT,SUM(case when imp_mest_stt_id ={4} and imp_time between {0} and {1} then amount else 0 end) MOBA_AMOUNT FROM V_HIS_IMP_MEST_MATERIAL IMMM WHERE TH_EXP_MEST_MATERIAL_ID IS NOT NULL AND IS_DELETE =0 AND IMP_MEST_STT_ID in({2},{3},{4}) AND EXMM.ID=IMMM.TH_EXP_MEST_MATERIAL_ID) IMMM on 1=1\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__REQUEST, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__APPROVAL, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT);
            }
            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0\n";
            if (filter.TIME_TRUE_FALSE.HasValue && !filter.TIME_TRUE_FALSE.Value)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.REQ_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID IN({0}) \n", string.Join(",", filter.REQ_ROOM_IDs));
                }
                if (filter.EXACT_BED_ROOM_IDs != null)
                {
                    query += string.Format("AND BR.ID IN({0}) \n", string.Join(",", filter.EXACT_BED_ROOM_IDs));
                }
            }
            else
            {
                query += string.Format("AND EXMM.EXP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND EX.REQ_DEPARTMENT_ID IN({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.REQ_ROOM_IDs != null)
                {
                    query += string.Format("AND EX.REQ_ROOM_ID IN({0}) \n", string.Join(",", filter.REQ_ROOM_IDs));
                }
                if (filter.EXACT_BED_ROOM_IDs != null)
                {
                    query += string.Format("AND BR1.ID IN({0}) \n", string.Join(",", filter.EXACT_BED_ROOM_IDs));
                }
            }
            if (filter.IMP_SOURCE_IDs != null)
            {
                //query += string.Format("AND M.IMP_SOURCE_ID IN({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
            }
            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND exists (select 1 from v_his_medi_stock ms join his_department dp on ms.department_id=dp.id where ms.id=exmm.medi_stock_id and dp.branch_id in ({0})) \n", string.Join(",", filter.BRANCH_IDs));
            }
            if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.MEDI_STOCK_ID NOT IN({0}) \n", string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
            }
            if (filter.IS_NOT_LOCAL_EXP == true)
            {
                string strAdd = "AND (EXMM.EXP_MEST_TYPE_ID NOT IN({0},{1},{2}) OR EX.IMP_MEDI_STOCK_ID IN ({3})) \n";
                if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
                }
                else
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, "''");
                }
            }

            if (filter.EXP_MEST_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_TYPE_IDs));
            }

            if (filter.EXP_MEST_REASON_IDs != null)
            {
                query += string.Format("AND ex.EXP_MEST_REASON_ID IN({0}) \n", string.Join(",", filter.EXP_MEST_REASON_IDs));
            }
            query += string.Format("AND EXMM.IS_CHEMICAL_SUBSTANCE ={0} ", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<DataGet>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00312");

            return result;
        }

        public List<DataGet> GetMobaMe(Mrs00312Filter filter)
        {
            List<DataGet> result = new List<DataGet>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "1 TYPE,\n";
            query += "0 AMOUNT,\n";
            query += "IMMM.BID_ID,\n";
            query += "IMMM.IMP_MEST_ID,\n";
            query += "IMMM.IMP_MEST_TYPE_ID,\n";
            query += "IMMM.IMP_TIME EXP_TIME,\n";
            query += "IMMM.ID,\n";
            query += "IMMM.IMP_PRICE,\n";
            query += "IMMM.IMP_VAT_RATIO,\n";
            query += "IMMM.MEDI_STOCK_ID,\n";
            query += "IMMM.MEDICINE_ID MEMA_ID,\n";
            query += "IMMM.MEDICINE_TYPE_ID MEMA_TYPE_ID,\n";
            query += "IMMM.PACKAGE_NUMBER,\n";
            query += "NVL(CHMS.DEPARTMENT_ID,IMMM.REQ_DEPARTMENT_ID) REQ_DEPARTMENT_ID,\n";
            query += "IMMM.REQ_ROOM_ID,\n";
            query += "IMMM.SERVICE_UNIT_ID,\n";
            query += "IMMM.SUPPLIER_ID,\n";
            query += "IM.CHMS_MEDI_STOCK_ID,\n";
            query += "IMMM.EXPIRED_DATE,\n";
            query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
            query += "IMMM.MEDICINE_GROUP_ID,\n";
            query += "0 TH_AMOUNT,\n";
            query += "IMMM.AMOUNT MOBA_AMOUNT,\n";
            query += "nvl(IMMM.PRICE,0) price,\n";
            query += "nvl(IMMM.VAT_RATIO,0) vat_ratio\n";
            query += "FROM V_HIS_IMP_MEST_MEDICINE IMMM\n";
            query += "JOIN HIS_MEDICINE M on m.id=immm.MEDICINE_id\n";
            query += "JOIN HIS_IMP_MEST IM ON IM.ID=IMMM.IMP_MEST_ID\n";
            query += "LEFT JOIN V_HIS_MEDI_STOCK CHMS ON CHMS.ID=IM.CHMS_MEDI_STOCK_ID\n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=im.TDL_TREATMENT_ID\n";

            query += "WHERE IMMM.IMP_MEST_STT_ID = 5 AND IMMM.IS_DELETE =0\n";
            query += string.Format("AND IMMM.IMP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);

            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.IMP_SOURCE_IDs != null)
            {
                //query += string.Format("AND M.IMP_SOURCE_ID IN({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("AND IMMM.MEDICINE_TYPE_ID IN ({0}) \n", string.Join(",", filter.MEDICINE_TYPE_IDs));
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND exists (select 1 from v_his_medi_stock ms join his_department dp on ms.department_id=dp.id where ms.id=immm.medi_stock_id and dp.branch_id in ({0})) \n", string.Join(",", filter.BRANCH_IDs));
            }
            if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID NOT IN({0}) \n", string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
            }
            if (filter.IS_NOT_LOCAL_EXP == true)
            {
                string strAdd = "AND (IMMM.IMP_MEST_TYPE_ID NOT IN({0},{1},{2}) OR IM.CHMS_MEDI_STOCK_ID IN ({3})) \n";
                if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL, string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
                }
                else
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL, "''");
                }
            }

            List<string> impMestTypeFilter = new List<string>();
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK))
            {
                impMestTypeFilter.Add(string.Format("  IMMM.IMP_MEST_TYPE_ID = {0} AND IM.CHMS_TYPE_ID = {1} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL));
            }

            if (filter.EXP_MEST_REASON_IDs != null)
            {
                query += string.Format("AND 1=0 \n", string.Join(",", filter.EXP_MEST_REASON_IDs));
            }

            if (impMestTypeFilter.Count > 0)
            {
                query += string.Format("AND ({0}) \n", string.Join("or ", impMestTypeFilter));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<DataGet>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00312");

            return result;
        }

        public List<DataGet> GetMobaMa(Mrs00312Filter filter)
        {
            List<DataGet> result = new List<DataGet>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "2 TYPE,\n";
            query += "0 AMOUNT,\n";
            query += "IMMM.BID_ID,\n";
            query += "IMMM.IMP_MEST_ID,\n";
            query += "IMMM.IMP_MEST_TYPE_ID,\n";
            query += "IMMM.IMP_TIME EXP_TIME,\n";
            query += "IMMM.ID,\n";
            query += "IMMM.IMP_PRICE,\n";
            query += "IMMM.IMP_VAT_RATIO,\n";
            query += "IMMM.MEDI_STOCK_ID,\n";
            query += "IMMM.MATERIAL_ID MEMA_ID,\n";
            query += "IMMM.MATERIAL_TYPE_ID MEMA_TYPE_ID,\n";
            query += "IMMM.PACKAGE_NUMBER,\n";
            query += "NVL(CHMS.DEPARTMENT_ID,IMMM.REQ_DEPARTMENT_ID) REQ_DEPARTMENT_ID,\n";
            query += "IMMM.REQ_ROOM_ID,\n";
            query += "IMMM.SERVICE_UNIT_ID,\n";
            query += "IMMM.SUPPLIER_ID,\n";
            query += "IM.CHMS_MEDI_STOCK_ID,\n";
            query += "IMMM.EXPIRED_DATE,\n";
            query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
            query += "null MEDICINE_GROUP_ID,\n";
            query += "0 TH_AMOUNT,\n";
            query += "IMMM.AMOUNT MOBA_AMOUNT,\n";
            query += "nvl(IMMM.PRICE,0) price,\n";
            query += "nvl(IMMM.VAT_RATIO,0) vat_ratio\n";
            query += "FROM V_HIS_IMP_MEST_MATERIAL IMMM\n";
            query += "join HIS_MATERIAL M on m.id=immm.material_id\n";
            query += "JOIN HIS_IMP_MEST IM ON IM.ID=IMMM.IMP_MEST_ID\n";
            query += "LEFT JOIN V_HIS_MEDI_STOCK CHMS ON CHMS.ID=IM.CHMS_MEDI_STOCK_ID\n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=im.TDL_TREATMENT_ID\n";
            query += "JOIN HIS_MATERIAL_TYPE Mety ON mety.ID=IMMM.MATERIAL_TYPE_ID\n";

            query += "WHERE IMMM.IMP_MEST_STT_ID = 5 AND IMMM.IS_DELETE =0\n";
            query += string.Format("AND IMMM.IMP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);

            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.IMP_SOURCE_IDs != null)
            {
                //query += string.Format("AND M.IMP_SOURCE_ID IN({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND exists (select 1 from v_his_medi_stock ms join his_department dp on ms.department_id=dp.id where ms.id=immm.medi_stock_id and dp.branch_id in ({0})) \n", string.Join(",", filter.BRANCH_IDs));
            }
            if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID NOT IN({0}) \n", string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
            }
            if (filter.IS_NOT_LOCAL_EXP == true)
            {
                string strAdd = "AND (IMMM.IMP_MEST_TYPE_ID NOT IN({0},{1},{2}) OR IM.CHMS_MEDI_STOCK_ID IN ({3})) \n";
                if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL, string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
                }
                else
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL, "''");
                }
            }

            List<string> impMestTypeFilter = new List<string>();
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK))
            {
                impMestTypeFilter.Add(string.Format("  IMMM.IMP_MEST_TYPE_ID = {0} AND IM.CHMS_TYPE_ID = {1} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL));
            }
            if (impMestTypeFilter.Count > 0)
            {
                query += string.Format("AND ({0}) \n", string.Join("or ", impMestTypeFilter));
            }
            query += string.Format("AND mety.IS_CHEMICAL_SUBSTANCE is null ");

            if (filter.EXP_MEST_REASON_IDs != null)
            {
                query += string.Format("AND 1=0 \n", string.Join(",", filter.EXP_MEST_REASON_IDs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<DataGet>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00312");

            return result;
        }

        public List<DataGet> GetMobaChem(Mrs00312Filter filter)
        {
            List<DataGet> result = new List<DataGet>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "3 TYPE,\n";
            query += "0 AMOUNT,\n";
            query += "IMMM.BID_ID,\n";
            query += "IMMM.IMP_MEST_ID,\n";
            query += "IMMM.IMP_MEST_TYPE_ID,\n";
            query += "IMMM.IMP_TIME EXP_TIME,\n";
            query += "IMMM.ID,\n";
            query += "IMMM.IMP_PRICE,\n";
            query += "IMMM.IMP_VAT_RATIO,\n";
            query += "IMMM.MEDI_STOCK_ID,\n";
            query += "IMMM.MATERIAL_ID MEMA_ID,\n";
            query += "IMMM.MATERIAL_TYPE_ID MEMA_TYPE_ID,\n";
            query += "IMMM.PACKAGE_NUMBER,\n";
            query += "NVL(CHMS.DEPARTMENT_ID,IMMM.REQ_DEPARTMENT_ID) REQ_DEPARTMENT_ID,\n";
            query += "IMMM.REQ_ROOM_ID,\n";
            query += "IMMM.SERVICE_UNIT_ID,\n";
            query += "IMMM.SUPPLIER_ID,\n";
            query += "IM.CHMS_MEDI_STOCK_ID,\n";
            query += "IMMM.EXPIRED_DATE,\n";
            query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
            query += "null MEDICINE_GROUP_ID,\n";
            query += "0 TH_AMOUNT,\n";
            query += "IMMM.AMOUNT MOBA_AMOUNT,\n";
            query += "nvl(IMMM.PRICE,0) price,\n";
            query += "nvl(IMMM.VAT_RATIO,0) vat_ratio\n";
            query += "FROM V_HIS_IMP_MEST_MATERIAL IMMM\n";
            query += "join HIS_MATERIAL M on m.id=immm.material_id\n";
            query += "JOIN HIS_MATERIAL_TYPE Mety ON mety.ID=IMMM.MATERIAL_TYPE_ID\n";
            query += "JOIN HIS_IMP_MEST IM ON IM.ID=IMMM.IMP_MEST_ID\n";
            query += "LEFT JOIN V_HIS_MEDI_STOCK CHMS ON CHMS.ID=IM.CHMS_MEDI_STOCK_ID\n";
            query += "LEFT JOIN HIS_TREATMENT TREA ON TREA.ID=im.TDL_TREATMENT_ID\n";

            query += "WHERE IMMM.IMP_MEST_STT_ID = 5 AND IMMM.IS_DELETE =0 and immm.req_department_id is not null and immm.req_room_id is not null\n";
            query += string.Format("AND IMMM.IMP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);

            if (filter.MEDI_STOCK_ID != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID IN({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }
            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND exists (select 1 from v_his_medi_stock ms join his_department dp on ms.department_id=dp.id where ms.id=immm.medi_stock_id and dp.branch_id in ({0})) \n", string.Join(",", filter.BRANCH_IDs));
            }
            if (filter.IMP_SOURCE_IDs != null)
            {
                //query += string.Format("AND M.IMP_SOURCE_ID IN({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
            }
            if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND IMMM.MEDI_STOCK_ID NOT IN({0}) \n", string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
            }
            if (filter.IS_NOT_LOCAL_EXP == true)
            {
                string strAdd = "AND (IMMM.IMP_MEST_TYPE_ID NOT IN({0},{1},{2}) OR IM.CHMS_MEDI_STOCK_ID IN ({3})) \n";
                if (filter.OUTSIDE_MEDI_STOCK_IDs != null)
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL, string.Join(",", filter.OUTSIDE_MEDI_STOCK_IDs));
                }
                else
                {
                    query += string.Format(strAdd, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL, "''");
                }
            }
            List<string> impMestTypeFilter = new List<string>();
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK))
            {
                impMestTypeFilter.Add(string.Format("  IMMM.IMP_MEST_TYPE_ID = {0} AND IM.CHMS_TYPE_ID = {1} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL));
            }
            if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP))
            {
                impMestTypeFilter.Add(string.Format(" IMMM.IMP_MEST_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL));
            }


            if (filter.EXP_MEST_REASON_IDs != null)
            {
                query += string.Format("AND 1=0 \n", string.Join(",", filter.EXP_MEST_REASON_IDs));
            }
            if (impMestTypeFilter.Count > 0)
            {
                query += string.Format("AND ({0}) \n", string.Join("or ", impMestTypeFilter));
            }
            query += string.Format("AND mety.IS_CHEMICAL_SUBSTANCE ={0} ",IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<DataGet>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00312");

            return result;
        }

    }
}
