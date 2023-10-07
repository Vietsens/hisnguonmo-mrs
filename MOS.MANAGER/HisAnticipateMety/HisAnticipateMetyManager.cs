using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMety
{
    public partial class HisAnticipateMetyManager : BusinessBase
    {
        public HisAnticipateMetyManager()
            : base()
        {

        }

        public HisAnticipateMetyManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_ANTICIPATE_METY> Get(HisAnticipateMetyFilterQuery filter)
        {
            List<HIS_ANTICIPATE_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ANTICIPATE_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMetyGet(param).Get(filter);
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

        
        public HIS_ANTICIPATE_METY GetById(long data)
        {
            HIS_ANTICIPATE_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ANTICIPATE_METY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMetyGet(param).GetById(data);
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

        
        public HIS_ANTICIPATE_METY GetById(long data, HisAnticipateMetyFilterQuery filter)
        {
            HIS_ANTICIPATE_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ANTICIPATE_METY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMetyGet(param).GetById(data, filter);
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
