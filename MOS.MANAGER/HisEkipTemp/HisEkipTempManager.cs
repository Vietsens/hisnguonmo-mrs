using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipTemp
{
    public partial class HisEkipTempManager : BusinessBase
    {
        public HisEkipTempManager()
            : base()
        {

        }

        public HisEkipTempManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_EKIP_TEMP> Get(HisEkipTempFilterQuery filter)
        {
            List<HIS_EKIP_TEMP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisEkipTempGet(param).Get(filter);
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
