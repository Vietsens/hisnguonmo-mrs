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
using MRS.Proccessor.Mrs00200;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00200
{
    class ManagerSql 
    {

        public List<Mrs00200RDO> Get(HisExpMestMaterialViewFilterQuery filter)
        {
            List<Mrs00200RDO> result = new List<Mrs00200RDO>();
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
                result = new SqlDAO().GetSql<Mrs00200RDO>(query);
               
            return result;
        }
        public List<Mrs00200RDO> Get(HisExpMestMedicineViewFilterQuery filter)
        {
            List<Mrs00200RDO> result = new List<Mrs00200RDO>();
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
            result = new SqlDAO().GetSql<Mrs00200RDO>(query);

            return result;
        }

        public List<HIS_EXP_MEST> Get(HisExpMestFilterQuery filter)
        {
            List<HIS_EXP_MEST> result = new List<HIS_EXP_MEST>();
            string query = "";
            query += "SELECT ";
            query += "EXM.*  ";

            query += "FROM HIS_RS.HIS_EXP_MEST EXM ";

            query += "WHERE 1=1 ";
            if (filter.IDs != null)
            {
                var skip = 0;
                List<string> strIds = new List<string>();
                strIds.Add("EXM.ID IN (-1)");
                while (filter.IDs.Count - skip > 0)
                {
                    var limit = filter.IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    strIds.Add(string.Format("EXM.ID IN ({0}) ", string.Join(",", limit)));
                }
                query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
            }

            Inventec.Common.Logging.LogSystem.Debug("SQL: " + query);
            result = new SqlDAO().GetSql<HIS_EXP_MEST>(query);

            return result;
        }

        public List<HIS_TREATMENT> Get(HisTreatmentFilterQuery filter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            string query = "";
            query += "SELECT ";
            query += "TREA.*  ";

            query += "FROM HIS_RS.HIS_TREATMENT TREA ";

            query += "WHERE 1=1 ";
            if (filter.IDs != null)
            {
                var skip = 0;
                List<string> strIds = new List<string>();
                strIds.Add("TREA.ID IN (-1)");
                while (filter.IDs.Count - skip > 0)
                {
                    var limit = filter.IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    strIds.Add(string.Format("TREA.ID IN ({0}) ", string.Join(",", limit)));
                }
                query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
            }

            Inventec.Common.Logging.LogSystem.Debug("SQL: " + query);
            result = new SqlDAO().GetSql<HIS_TREATMENT>(query);

            return result;
        }

    }
}
