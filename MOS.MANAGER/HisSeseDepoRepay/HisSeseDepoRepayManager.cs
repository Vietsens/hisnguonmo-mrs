using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSeseDepoRepay
{
    public partial class HisSeseDepoRepayManager : BusinessBase
    {
        public HisSeseDepoRepayManager()
            : base()
        {

        }

        public HisSeseDepoRepayManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SESE_DEPO_REPAY> Get(HisSeseDepoRepayFilterQuery filter)
        {
            List<HIS_SESE_DEPO_REPAY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SESE_DEPO_REPAY> resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).Get(filter);
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

        
        public HIS_SESE_DEPO_REPAY GetById(long data)
        {
            HIS_SESE_DEPO_REPAY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SESE_DEPO_REPAY resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetById(data);
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

        
        public HIS_SESE_DEPO_REPAY GetById(long data, HisSeseDepoRepayFilterQuery filter)
        {
            HIS_SESE_DEPO_REPAY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SESE_DEPO_REPAY resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetById(data, filter);
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

        
        public List<HIS_SESE_DEPO_REPAY> GetBySereServDepositIds(List<long> data)
        {
            List<HIS_SESE_DEPO_REPAY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SESE_DEPO_REPAY> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SESE_DEPO_REPAY>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSeseDepoRepayGet(param).GetBySereServDepositIds(Ids));
                    }
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

        
        public List<HIS_SESE_DEPO_REPAY> GetNoCancelBySereServDepositIds(List<long> data)
        {
            List<HIS_SESE_DEPO_REPAY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SESE_DEPO_REPAY> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SESE_DEPO_REPAY>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSeseDepoRepayGet(param).GetNoCancelBySereServDepositIds(Ids));
                    }
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

        
        public List<HIS_SESE_DEPO_REPAY> GetNoCancelBySereServDepositId(long data)
        {
            List<HIS_SESE_DEPO_REPAY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SESE_DEPO_REPAY> resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetNoCancelBySereServDepositId(data);
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

        
        public List<HIS_SESE_DEPO_REPAY> GetByRepayId(long data)
        {
            List<HIS_SESE_DEPO_REPAY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SESE_DEPO_REPAY> resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetByRepayId(data);
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
