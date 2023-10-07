using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDhst
{
    public partial class HisDhstManager : BusinessBase
    {
        public HisDhstManager()
            : base()
        {

        }

        public HisDhstManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_DHST> Get(HisDhstFilterQuery filter)
        {
            List<HIS_DHST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DHST> resultData = null;
                if (valid)
                {
                    resultData = new HisDhstGet(param).Get(filter);
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

        
        public HIS_DHST GetById(long data)
        {
            HIS_DHST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DHST resultData = null;
                if (valid)
                {
                    resultData = new HisDhstGet(param).GetById(data);
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

        
        public HIS_DHST GetById(long data, HisDhstFilterQuery filter)
        {
            HIS_DHST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DHST resultData = null;
                if (valid)
                {
                    resultData = new HisDhstGet(param).GetById(data, filter);
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

        
        public List<HIS_DHST> GetByTreatmentId(long filter)
        {
            List<HIS_DHST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DHST> resultData = null;
                if (valid)
                {
                    resultData = new HisDhstGet(param).GetByTreatmentId(filter);
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

        
        public List<HIS_DHST> GetByTrackingId(long filter)
        {
            List<HIS_DHST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DHST> resultData = null;
                if (valid)
                {
                    resultData = new HisDhstGet(param).GetByTrackingId(filter);
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
