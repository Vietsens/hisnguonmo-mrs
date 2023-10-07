using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMachine
{
    partial class HisMachineGet : BusinessBase
    {
        internal HisMachineGet()
            : base()
        {

        }

        internal HisMachineGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MACHINE> Get(HisMachineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMachineDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MACHINE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMachineFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MACHINE GetById(long id, HisMachineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMachineDAO.GetById(id, filter.Query());
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
