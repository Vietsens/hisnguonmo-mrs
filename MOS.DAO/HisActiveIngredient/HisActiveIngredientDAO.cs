using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisActiveIngredient
{
    public partial class HisActiveIngredientDAO : EntityBase
    {
        private HisActiveIngredientGet GetWorker
        {
            get
            {
                return (HisActiveIngredientGet)Worker.Get<HisActiveIngredientGet>();
            }
        }
        public List<HIS_ACTIVE_INGREDIENT> Get(HisActiveIngredientSO search, CommonParam param)
        {
            List<HIS_ACTIVE_INGREDIENT> result = new List<HIS_ACTIVE_INGREDIENT>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_ACTIVE_INGREDIENT GetById(long id, HisActiveIngredientSO search)
        {
            HIS_ACTIVE_INGREDIENT result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
