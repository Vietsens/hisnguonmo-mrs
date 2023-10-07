using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00566
{
    public partial class HisSereServManager : BusinessBase
    {
        public List<HIS_SERE_SERV> GetHisSereServByTransactionTime(long TRANSACTION_TIME_FROM, long TRANSACTION_TIME_TO)
        {
            try
            {
                    List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
                    string sql = "SELECT * FROM HIS_SERE_SERV SS WHERE EXISTS (SELECT 1 FROM HIS_SERE_SERV_BILL SSB WHERE SS.ID = SSB.SERE_SERV_ID AND EXISTS (SELECT 1 FROM HIS_TRANSACTION TRS WHERE TRS.ID = SSB.BILL_ID AND TRANSACTION_TIME BETWEEN {0} AND {1} AND NVL(TRS.IS_CANCEL,0)<>1)) ";
                    string query = string.Format(sql, TRANSACTION_TIME_FROM, TRANSACTION_TIME_TO);
                        result = DAOWorker.SqlDAO.GetSql<HIS_SERE_SERV>(query);
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
