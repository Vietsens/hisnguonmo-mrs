using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEmteMaterialTypeSO : StagingObjectBase
    {
        public HisEmteMaterialTypeSO()
        {
            //listHisEmteMaterialTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisEmteMaterialTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MATERIAL_TYPE, bool>>> listHisEmteMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MATERIAL_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EMTE_MATERIAL_TYPE, bool>>> listVHisEmteMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EMTE_MATERIAL_TYPE, bool>>>();
    }
}
