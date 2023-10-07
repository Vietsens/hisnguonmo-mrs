using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicineSO : StagingObjectBase
    {
        public HisMedicineSO()
        {
            //listHisMedicineExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMedicineExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE, bool>>> listHisMedicineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE, bool>>> listVHisMedicineExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_1, bool>>> listVHisMedicine1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_2, bool>>> listVHisMedicine2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_2, bool>>>();
    }
}
