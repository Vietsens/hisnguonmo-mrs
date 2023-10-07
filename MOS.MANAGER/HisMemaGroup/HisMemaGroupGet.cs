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
        internal HisMemaGroupGet()
            : base()
        {

        }

        internal HisMemaGroupGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEMA_GROUP> Get(HisMemaGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMemaGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEMA_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisMemaGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEMA_GROUP GetById(long id, HisMemaGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMemaGroupDAO.GetById(id, filter.Query());
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
