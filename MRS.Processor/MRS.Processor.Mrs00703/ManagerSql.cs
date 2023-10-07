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

namespace MRS.Processor.Mrs00703
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00703RDO> Get(HisExpMestMaterialViewFilterQuery filter)
        {
            List<Mrs00703RDO> result = new List<Mrs00703RDO>();
            CommonParam paramGet = new CommonParam();
                string query = "";
                query += "SELECT ";
                query += "EXMM.TDL_MEDI_STOCK_ID AS MEDI_STOCK_ID, ";
                query += "NVL(EXMM.MATERIAL_ID,0) AS MEDI_MATE_ID, ";
                query += "SUM(-EXMM.AMOUNT) AS BEGIN_AMOUNT,  ";
                query += "SUM(-EXMM.AMOUNT) AS END_AMOUNT  ";

                query += "FROM HIS_EXP_MEST_MATERIAL EXMM ";
                query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 AND EXMM.EXP_MEST_ID IS NOT NULL ";
                query += string.Format("AND EXMM.TDL_MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
                
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
                result = new SqlDAO().GetSql<Mrs00703RDO>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");
            return result;
        }

        public List<Mrs00703RDO> Get(HisExpMestMedicineViewFilterQuery filter)
        {
            List<Mrs00703RDO> result = new List<Mrs00703RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "EXMM.TDL_MEDI_STOCK_ID AS MEDI_STOCK_ID, ";
            query += "NVL(EXMM.MEDICINE_ID,0) AS MEDI_MATE_ID, ";
            query += "SUM(-EXMM.AMOUNT) AS BEGIN_AMOUNT,  ";
            query += "SUM(-EXMM.AMOUNT) AS END_AMOUNT  ";

            query += "FROM HIS_EXP_MEST_MEDICINE EXMM ";

            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 AND EXMM.EXP_MEST_ID IS NOT NULL ";
            query += string.Format("AND EXMM.TDL_MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));

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
            result = new SqlDAO().GetSql<Mrs00703RDO>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");

            return result;
        }

        public List<V_HIS_IMP_MEST_MEDICINE> GetMediInput(Mrs00703Filter filter, List<long> mediStockIds)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "IMMM.* ";

            query += "FROM V_HIS_IMP_MEST_MEDICINE IMMM ";

            query += "WHERE IMMM.IMP_MEST_STT_ID =2 AND IMMM.IMP_MEST_TYPE_ID = 5 ";
            query += string.Format("AND IMMM.MEDI_STOCK_ID IN ({0}) ", string.Join(",", mediStockIds));


            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND IMMM.CREATE_TIME >={0} ", filter.TIME_FROM);
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<V_HIS_IMP_MEST_MEDICINE>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");

            return result;
        }

        public List<V_HIS_IMP_MEST_MATERIAL> GetMateInput(Mrs00703Filter filter,List<long> mediStockIds)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "IMMM.* ";

            query += "FROM V_HIS_IMP_MEST_MATERIAL IMMM ";

            query += "WHERE IMMM.IMP_MEST_STT_ID =2 AND IMMM.IMP_MEST_TYPE_ID = 5 ";
            query += string.Format("AND IMMM.MEDI_STOCK_ID IN ({0}) ", string.Join(",", mediStockIds));


            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND IMMM.CREATE_TIME >={0} ", filter.TIME_FROM);
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<V_HIS_IMP_MEST_MATERIAL>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");

            return result;
        }

        internal List<ImpMestIdChmsMediStockId> GetChmsMediStockId(long min, long max)
        {
            List<ImpMestIdChmsMediStockId> result = new List<ImpMestIdChmsMediStockId>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "IM.ID IMP_MEST_ID, ";
            query += "EX.MEDI_STOCK_ID CHMS_MEDI_STOCK_ID ";

            query += "FROM HIS_IMP_MEST IM ";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=IM.CHMS_EXP_MEST_ID ";

            query += "WHERE IM.IMP_MEST_TYPE_ID IN({0},{1}) AND IM.CHMS_MEDI_STOCK_ID IS NULL AND IM.ID BETWEEN {2} AND {3} ";
            query = string.Format(query, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, min, max);


            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<ImpMestIdChmsMediStockId>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");

            return result;
        }
    }
}
