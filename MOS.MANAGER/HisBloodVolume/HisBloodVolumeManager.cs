using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodVolume
{
    public partial class HisBloodVolumeManager : BusinessBase
    {
        public HisBloodVolumeManager()
            : base()
        {

        }

        public HisBloodVolumeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BLOOD_VOLUME> Get(HisBloodVolumeFilterQuery filter)
        {
             List<HIS_BLOOD_VOLUME> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BLOOD_VOLUME> resultData = null;
                if (valid)
                {
                    resultData = new HisBloodVolumeGet(param).Get(filter);
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

        
        public  HIS_BLOOD_VOLUME GetById(long data)
        {
             HIS_BLOOD_VOLUME result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_VOLUME resultData = null;
                if (valid)
                {
                    resultData = new HisBloodVolumeGet(param).GetById(data);
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

        
        public  HIS_BLOOD_VOLUME GetById(long data, HisBloodVolumeFilterQuery filter)
        {
             HIS_BLOOD_VOLUME result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_VOLUME resultData = null;
                if (valid)
                {
                    resultData = new HisBloodVolumeGet(param).GetById(data, filter);
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
