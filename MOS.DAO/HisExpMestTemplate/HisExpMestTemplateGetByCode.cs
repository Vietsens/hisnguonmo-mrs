using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisExpMestTemplate
{
    partial class HisExpMestTemplateGet : EntityBase
    {
        public HIS_EXP_MEST_TEMPLATE GetByCode(string code, HisExpMestTemplateSO search)
        {
            HIS_EXP_MEST_TEMPLATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_EXP_MEST_TEMPLATE.AsQueryable().Where(p => p.EXP_MEST_TEMPLATE_CODE == code);
                        if (search.listHisExpMestTemplateExpression != null && search.listHisExpMestTemplateExpression.Count > 0)
                        {
                            foreach (var item in search.listHisExpMestTemplateExpression)
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
