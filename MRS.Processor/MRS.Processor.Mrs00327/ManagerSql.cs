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

namespace MRS.Processor.Mrs00327
{
    public partial class ManagerSql : BusinessBase
    {

        List<long> CHMS_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
        };
        public List<Mrs00327RDO> Get(HisExpMestMaterialViewFilterQuery filter)
        {
            List<Mrs00327RDO> result = new List<Mrs00327RDO>();
            CommonParam paramGet = new CommonParam();
                string query = "";
                query += "SELECT ";
                query += "EXMM.TDL_MEDI_STOCK_ID AS MEDI_STOCK_ID, ";
                query += "NVL(EXMM.TDL_MATERIAL_TYPE_ID,0) AS SERVICE_TYPE_ID, ";
                query += "(NVL(EXMM.IMP_PRICE,0)*(1+NVL(EXMM.IMP_VAT_RATIO,0))) AS IMP_PRICE, ";
                query += "SUM(-EXMM.AMOUNT) AS BEGIN_AMOUNT  ";

                query += "FROM V_HIS_EXP_MEST_MATERIAL EXMM ";
                query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 ";
                query += string.Format("AND EXMM.EXP_MEST_TYPE_ID NOT IN ({0}) ", string.Join(",", CHMS_EXP_MEST_TYPE_IDs));
                if (filter.EXP_MEST_IDs != null)
                {
                    query += string.Format("AND EXMM.EXP_MEST_ID IN ({0}) ", string.Join(",", filter.EXP_MEST_IDs));
                }
                if (filter.MATERIAL_TYPE_IDs != null)
                {
                    query += string.Format("AND EXMM.MATERIAL_TYPE_ID IN ({0}) ", string.Join(",", filter.MATERIAL_TYPE_IDs));
                }

                if (filter.EXP_TIME_TO != null)
                {
                    query += string.Format("AND EXMM.EXP_TIME < {0} ", filter.EXP_TIME_TO);
                }
                if (filter.EXP_TIME_FROM != null)
                {
                    query += string.Format("AND EXMM.EXP_TIME >={0} ", filter.EXP_TIME_FROM);
                }
                query += "GROUP BY ";
                query += "EXMM.TDL_MEDI_STOCK_ID, ";
                query += "NVL(EXMM.TDL_MATERIAL_TYPE_ID,0), ";
                query += "(NVL(EXMM.IMP_PRICE,0)*(1+NVL(EXMM.IMP_VAT_RATIO,0))) ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<Mrs00327RDO>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00327");
            return result;
        }
        public List<Mrs00327RDO> Get(HisExpMestMedicineViewFilterQuery filter)
        {
            List<Mrs00327RDO> result = new List<Mrs00327RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "EXMM.TDL_MEDI_STOCK_ID AS MEDI_STOCK_ID, ";
            query += "NVL(EXMM.TDL_MEDICINE_TYPE_ID,0) AS SERVICE_TYPE_ID, ";
            query += "(NVL(EXMM.IMP_PRICE,0)*(1+NVL(EXMM.IMP_VAT_RATIO,0))) AS IMP_PRICE, ";
            query += "SUM(-EXMM.AMOUNT) AS BEGIN_AMOUNT  ";

            query += "FROM V_HIS_EXP_MEST_MEDICINE EXMM ";
            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 ";
            query += string.Format("AND EXMM.EXP_MEST_TYPE_ID NOT IN ({0}) ", string.Join(",", CHMS_EXP_MEST_TYPE_IDs));
            if (filter.EXP_MEST_IDs != null)
            {
                query += string.Format("AND EXMM.EXP_MEST_ID IN ({0}) ", string.Join(",", filter.EXP_MEST_IDs));
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("AND EXMM.MEDICINE_TYPE_ID IN ({0}) ", string.Join(",", filter.MEDICINE_TYPE_IDs));
            }

            if (filter.EXP_TIME_TO != null)
            {
                query += string.Format("AND EXMM.EXP_TIME < {0} ", filter.EXP_TIME_TO);
            }
            if (filter.EXP_TIME_FROM != null)
            {
                query += string.Format("AND EXMM.EXP_TIME >={0} ", filter.EXP_TIME_FROM);
            }
            query += "GROUP BY ";
            query += "EXMM.TDL_MEDI_STOCK_ID, ";
            query += "NVL(EXMM.TDL_MEDICINE_TYPE_ID,0), ";
            query += "(NVL(EXMM.IMP_PRICE,0)*(1+NVL(EXMM.IMP_VAT_RATIO,0))) ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00327RDO>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00327");

            return result;
        }

    }
}
