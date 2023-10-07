using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisTreatment;
using MOS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisServiceReq
{
    partial class HisServiceReqGet : GetBase
    {
        internal List<HisServiceReqGroupByDateSDO> GetGroupByDate(long treatmentId)
        {
            try
            {
                string query =
                    "SELECT (INTRUCTION_TIME - MOD(INTRUCTION_TIME, 1000000)) AS InstructionDate, TREATMENT_ID as TreatmentId, count(id) as Total "
                    + " FROM HIS_SERVICE_REQ "
                    + " WHERE TREATMENT_ID = :param1 "
                    + " GROUP by (INTRUCTION_TIME - MOD(INTRUCTION_TIME, 1000000)), TREATMENT_ID";
                return DAOWorker.SqlDAO.GetSql<HisServiceReqGroupByDateSDO>(query, treatmentId);
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
