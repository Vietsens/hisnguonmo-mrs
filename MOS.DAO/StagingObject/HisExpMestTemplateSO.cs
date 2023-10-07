using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestTemplateSO : StagingObjectBase
    {
        public HisExpMestTemplateSO()
        {
            //listHisExpMestTemplateExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_TEMPLATE, bool>>> listHisExpMestTemplateExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_TEMPLATE, bool>>>();
    }
}
