using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMaterialBeanSO : StagingObjectBase
    {
        public HisMaterialBeanSO()
        {
            //listHisMaterialBeanExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMaterialBeanExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_BEAN, bool>>> listHisMaterialBeanExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_BEAN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN, bool>>> listVHisMaterialBeanExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN_1, bool>>> listVHisMaterialBean1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN_2, bool>>> listVHisMaterialBean2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MATERIAL_BEAN_2, bool>>>();
    }
}
