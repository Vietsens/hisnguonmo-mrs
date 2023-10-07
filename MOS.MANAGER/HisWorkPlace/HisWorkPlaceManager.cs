using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisWorkPlace
{
    public partial class HisWorkPlaceManager : BusinessBase
    {
        public HisWorkPlaceManager()
            : base()
        {

        }

        public HisWorkPlaceManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_WORK_PLACE> Get(HisWorkPlaceFilterQuery filter)
        {
            List<HIS_WORK_PLACE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_WORK_PLACE> resultData = null;
                if (valid)
                {
                    resultData = new HisWorkPlaceGet(param).Get(filter);
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

        
        public HIS_WORK_PLACE GetById(long data)
        {
            HIS_WORK_PLACE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_WORK_PLACE resultData = null;
                if (valid)
                {
                    resultData = new HisWorkPlaceGet(param).GetById(data);
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

        
        public HIS_WORK_PLACE GetById(long data, HisWorkPlaceFilterQuery filter)
        {
            HIS_WORK_PLACE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_WORK_PLACE resultData = null;
                if (valid)
                {
                    resultData = new HisWorkPlaceGet(param).GetById(data, filter);
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
