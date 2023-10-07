using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReq
{
    partial class HisServiceReqGet : GetBase
    {
        internal List<D_HIS_SERVICE_REQ_2> GetDHisServiceReq2(DHisServiceReq2Filter filter)
        {
            try
            {
                string query = "SELECT * FROM D_HIS_SERVICE_REQ_2 WHERE 1 = 1 ";
                if (filter.TREATMENT_ID.HasValue)
                {
                    query += string.Format(" AND TREATMENT_ID = {0}", filter.TREATMENT_ID.Value);
                }
                if (filter.REQUEST_DEPARTMENT_ID.HasValue)
                {
                    query += string.Format(" AND REQUEST_DEPARTMENT_ID = {0}", filter.REQUEST_DEPARTMENT_ID.Value);
                }

                return DAOWorker.SqlDAO.GetSql<D_HIS_SERVICE_REQ_2>(query);
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
