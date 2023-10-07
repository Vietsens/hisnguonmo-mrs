using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentLocation
{
    public partial class HisAccidentLocationManager : BusinessBase
    {
        public HisAccidentLocationManager()
            : base()
        {

        }

        public HisAccidentLocationManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_LOCATION> Get(HisAccidentLocationFilterQuery filter)
        {
             List<HIS_ACCIDENT_LOCATION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_LOCATION> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentLocationGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_LOCATION GetById(long data)
        {
             HIS_ACCIDENT_LOCATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_LOCATION resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentLocationGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_LOCATION GetById(long data, HisAccidentLocationFilterQuery filter)
        {
             HIS_ACCIDENT_LOCATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_LOCATION resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentLocationGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_LOCATION GetByCode(string data)
        {
             HIS_ACCIDENT_LOCATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_LOCATION resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentLocationGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_LOCATION GetByCode(string data, HisAccidentLocationFilterQuery filter)
        {
             HIS_ACCIDENT_LOCATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_LOCATION resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentLocationGet(param).GetByCode(data, filter);
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
