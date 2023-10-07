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

namespace MRS.Processor.Mrs00611
{
    public partial class Mrs00611RDOManager : BusinessBase
    {
        public List<Mrs00611RDO> GetRdo(Mrs00611Filter filter)
        {
            try
            {
                List<Mrs00611RDO> result = new List<Mrs00611RDO>();
                string query = "";
                query += "SELECT * ";
                query += "FROM HIS_VITAMIN_A VT ";

                query += "LEFT JOIN HIS_PATIENT ";
                query += "ON VT.PATIENT_ID = HIS_PATIENT.ID ";
                query += "WHERE VT.EXECUTE_TIME is not null ";
                if (filter.TIME_FROM > 0)
                {
                    query += string.Format("AND VT.EXECUTE_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO > 0)
                {
                    query += string.Format("AND VT.EXECUTE_TIME <= {0} ", filter.TIME_TO);
                }
                if (filter.BRANCH_ID != null && filter.BRANCH_ID > 0)
                {
                    query += string.Format("AND VT.BRANCH_ID = {0} ", filter.BRANCH_ID);
                }
                query += "AND VT.CASE_TYPE = 1 ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00611RDO>(query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        public HIS_BRANCH GetBranch(Mrs00611Filter filter)
        {
            HIS_BRANCH result = new HIS_BRANCH();
            try
            {
                if (filter != null && filter.BRANCH_ID != null && filter.BRANCH_ID > 0)
                {
                    string query = "";
                    query += "SELECT * ";
                    query += "FROM HIS_BRANCH ";
                    query += string.Format("WHERE ID = {0} ", filter.BRANCH_ID);
                    Inventec.Common.Logging.LogSystem.Info("SQL get HIS_BRANCH: " + query);
                    result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<HIS_BRANCH>(query);
                }
                
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
