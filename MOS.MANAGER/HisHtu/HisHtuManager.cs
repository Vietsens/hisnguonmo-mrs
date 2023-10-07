using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHtu
{
    public partial class HisHtuManager : BusinessBase
    {
        public HisHtuManager()
            : base()
        {

        }

        public HisHtuManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_HTU> Get(HisHtuFilterQuery filter)
        {
            List<HIS_HTU> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_HTU> resultData = null;
                if (valid)
                {
                    resultData = new HisHtuGet(param).Get(filter);
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

        
        public HIS_HTU GetById(long data)
        {
            HIS_HTU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_HTU resultData = null;
                if (valid)
                {
                    resultData = new HisHtuGet(param).GetById(data);
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

        
        public HIS_HTU GetById(long data, HisHtuFilterQuery filter)
        {
            HIS_HTU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_HTU resultData = null;
                if (valid)
                {
                    resultData = new HisHtuGet(param).GetById(data, filter);
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
