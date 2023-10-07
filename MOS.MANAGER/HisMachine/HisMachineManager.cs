using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMachine
{
    public partial class HisMachineManager : BusinessBase
    {
        public HisMachineManager()
            : base()
        {

        }

        public HisMachineManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_MACHINE> Get(HisMachineFilterQuery filter)
        {
            List<HIS_MACHINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisMachineGet(param).Get(filter);
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
