using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicineBeanSO : StagingObjectBase
    {
        public HisMedicineBeanSO()
        {
            //listHisMedicineBeanExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMedicineBeanExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_BEAN, bool>>> listHisMedicineBeanExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_BEAN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN, bool>>> listVHisMedicineBeanExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_1, bool>>> listVHisMedicineBean1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_2, bool>>> listVHisMedicineBean2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_BEAN_2, bool>>>();
    }
}
