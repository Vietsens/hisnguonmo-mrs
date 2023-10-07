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

namespace MRS.Processor.Mrs00647
{
    public partial class ManagerSql : BusinessBase
    {
        public ManagerSql()
            : base()
        {

        }

        public ManagerSql(CommonParam param)
            : base(param)
        {

        }
        public List<V_HIS_EXP_MEST_MATERIAL> GetView(HisExpMestMaterialViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = new List<V_HIS_EXP_MEST_MATERIAL>();
           
                string query = "";
                query += "SELECT ";
                query += "EXMA.* ";

                query += "FROM V_HIS_EXP_MEST_MATERIAL EXMA ";
                query += "WHERE IS_EXPORT=1 AND IS_DELETE =0 ";
                query += string.Format("AND EXMA.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
                
                if (filter.EXP_TIME_TO != null)
                {
                    query += string.Format("AND EXMA.EXP_TIME < {0} ", filter.EXP_TIME_TO);
                }
                if (filter.EXP_TIME_FROM != null)
                {
                    query += string.Format("AND EXMA.EXP_TIME >={0}   ", filter.EXP_TIME_FROM);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<V_HIS_EXP_MEST_MATERIAL>(param,query);

            return result;
        }
        public List<V_HIS_EXP_MEST_MEDICINE> GetView(HisExpMestMedicineViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = new List<V_HIS_EXP_MEST_MEDICINE>();
           
                string query = "";
                query += "SELECT ";
                query += "EXMA.* ";

                query += "FROM V_HIS_EXP_MEST_MEDICINE EXMA ";
                query += "WHERE IS_EXPORT=1 AND IS_DELETE =0 ";
                query += string.Format("AND EXMA.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));

                if (filter.EXP_TIME_TO != null)
                {
                    query += string.Format("AND EXMA.EXP_TIME < {0} ", filter.EXP_TIME_TO);
                }
                if (filter.EXP_TIME_FROM != null)
                {
                    query += string.Format("AND EXMA.EXP_TIME >={0}   ", filter.EXP_TIME_FROM);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<V_HIS_EXP_MEST_MEDICINE>(param,query);


            return result;
        }

    }
}
