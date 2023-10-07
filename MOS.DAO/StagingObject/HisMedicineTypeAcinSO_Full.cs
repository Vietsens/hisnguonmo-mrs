using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicineTypeAcinSO : StagingObjectBase
    {
        public HisMedicineTypeAcinSO()
        {
            //listHisMedicineTypeAcinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMedicineTypeAcinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_TYPE_ACIN, bool>>> listHisMedicineTypeAcinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_TYPE_ACIN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_TYPE_ACIN, bool>>> listVHisMedicineTypeAcinExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_TYPE_ACIN, bool>>>();
    }
}
