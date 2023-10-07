using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaSum
{
    public partial class HisRehaSumManager : BusinessBase
    {
        public HisRehaSumManager()
            : base()
        {

        }

        public HisRehaSumManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_REHA_SUM> Get(HisRehaSumFilterQuery filter)
        {
            List<HIS_REHA_SUM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REHA_SUM> resultData = null;
                if (valid)
                {
                    resultData = new HisRehaSumGet(param).Get(filter);
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

        
        public HIS_REHA_SUM GetById(long data)
        {
            HIS_REHA_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REHA_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisRehaSumGet(param).GetById(data);
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

        
        public HIS_REHA_SUM GetById(long data, HisRehaSumFilterQuery filter)
        {
            HIS_REHA_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REHA_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisRehaSumGet(param).GetById(data, filter);
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

        
        public List<HIS_REHA_SUM> GetByTreatmentId(long data)
        {
            List<HIS_REHA_SUM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_REHA_SUM> resultData = null;
                if (valid)
                {
                    resultData = new HisRehaSumGet(param).GetByTreatmentId(data);
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
