using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEmteMedicineTypeSO : StagingObjectBase
    {
        public HisEmteMedicineTypeSO()
        {
            //listHisEmteMedicineTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisEmteMedicineTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MEDICINE_TYPE, bool>>> listHisEmteMedicineTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MEDICINE_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EMTE_MEDICINE_TYPE, bool>>> listVHisEmteMedicineTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EMTE_MEDICINE_TYPE, bool>>>();
    }
}
