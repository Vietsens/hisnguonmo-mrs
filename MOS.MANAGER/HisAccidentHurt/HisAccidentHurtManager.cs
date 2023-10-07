using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurt
{
    public partial class HisAccidentHurtManager : BusinessBase
    {
        public HisAccidentHurtManager()
            : base()
        {

        }

        public HisAccidentHurtManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_HURT> Get(HisAccidentHurtFilterQuery filter)
        {
             List<HIS_ACCIDENT_HURT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_HURT> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_HURT GetById(long data)
        {
             HIS_ACCIDENT_HURT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_HURT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_HURT GetById(long data, HisAccidentHurtFilterQuery filter)
        {
             HIS_ACCIDENT_HURT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_HURT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentHurtGet(param).GetById(data, filter);
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
