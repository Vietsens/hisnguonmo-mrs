using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttMethod
{
    public partial class HisPtttMethodManager : BusinessBase
    {
        public HisPtttMethodManager()
            : base()
        {

        }

        public HisPtttMethodManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PTTT_METHOD> Get(HisPtttMethodFilterQuery filter)
        {
             List<HIS_PTTT_METHOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PTTT_METHOD> resultData = null;
                if (valid)
                {
                    resultData = new HisPtttMethodGet(param).Get(filter);
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

        
        public  HIS_PTTT_METHOD GetById(long data)
        {
             HIS_PTTT_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisPtttMethodGet(param).GetById(data);
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

        
        public  HIS_PTTT_METHOD GetById(long data, HisPtttMethodFilterQuery filter)
        {
             HIS_PTTT_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisPtttMethodGet(param).GetById(data, filter);
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

        
        public  HIS_PTTT_METHOD GetByCode(string data)
        {
             HIS_PTTT_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisPtttMethodGet(param).GetByCode(data);
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

        
        public  HIS_PTTT_METHOD GetByCode(string data, HisPtttMethodFilterQuery filter)
        {
             HIS_PTTT_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisPtttMethodGet(param).GetByCode(data, filter);
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
