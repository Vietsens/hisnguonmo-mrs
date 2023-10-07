using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrain
{
    public partial class HisRehaTrainManager : BusinessBase
    {
        public HisRehaTrainManager()
            : base()
        {

        }

        public HisRehaTrainManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_REHA_TRAIN> Get(HisRehaTrainFilterQuery filter)
        {
            List<HIS_REHA_TRAIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisRehaTrainGet(param).Get(filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<V_HIS_REHA_TRAIN> GetView(HisRehaTrainViewFilterQuery filter)
        {
            List<V_HIS_REHA_TRAIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisRehaTrainGet(param).GetView(filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<V_HIS_REHA_TRAIN> GetViewByRehaSumId(long rehaSumId)
        {
            List<V_HIS_REHA_TRAIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisRehaTrainGet(param).GetViewByRehaSumId(rehaSumId);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<V_HIS_REHA_TRAIN> GetViewByServiceReqId(long serviceReqId)
        {
            List<V_HIS_REHA_TRAIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisRehaTrainGet(param).GetViewByServiceReqId(serviceReqId);
                }
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
