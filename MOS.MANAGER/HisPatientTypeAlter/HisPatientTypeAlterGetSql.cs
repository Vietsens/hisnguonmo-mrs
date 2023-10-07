using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAlter
{
    partial class HisPatientTypeAlterGet : GetBase
    {
        internal List<D_HIS_PATIENT_TYPE_ALTER_1> GetDHisPatientTypeAlter1(DHisPatientTypeAlter1Filter filter)
        {
            try
            {
                string query = "SELECT * FROM D_HIS_PATIENT_TYPE_ALTER_1 WHERE 1 = 1 ";
                if (filter.TREATMENT_ID.HasValue)
                {
                    query += string.Format(" AND TREATMENT_ID = {0}", filter.TREATMENT_ID.Value);
                }

                return DAOWorker.SqlDAO.GetSql<D_HIS_PATIENT_TYPE_ALTER_1>(query);
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
