using Inventec.Core;
using System;
using MOS.MANAGER.Base;
using MOS.EFMODEL.DataModels;
using System.Collections.Generic;

namespace MOS.MANAGER.HisVitaminA
{
    public partial class HisVitaminAManager : BusinessBase
    {
        public HisVitaminAManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_VITAMIN_A> Get(HisVitaminAFilterQuery filter)
        {
            List<HIS_VITAMIN_A> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_VITAMIN_A> resultData = null;
                if (valid)
                {
                    resultData = new HisVitaminAGet(param).Get(filter);
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

        public HIS_VITAMIN_A GetById(long data)
        {
            HIS_VITAMIN_A result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_VITAMIN_A resultData = null;
                if (valid)
                {
                    resultData = new HisVitaminAGet(param).GetById(data);
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

        public HIS_VITAMIN_A GetById(long data, HisVitaminAFilterQuery filter)
        {
            HIS_VITAMIN_A result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_VITAMIN_A resultData = null;
                if (valid)
                {
                    resultData = new HisVitaminAGet(param).GetById(data, filter);
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
