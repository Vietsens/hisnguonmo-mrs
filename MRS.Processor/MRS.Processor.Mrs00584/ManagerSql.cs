using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00584
{
    class ManagerSql 
    {

        public List<Mrs00584RDO> Get(HisExpMestMaterialViewFilterQuery filter)
        {
            List<Mrs00584RDO> result = new List<Mrs00584RDO>();
                string query = "";
                query += "SELECT ";
                query += "EXMM.TDL_MEDI_STOCK_ID AS MEDI_STOCK_ID, ";
                query += "NVL(EXMM.MATERIAL_ID,0) AS MATERIAL_ID, ";
                query += "SUM(-EXMM.AMOUNT) AS BEGIN_AMOUNT,  ";
                query += "SUM(-EXMM.AMOUNT) AS END_AMOUNT  ";

                query += "FROM HIS_EXP_MEST_MATERIAL EXMM ";
                query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 AND EXMM.EXP_MEST_ID IS NOT NULL ";
                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("AND EXMM.TDL_MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
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
                query += "EXMM.MATERIAL_ID ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<Mrs00584RDO>(query);
               
            return result;
        }
        public List<Mrs00584RDO> Get(HisExpMestMedicineViewFilterQuery filter)
        {
            List<Mrs00584RDO> result = new List<Mrs00584RDO>();
            string query = "";
            query += "SELECT ";
            query += "EXMM.TDL_MEDI_STOCK_ID AS MEDI_STOCK_ID, ";
            query += "NVL(EXMM.MEDICINE_ID,0) AS MEDICINE_ID, ";
            query += "SUM(-EXMM.AMOUNT) AS BEGIN_AMOUNT,  ";
            query += "SUM(-EXMM.AMOUNT) AS END_AMOUNT  ";

            query += "FROM HIS_EXP_MEST_MEDICINE EXMM ";

            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 AND EXMM.EXP_MEST_ID IS NOT NULL ";
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.TDL_MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
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
            query += "EXMM.MEDICINE_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00584RDO>(query);

            return result;
        }

        public List<ExpMestIdReason> Get(Mrs00584Filter filter)
        {
            List<ExpMestIdReason> result = new List<ExpMestIdReason>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "EX.ID AS EXP_MEST_ID, ";
            query += "ER.ID AS EXP_MEST_REASON_ID, ";
            query += "ER.EXP_MEST_REASON_CODE ";
            query += "FROM HIS_EXP_MEST EX ";
            query += "LEFT JOIN HIS_EXP_MEST_REASON ER ON ER.ID = EX.EXP_MEST_REASON_ID ";

            query += "WHERE EX.EXP_MEST_STT_ID=5 AND EX.IS_DELETE =0 ";
            query += string.Format("AND EX.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));

            if (filter.TIME_TO != null)
            {
                query += string.Format("AND EX.FINISH_TIME < {0} ", filter.TIME_TO);
            }
            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND EX.FINISH_TIME >={0} ", filter.TIME_FROM);
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<ExpMestIdReason>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00584");

            return result;
        }

        public List<V_HIS_IMP_MEST_MEDICINE> GetMediInput(Mrs00584Filter filter)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "IMMM.* ";

            query += "FROM V_HIS_IMP_MEST_MEDICINE IMMM ";

            query += "WHERE IMMM.IMP_MEST_STT_ID =2 AND IMMM.IMP_MEST_TYPE_ID = 5 ";
            query += string.Format("AND IMMM.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));


            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND IMMM.CREATE_TIME >={0} ", filter.TIME_FROM);
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<V_HIS_IMP_MEST_MEDICINE>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00584");

            return result;
        }

        public List<V_HIS_IMP_MEST_MATERIAL> GetMateInput(Mrs00584Filter filter)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "IMMM.* ";

            query += "FROM V_HIS_IMP_MEST_MATERIAL IMMM ";

            query += "WHERE IMMM.IMP_MEST_STT_ID =2 AND IMMM.IMP_MEST_TYPE_ID = 5 ";
            query += string.Format("AND IMMM.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));


            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND IMMM.CREATE_TIME >={0} ", filter.TIME_FROM);
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<V_HIS_IMP_MEST_MATERIAL>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00584");

            return result;
        }

    }
}
