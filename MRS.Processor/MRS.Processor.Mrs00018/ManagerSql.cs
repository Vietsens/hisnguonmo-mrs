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

namespace MRS.Processor.Mrs00018
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
        public List<HIS_MEDICINE> GetMedicine(Mrs00018Filter filter)
        {
            List<HIS_MEDICINE> result = new List<HIS_MEDICINE>();

            string query = "";
            query += "SELECT ";
            query += "MA.* ";

            query += "FROM HIS_RS.HIS_MEDICINE MA ";

            query += "WHERE MA.IS_DELETE =0 ";

            query += "AND EXISTS (SELECT 1 FROM  HIS_RS.HIS_SERE_SERV SS JOIN HIS_RS.HIS_HEIN_APPROVAL HAP  ON SS.HEIN_APPROVAL_ID=HAP.ID ";
            query += "WHERE 1=1 ";
            query += "AND SS.MEDICINE_ID=MA.ID ";
            if (filter.TIME_TO != null)
            {
                query += string.Format("AND HAP.EXECUTE_TIME < {0} ", filter.TIME_TO);
            }
            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND HAP.EXECUTE_TIME >={0}   ", filter.TIME_FROM);
            }
            query += ") ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<HIS_MEDICINE>(param, query);

            return result;
        }
    }
}
