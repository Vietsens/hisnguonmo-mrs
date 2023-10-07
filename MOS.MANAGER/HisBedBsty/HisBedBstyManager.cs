using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedBsty
{
    public partial class HisBedBstyManager : BusinessBase
    {
        public HisBedBstyManager()
            : base()
        {

        }

        public HisBedBstyManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_BED_BSTY> Get(HisBedBstyFilterQuery filter)
        {
            List<HIS_BED_BSTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BED_BSTY> resultData = null;
                if (valid)
                {
                    resultData = new HisBedBstyGet(param).Get(filter);
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

        
        public HIS_BED_BSTY GetById(long data)
        {
            HIS_BED_BSTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BED_BSTY resultData = null;
                if (valid)
                {
                    resultData = new HisBedBstyGet(param).GetById(data);
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

        
        public HIS_BED_BSTY GetById(long data, HisBedBstyFilterQuery filter)
        {
            HIS_BED_BSTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BED_BSTY resultData = null;
                if (valid)
                {
                    resultData = new HisBedBstyGet(param).GetById(data, filter);
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
