using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMemaGroup
{
    partial class HisMemaGroupGet : BusinessBase
    {
        internal HIS_MEMA_GROUP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMemaGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEMA_GROUP GetByCode(string code, HisMemaGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMemaGroupDAO.GetByCode(code, filter.Query());
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
