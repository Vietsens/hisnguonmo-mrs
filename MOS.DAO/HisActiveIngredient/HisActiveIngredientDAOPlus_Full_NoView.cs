using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisActiveIngredient
{
    public partial class HisActiveIngredientDAO : EntityBase
    {
        public HIS_ACTIVE_INGREDIENT GetByCode(string code, HisActiveIngredientSO search)
        {
            HIS_ACTIVE_INGREDIENT result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_ACTIVE_INGREDIENT> GetDicByCode(HisActiveIngredientSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACTIVE_INGREDIENT> result = new Dictionary<string, HIS_ACTIVE_INGREDIENT>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
