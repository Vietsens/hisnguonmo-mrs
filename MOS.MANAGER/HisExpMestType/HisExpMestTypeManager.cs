using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestType
{
    public partial class HisExpMestTypeManager : BusinessBase
    {
        public HisExpMestTypeManager()
            : base()
        {

        }

        public HisExpMestTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXP_MEST_TYPE> Get(HisExpMestTypeFilterQuery filter)
        {
             List<HIS_EXP_MEST_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTypeGet(param).Get(filter);
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

        
        public  HIS_EXP_MEST_TYPE GetById(long data)
        {
             HIS_EXP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTypeGet(param).GetById(data);
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

        
        public  HIS_EXP_MEST_TYPE GetById(long data, HisExpMestTypeFilterQuery filter)
        {
             HIS_EXP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTypeGet(param).GetById(data, filter);
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

        
        public  HIS_EXP_MEST_TYPE GetByCode(string data)
        {
             HIS_EXP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTypeGet(param).GetByCode(data);
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

        
        public  HIS_EXP_MEST_TYPE GetByCode(string data, HisExpMestTypeFilterQuery filter)
        {
             HIS_EXP_MEST_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTypeGet(param).GetByCode(data, filter);
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
