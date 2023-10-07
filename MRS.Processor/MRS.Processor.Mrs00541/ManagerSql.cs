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
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00541
{
    public class ManagerSql
    {
        public List<TreatmentId> GetTreatment(Mrs00541Filter filter)
        {
            List<TreatmentId> result = new List<TreatmentId>();
            try
            {
                string query = "";
                query += "select  ";
                query += "distinct(trea.id) id ";
                query += "from ";
                query += "HIS_TREATMENT trea  ";
                query += "left join v_his_hein_approval hap on hap.treatment_id=trea.id ";
                query += "where 1=1 ";

                if (filter.CHOOSE_TIME != null && filter.CHOOSE_TIME == true)
                {
                    query += "and trea.is_pause=1 ";

                    query += string.Format("and trea.out_time between {0} and {1} ", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);

                    if (filter.BRANCH_ID != null)
                    {
                        query += string.Format("AND trea.branch_id = {0} ", filter.BRANCH_ID);
                    }
                    if (filter.BRANCH_IDs != null)
                    {
                        query += string.Format("AND trea.branch_id in ({0}) ", string.Join(",",filter.BRANCH_IDs));
                    }
                }
                else
                {
                    query += "and hap.is_delete=0 ";

                    query += string.Format("and hap.execute_time between {0} and {1} ", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);

                    if (filter.BRANCH_ID != null)
                    {
                        query += string.Format("AND hap.branch_id = {0} ", filter.BRANCH_ID);
                    }
                    if (filter.BRANCH_IDs != null)
                    {
                        query += string.Format("AND hap.branch_id in ({0}) ", string.Join(",", filter.BRANCH_IDs));
                    }
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TreatmentId>(query)?? new List<TreatmentId>();
               
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
