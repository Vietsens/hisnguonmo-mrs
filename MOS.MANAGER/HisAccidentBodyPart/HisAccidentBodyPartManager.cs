using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentBodyPart
{
    public partial class HisAccidentBodyPartManager : BusinessBase
    {
        public HisAccidentBodyPartManager()
            : base()
        {

        }

        public HisAccidentBodyPartManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_BODY_PART> Get(HisAccidentBodyPartFilterQuery filter)
        {
             List<HIS_ACCIDENT_BODY_PART> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_BODY_PART> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentBodyPartGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_BODY_PART GetById(long data)
        {
             HIS_ACCIDENT_BODY_PART result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_BODY_PART resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentBodyPartGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_BODY_PART GetById(long data, HisAccidentBodyPartFilterQuery filter)
        {
             HIS_ACCIDENT_BODY_PART result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_BODY_PART resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentBodyPartGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_BODY_PART GetByCode(string data)
        {
             HIS_ACCIDENT_BODY_PART result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_BODY_PART resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentBodyPartGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_BODY_PART GetByCode(string data, HisAccidentBodyPartFilterQuery filter)
        {
             HIS_ACCIDENT_BODY_PART result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_BODY_PART resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentBodyPartGet(param).GetByCode(data, filter);
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
