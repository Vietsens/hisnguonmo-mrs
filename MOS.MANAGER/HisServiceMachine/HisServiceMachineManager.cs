using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMachine
{
    public partial class HisServiceMachineManager : BusinessBase
    {
        public HisServiceMachineManager()
            : base()
        {

        }

        public HisServiceMachineManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_SERVICE_MACHINE> Get(HisServiceMachineFilterQuery filter)
        {
            List<HIS_SERVICE_MACHINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisServiceMachineGet(param).Get(filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_SERVICE_MACHINE> GetByMachineId(long machineId)
        {
            List<HIS_SERVICE_MACHINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisServiceMachineGet(param).GetByMachineId(machineId);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
