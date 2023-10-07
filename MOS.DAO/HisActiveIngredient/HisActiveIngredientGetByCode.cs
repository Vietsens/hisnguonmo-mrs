using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisActiveIngredient
{
    partial class HisActiveIngredientGet : EntityBase
    {
        public HIS_ACTIVE_INGREDIENT GetByCode(string code, HisActiveIngredientSO search)
        {
            HIS_ACTIVE_INGREDIENT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_ACTIVE_INGREDIENT.AsQueryable().Where(p => p.ACTIVE_INGREDIENT_CODE == code);
                        if (search.listHisActiveIngredientExpression != null && search.listHisActiveIngredientExpression.Count > 0)
                        {
                            foreach (var item in search.listHisActiveIngredientExpression)
                            {
                                query = query.Where(item);
                            }
                        }
                        result = query.SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
