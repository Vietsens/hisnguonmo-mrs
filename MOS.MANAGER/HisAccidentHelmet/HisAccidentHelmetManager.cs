using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHelmet
{
    public partial class HisAccidentHelmetManager : BusinessBase
    {
        public HisAccidentHelmetManager()
            : base()
        {

        }

        public HisAccidentHelmetManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_HELMET> Get(HisAccidentHelmetFilterQuery filter)
        {
             List<HIS_ACCIDENT_HELMET> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_HELMET> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHelmetGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_HELMET GetById(long data)
        {
             HIS_ACCIDENT_HELMET result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_HELMET resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHelmetGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_HELMET GetById(long data, HisAccidentHelmetFilterQuery filter)
        {
             HIS_ACCIDENT_HELMET result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_HELMET resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHelmetGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_HELMET GetByCode(string data)
        {
             HIS_ACCIDENT_HELMET result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_HELMET resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHelmetGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_HELMET GetByCode(string data, HisAccidentHelmetFilterQuery filter)
        {
             HIS_ACCIDENT_HELMET result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_HELMET resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHelmetGet(param).GetByCode(data, filter);
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
