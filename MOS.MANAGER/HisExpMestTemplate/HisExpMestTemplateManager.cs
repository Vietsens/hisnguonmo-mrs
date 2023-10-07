using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestTemplate
{
    public partial class HisExpMestTemplateManager : BusinessBase
    {
        public HisExpMestTemplateManager()
            : base()
        {

        }

        public HisExpMestTemplateManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXP_MEST_TEMPLATE> Get(HisExpMestTemplateFilterQuery filter)
        {
             List<HIS_EXP_MEST_TEMPLATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_TEMPLATE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTemplateGet(param).Get(filter);
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

        
        public  HIS_EXP_MEST_TEMPLATE GetById(long data)
        {
             HIS_EXP_MEST_TEMPLATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_TEMPLATE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTemplateGet(param).GetById(data);
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

        
        public  HIS_EXP_MEST_TEMPLATE GetById(long data, HisExpMestTemplateFilterQuery filter)
        {
             HIS_EXP_MEST_TEMPLATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_TEMPLATE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTemplateGet(param).GetById(data, filter);
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

        
        public  HIS_EXP_MEST_TEMPLATE GetByCode(string data)
        {
             HIS_EXP_MEST_TEMPLATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_TEMPLATE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTemplateGet(param).GetByCode(data);
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

        
        public  HIS_EXP_MEST_TEMPLATE GetByCode(string data, HisExpMestTemplateFilterQuery filter)
        {
             HIS_EXP_MEST_TEMPLATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_TEMPLATE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestTemplateGet(param).GetByCode(data, filter);
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
