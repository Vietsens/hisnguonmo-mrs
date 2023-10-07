using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisActiveIngredient
{
    public partial class HisActiveIngredientManager : BusinessBase
    {
        public HisActiveIngredientManager()
            : base()
        {

        }

        public HisActiveIngredientManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACTIVE_INGREDIENT> Get(HisActiveIngredientFilterQuery filter)
        {
             List<HIS_ACTIVE_INGREDIENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACTIVE_INGREDIENT> resultData = null;
                if (valid)
                {
                    resultData = new HisActiveIngredientGet(param).Get(filter);
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

        
        public  HIS_ACTIVE_INGREDIENT GetById(long data)
        {
             HIS_ACTIVE_INGREDIENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACTIVE_INGREDIENT resultData = null;
                if (valid)
                {
                    resultData = new HisActiveIngredientGet(param).GetById(data);
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

        
        public  HIS_ACTIVE_INGREDIENT GetById(long data, HisActiveIngredientFilterQuery filter)
        {
             HIS_ACTIVE_INGREDIENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACTIVE_INGREDIENT resultData = null;
                if (valid)
                {
                    resultData = new HisActiveIngredientGet(param).GetById(data, filter);
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

        
        public  HIS_ACTIVE_INGREDIENT GetByCode(string data)
        {
             HIS_ACTIVE_INGREDIENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACTIVE_INGREDIENT resultData = null;
                if (valid)
                {
                    resultData = new HisActiveIngredientGet(param).GetByCode(data);
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

        
        public  HIS_ACTIVE_INGREDIENT GetByCode(string data, HisActiveIngredientFilterQuery filter)
        {
             HIS_ACTIVE_INGREDIENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACTIVE_INGREDIENT resultData = null;
                if (valid)
                {
                    resultData = new HisActiveIngredientGet(param).GetByCode(data, filter);
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
