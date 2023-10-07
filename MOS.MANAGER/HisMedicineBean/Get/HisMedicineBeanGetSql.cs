using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    partial class HisMedicineBeanGet : GetBase
    {
        internal List<HIS_MEDICINE_BEAN> GetSqlByIds(List<long> ids)
        {
            try
            {
                string querySql = this.AddInClause(ids, "SELECT * FROM HIS_MEDICINE_BEAN WHERE {IN_CLAUSE} ", "ID");
                return DAOWorker.SqlDAO.GetSql<HIS_MEDICINE_BEAN>(querySql);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
