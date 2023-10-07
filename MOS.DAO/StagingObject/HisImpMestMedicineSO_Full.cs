using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestMedicineSO : StagingObjectBase
    {
        public HisImpMestMedicineSO()
        {
            //listHisImpMestMedicineExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisImpMestMedicineExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MEDICINE, bool>>> listHisImpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MEDICINE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE, bool>>> listVHisImpMestMedicineExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_2, bool>>> listVHisImpMestMedicine2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_2, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_3, bool>>> listVHisImpMestMedicine3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MEDICINE_3, bool>>>();
    }
}
