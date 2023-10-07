using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServMaty
{
    public partial class HisSereServMatyManager : BusinessBase
    {
        public HisSereServMatyManager()
            : base()
        {

        }

        public HisSereServMatyManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_SERE_SERV_MATY> Get(HisSereServMatyFilterQuery filter)
        {
            List<HIS_SERE_SERV_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisSereServMatyGet(param).Get(filter);
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
