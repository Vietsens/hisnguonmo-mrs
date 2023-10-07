using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateBlty
{
    public partial class HisAnticipateBltyManager : BusinessBase
    {
        public HisAnticipateBltyManager()
            : base()
        {

        }

        public HisAnticipateBltyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ANTICIPATE_BLTY> Get(HisAnticipateBltyFilterQuery filter)
        {
             List<HIS_ANTICIPATE_BLTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ANTICIPATE_BLTY> resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateBltyGet(param).Get(filter);
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

        
        public  HIS_ANTICIPATE_BLTY GetById(long data)
        {
             HIS_ANTICIPATE_BLTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ANTICIPATE_BLTY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateBltyGet(param).GetById(data);
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

        
        public  HIS_ANTICIPATE_BLTY GetById(long data, HisAnticipateBltyFilterQuery filter)
        {
             HIS_ANTICIPATE_BLTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ANTICIPATE_BLTY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateBltyGet(param).GetById(data, filter);
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
