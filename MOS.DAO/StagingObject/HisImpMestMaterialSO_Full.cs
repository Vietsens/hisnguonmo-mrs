using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestMaterialSO : StagingObjectBase
    {
        public HisImpMestMaterialSO()
        {
            //listHisImpMestMaterialExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisImpMestMaterialExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MATERIAL, bool>>> listHisImpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MATERIAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL, bool>>> listVHisImpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL_2, bool>>> listVHisImpMestMaterial2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL_2, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL_3, bool>>> listVHisImpMestMaterial3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL_3, bool>>>();
    }
}
