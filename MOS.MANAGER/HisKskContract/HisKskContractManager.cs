using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisKskContract
{
    public partial class HisKskContractManager : BusinessBase
    {
        public HisKskContractManager()
            : base()
        {

        }

        public HisKskContractManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_KSK_CONTRACT> Get(HisKskContractFilterQuery filter)
        {
            List<HIS_KSK_CONTRACT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_KSK_CONTRACT> resultData = null;
                if (valid)
                {
                    resultData = new HisKskContractGet(param).Get(filter);
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

        
        public HIS_KSK_CONTRACT GetById(long data)
        {
            HIS_KSK_CONTRACT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_KSK_CONTRACT resultData = null;
                if (valid)
                {
                    resultData = new HisKskContractGet(param).GetById(data);
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

        
        public HIS_KSK_CONTRACT GetById(long data, HisKskContractFilterQuery filter)
        {
            HIS_KSK_CONTRACT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_KSK_CONTRACT resultData = null;
                if (valid)
                {
                    resultData = new HisKskContractGet(param).GetById(data, filter);
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

        
        public List<HIS_KSK_CONTRACT> GetByIds(List<long> data)
        {
            List<HIS_KSK_CONTRACT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_KSK_CONTRACT> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_KSK_CONTRACT>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisKskContractGet(param).GetByIds(Ids));
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
    }
}
