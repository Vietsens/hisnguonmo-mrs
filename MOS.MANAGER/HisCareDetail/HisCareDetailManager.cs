using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareDetail
{
    public partial class HisCareDetailManager : BusinessBase
    {
        public HisCareDetailManager()
            : base()
        {

        }

        public HisCareDetailManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_CARE_DETAIL> Get(HisCareDetailFilterQuery filter)
        {
             List<HIS_CARE_DETAIL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CARE_DETAIL> resultData = null;
                if (valid)
                {
                    resultData = new HisCareDetailGet(param).Get(filter);
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

        
        public  HIS_CARE_DETAIL GetById(long data)
        {
             HIS_CARE_DETAIL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CARE_DETAIL resultData = null;
                if (valid)
                {
                    resultData = new HisCareDetailGet(param).GetById(data);
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

        
        public  HIS_CARE_DETAIL GetById(long data, HisCareDetailFilterQuery filter)
        {
             HIS_CARE_DETAIL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CARE_DETAIL resultData = null;
                if (valid)
                {
                    resultData = new HisCareDetailGet(param).GetById(data, filter);
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
