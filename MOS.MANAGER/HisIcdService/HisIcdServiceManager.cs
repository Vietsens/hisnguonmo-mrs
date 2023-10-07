using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdService
{
    public partial class HisIcdServiceManager : BusinessBase
    {
        public HisIcdServiceManager()
            : base()
        {

        }

        public HisIcdServiceManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_ICD_SERVICE> Get(HisIcdServiceFilterQuery filter)
        {
            List<HIS_ICD_SERVICE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisIcdServiceGet(param).Get(filter);
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
