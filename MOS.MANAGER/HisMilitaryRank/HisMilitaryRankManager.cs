using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMilitaryRank
{
    public partial class HisMilitaryRankManager : BusinessBase
    {
        public HisMilitaryRankManager()
            : base()
        {

        }

        public HisMilitaryRankManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MILITARY_RANK> Get(HisMilitaryRankFilterQuery filter)
        {
             List<HIS_MILITARY_RANK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MILITARY_RANK> resultData = null;
                if (valid)
                {
                    resultData = new HisMilitaryRankGet(param).Get(filter);
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

        
        public  HIS_MILITARY_RANK GetById(long data)
        {
             HIS_MILITARY_RANK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MILITARY_RANK resultData = null;
                if (valid)
                {
                    resultData = new HisMilitaryRankGet(param).GetById(data);
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

        
        public  HIS_MILITARY_RANK GetById(long data, HisMilitaryRankFilterQuery filter)
        {
             HIS_MILITARY_RANK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MILITARY_RANK resultData = null;
                if (valid)
                {
                    resultData = new HisMilitaryRankGet(param).GetById(data, filter);
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

        
        public  HIS_MILITARY_RANK GetByCode(string data)
        {
             HIS_MILITARY_RANK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MILITARY_RANK resultData = null;
                if (valid)
                {
                    resultData = new HisMilitaryRankGet(param).GetByCode(data);
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

        
        public  HIS_MILITARY_RANK GetByCode(string data, HisMilitaryRankFilterQuery filter)
        {
             HIS_MILITARY_RANK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MILITARY_RANK resultData = null;
                if (valid)
                {
                    resultData = new HisMilitaryRankGet(param).GetByCode(data, filter);
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
