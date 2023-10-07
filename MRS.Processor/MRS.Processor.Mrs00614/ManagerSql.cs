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

namespace MRS.Processor.Mrs00614
{
    public partial class Mrs00614RDOManager : BusinessBase
    {
        public List<Mrs00614RDO> GetRdo(Mrs00614Filter filter)
        {
            try
            {
                List<Mrs00614RDO> result = new List<Mrs00614RDO>();
                string query = "";
                query += "SELECT * ";
                query += "FROM HIS_VITAMIN_A VT ";

                query += "LEFT JOIN HIS_BRANCH ";
                query += "ON VT.BRANCH_ID = HIS_BRANCH.ID WHERE ";
                if (filter.YEAR > 0)
                {
                    query += string.Format("VT.EXECUTE_TIME >= {0} ", filter.YEAR.ToString().Substring(0,4) + "0101000000 ");
                    query += string.Format("AND VT.EXECUTE_TIME <= {0} ", filter.YEAR.ToString().Substring(0, 4) + "1231235959 ");
                }
                query += "AND (VT.CASE_TYPE = 1 OR VT.CASE_TYPE = 2) ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00614RDO>(query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

    }
}
