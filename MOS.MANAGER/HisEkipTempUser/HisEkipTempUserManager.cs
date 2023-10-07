using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipTempUser
{
    public partial class HisEkipTempUserManager : BusinessBase
    {
        public HisEkipTempUserManager()
            : base()
        {

        }

        public HisEkipTempUserManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_EKIP_TEMP_USER> Get(HisEkipTempUserFilterQuery filter)
        {
            List<HIS_EKIP_TEMP_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisEkipTempUserGet(param).Get(filter);
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
