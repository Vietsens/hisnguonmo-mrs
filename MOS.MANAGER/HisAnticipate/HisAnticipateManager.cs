using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipate
{
    public partial class HisAnticipateManager : BusinessBase
    {
        public HisAnticipateManager()
            : base()
        {

        }

        public HisAnticipateManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ANTICIPATE> Get(HisAnticipateFilterQuery filter)
        {
             List<HIS_ANTICIPATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ANTICIPATE> resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).Get(filter);
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

        
        public  HIS_ANTICIPATE GetById(long data)
        {
             HIS_ANTICIPATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ANTICIPATE resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).GetById(data);
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

        
        public  HIS_ANTICIPATE GetById(long data, HisAnticipateFilterQuery filter)
        {
             HIS_ANTICIPATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ANTICIPATE resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateGet(param).GetById(data, filter);
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
