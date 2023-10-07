using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestMedicineSO : StagingObjectBase
    {
        public HisExpMestMedicineSO()
        {
            listHisExpMestMedicineExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMedicineExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMedicine1Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMedicine3Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisExpMestMedicine4Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MEDICINE, bool>>> listHisExpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MEDICINE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE, bool>>> listVHisExpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_1, bool>>> listVHisExpMestMedicine1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_3, bool>>> listVHisExpMestMedicine3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_3, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_4, bool>>> listVHisExpMestMedicine4Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_4, bool>>>();
    }
}
