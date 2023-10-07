using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisOtherPaySource
{
    public partial class HisOtherPaySourceManager : BusinessBase
    {
        public HisOtherPaySourceManager()
            : base()
        {

        }
        
        public HisOtherPaySourceManager(CommonParam param)
            : base(param)
        {

        }
		
        public List<HIS_OTHER_PAY_SOURCE> Get(HisOtherPaySourceFilterQuery filter)
        {
            List<HIS_OTHER_PAY_SOURCE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisOtherPaySourceGet(param).Get(filter);
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
