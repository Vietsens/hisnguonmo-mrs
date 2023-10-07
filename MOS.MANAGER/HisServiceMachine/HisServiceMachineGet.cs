using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMachine
{
    partial class HisServiceMachineGet : BusinessBase
    {
        internal HisServiceMachineGet()
            : base()
        {

        }

        internal HisServiceMachineGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_MACHINE> Get(HisServiceMachineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMachineDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_MACHINE GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceMachineFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_MACHINE GetById(long id, HisServiceMachineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMachineDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_MACHINE> GetByMachineId(long id)
        {
            HisServiceMachineFilterQuery filter = new HisServiceMachineFilterQuery();
            filter.MACHINE_ID = id;
            return this.Get(filter);
        }
    }
}
