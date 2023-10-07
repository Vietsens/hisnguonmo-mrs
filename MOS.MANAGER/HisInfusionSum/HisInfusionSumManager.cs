using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInfusionSum
{
    public partial class HisInfusionSumManager : BusinessBase
    {
        public HisInfusionSumManager()
            : base()
        {

        }

        public HisInfusionSumManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_INFUSION_SUM> Get(HisInfusionSumFilterQuery filter)
        {
             List<HIS_INFUSION_SUM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_INFUSION_SUM> resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionSumGet(param).Get(filter);
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

        
        public  HIS_INFUSION_SUM GetById(long data)
        {
             HIS_INFUSION_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_INFUSION_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionSumGet(param).GetById(data);
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

        
        public  HIS_INFUSION_SUM GetById(long data, HisInfusionSumFilterQuery filter)
        {
             HIS_INFUSION_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_INFUSION_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisInfusionSumGet(param).GetById(data, filter);
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
