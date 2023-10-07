using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00067
{
    class ManagerSql
    {
        public List<CHECK_AMOUNT> GetAmountChecked(Mrs00067Filter filter)
        {
            List<CHECK_AMOUNT> result = null;
            try
            {
                string query = "select imp.medicine_type_id, imp.medi_stock_id, 0 as exp_amount, imp.amount as IMP_AMOUNT, imp.package_number, imp.expired_date\n";
                query += "from v_his_imp_mest_medicine imp\n";
                
                query += "where 1=1 \n";


                if (filter.MEDICINE_TYPE_CODEs != null)
                {
                    filter.MEDICINE_TYPE_ID = null;
                    query += string.Format("and imp.medicine_type_id in {0} \n", filter.MEDICINE_TYPE_IDs);
                
                }
               
                    if (filter.MEDICINE_TYPE_ID != null)
                    {
                        query += string.Format("and imp.medicine_type_id = {0} \n", filter.MEDICINE_TYPE_ID);
                    }
                
               


                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("and imp.medi_stock_id in ({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("and imp.medi_stock_id = {0} \n", filter.MEDI_STOCK_ID);
                }
                query += "union all \n";
                query += "select exp.medicine_type_id, exp.medi_stock_id, sum(nvl(exp.amount,0)) as EXP_AMOUNT, 0 as imp_amount, exp.package_number, exp.expired_date \n";

                query += "from v_his_exp_mest_medicine exp\n";
                query += "where 1=1 \n";

                if (filter.MEDICINE_TYPE_CODEs != null)
                {
                    filter.MEDICINE_TYPE_ID = null;
                    query += string.Format("and exp.medicine_type_id in {0} \n", filter.MEDICINE_TYPE_IDs);

                }
                
                    if (filter.MEDICINE_TYPE_ID != null)
                    {
                        query += string.Format("and exp.medicine_type_id = {0} \n", filter.MEDICINE_TYPE_ID);
                    }
                
              

                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("and exp.medi_stock_id in ({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("and exp.medi_stock_id = {0} \n", filter.MEDI_STOCK_ID);
                }
                query += "group by exp.medicine_type_id, exp.medi_stock_id, exp.package_number, exp.expired_date";
                result = new MOS.DAO.Sql.SqlDAO().GetSql<CHECK_AMOUNT>(query);
                LogSystem.Info("SQL:" + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return result;
            }
            return result;
        }

        
    }
}
