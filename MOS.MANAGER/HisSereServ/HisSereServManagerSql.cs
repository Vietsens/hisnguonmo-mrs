using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.HisSereServ
{
    public partial class HisSereServManager : BusinessBase
    {
        public List<HIS_SERE_SERV> GetHisSereServByTransactionIds(List<long> transactionIds)
        {
            try
            {
                if (transactionIds != null && transactionIds.Count > 0)
                {
                    List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
                    string sql = "SELECT * FROM HIS_SERE_SERV SS WHERE EXISTS (SELECT SSB.SERE_SERV_ID FROM HIS_SERE_SERV_BILL SSB WHERE SS.ID = SSB.SERE_SERV_ID AND SSB.BILL_ID IN ({0})) ";

                    if (transactionIds.Count < 1000)
                    {
                        string query = string.Format(sql, string.Join(",", transactionIds));
                        result = DAOWorker.SqlDAO.GetSql<HIS_SERE_SERV>(query);
                    }
                    else
                    {
                        var skip = 0;
                        while (transactionIds.Count - skip > 0)
                        {
                            var Ids = transactionIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            string query = string.Format(sql, string.Join(",", Ids));
                            var data = DAOWorker.SqlDAO.GetSql<HIS_SERE_SERV>(query);
                            if (data != null)
                                result.AddRange(data);
                        }
                    }
                    return result;
                }
                else
                    return null;
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
