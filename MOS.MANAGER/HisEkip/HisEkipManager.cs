using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkip
{
    public partial class HisEkipManager : BusinessBase
    {
        public HisEkipManager()
            : base()
        {

        }

        public HisEkipManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EKIP> Get(HisEkipFilterQuery filter)
        {
            List<HIS_EKIP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EKIP> resultData = null;
                if (valid)
                {
                    resultData = new HisEkipGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_EKIP GetById(long filter)
        {
            HIS_EKIP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                HIS_EKIP resultData = null;
                if (valid)
                {
                    resultData = new HisEkipGet(param).GetById(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_EKIP GetById(long id, HisEkipFilterQuery filter)
        {
            HIS_EKIP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                HIS_EKIP resultData = null;
                if (valid)
                {
                    resultData = new HisEkipGet(param).GetById(id, filter);
                }
                result = resultData;
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
