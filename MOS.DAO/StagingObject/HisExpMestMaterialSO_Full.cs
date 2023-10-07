using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestMaterialSO : StagingObjectBase
    {
        public HisExpMestMaterialSO()
        {
            listHisExpMestMaterialExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMaterialExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMaterial1Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMaterial3Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMaterial4Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATERIAL, bool>>> listHisExpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATERIAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL, bool>>> listVHisExpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_1, bool>>> listVHisExpMestMaterial1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_3, bool>>> listVHisExpMestMaterial3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_3, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_4, bool>>> listVHisExpMestMaterial4Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_4, bool>>>();
    }
}
