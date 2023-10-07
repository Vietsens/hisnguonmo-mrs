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

namespace MRS.Processor.Mrs00637
{
    public partial class ManagerSql : BusinessBase
    {

        public List<DetailRDOMaterial> GetMaterial(Mrs00637Filter filter)
        {
            List<DetailRDOMaterial> result = new List<DetailRDOMaterial>();
            CommonParam paramGet = new CommonParam();
                string query = "";
                query += "SELECT ";
                query += "EXMM.MATERIAL_TYPE_ID,  ";
                query += "EXMM.MATERIAL_TYPE_CODE,  ";
                query += "EXMM.MATERIAL_TYPE_NAME,  ";
                query += "EXMM.SERVICE_UNIT_NAME,  ";
                query += "EXMM.PRICE,  ";
                query += "EXMM.AMOUNT,  ";
                query += "SR.INTRUCTION_TIME  ";

                query += "FROM V_HIS_EXP_MEST_MATERIAL EXMM, ";
                query += "HIS_SERVICE_REQ SR ";
                query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 AND EXMM.TDL_SERVICE_REQ_ID = SR.ID and sr.is_delete=0 ";
                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("AND EXMM.TDL_MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME >={0} ", filter.TIME_FROM);
                }

                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID ={0} ", filter.REQUEST_DEPARTMENT_ID);
                }

                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN {0} ", filter.REQUEST_DEPARTMENT_IDs);
                }
               
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<DetailRDOMaterial>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00637");
            return result;
        }
        public List<DetailRDOMedicine> GetMedicine(Mrs00637Filter filter)
        {
            List<DetailRDOMedicine> result = new List<DetailRDOMedicine>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT ";
            query += "EXMM.MEDICINE_TYPE_ID,  ";
            query += "EXMM.MEDICINE_TYPE_CODE,  ";
            query += "EXMM.MEDICINE_TYPE_NAME,  ";
            query += "EXMM.SERVICE_UNIT_NAME,  ";
            query += "EXMM.PRICE,  ";
            query += "EXMM.AMOUNT,  ";
            query += "SR.INTRUCTION_TIME  ";

            query += "FROM V_HIS_EXP_MEST_MEDICINE EXMM, ";
            query += "HIS_SERVICE_REQ SR ";
            query += "WHERE EXMM.IS_EXPORT=1 AND EXMM.IS_DELETE =0 AND EXMM.TDL_SERVICE_REQ_ID = SR.ID and sr.is_delete=0 ";
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXMM.TDL_MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
            }

            if (filter.TIME_TO != null)
            {
                query += string.Format("AND SR.INTRUCTION_TIME < {0} ", filter.TIME_TO);
            }
            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND SR.INTRUCTION_TIME >={0} ", filter.TIME_FROM);
            }

            if (filter.REQUEST_DEPARTMENT_ID != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID ={0} ", filter.REQUEST_DEPARTMENT_ID);
            }

            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN {0} ", filter.REQUEST_DEPARTMENT_IDs);
            }
               
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<DetailRDOMedicine>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00637");

            return result;
        }

    }
}
