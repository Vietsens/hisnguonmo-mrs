using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisActiveIngredient
{
    class HisActiveIngredientGet : GetBase
    {
        internal HisActiveIngredientGet()
            : base()
        {

        }

        internal HisActiveIngredientGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACTIVE_INGREDIENT> Get(HisActiveIngredientFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisActiveIngredientDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACTIVE_INGREDIENT GetById(long id)
        {
            try
            {
                return GetById(id, new HisActiveIngredientFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACTIVE_INGREDIENT GetById(long id, HisActiveIngredientFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisActiveIngredientDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACTIVE_INGREDIENT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisActiveIngredientFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACTIVE_INGREDIENT GetByCode(string code, HisActiveIngredientFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisActiveIngredientDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
