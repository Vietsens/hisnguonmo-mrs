using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAcinInteractive
{
    class HisAcinInteractiveGet : GetBase
    {
        internal HisAcinInteractiveGet()
            : base()
        {

        }

        internal HisAcinInteractiveGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACIN_INTERACTIVE> Get(HisAcinInteractiveFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAcinInteractiveDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_ACIN_INTERACTIVE> GetView(HisAcinInteractiveViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAcinInteractiveDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACIN_INTERACTIVE GetById(long id)
        {
            try
            {
                return GetById(id, new HisAcinInteractiveFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACIN_INTERACTIVE GetById(long id, HisAcinInteractiveFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAcinInteractiveDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ACIN_INTERACTIVE> GetByActiveIngredientIdOrActiveIngredientConflictId(long activeIngredientId)
        {
            try
            {
                HisAcinInteractiveFilterQuery filter = new HisAcinInteractiveFilterQuery();
                filter.ACTIVE_INGREDIENT_ID__OR__ACTIVE_INGREDIENT_CONFLICT_ID = activeIngredientId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_ACIN_INTERACTIVE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisAcinInteractiveViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ACIN_INTERACTIVE GetViewById(long id, HisAcinInteractiveViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAcinInteractiveDAO.GetViewById(id, filter.Query());
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
