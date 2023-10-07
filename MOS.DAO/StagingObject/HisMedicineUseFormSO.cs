using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicineUseFormSO : StagingObjectBase
    {
        public HisMedicineUseFormSO()
        {
            //listHisMedicineUseFormExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_USE_FORM, bool>>> listHisMedicineUseFormExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_USE_FORM, bool>>>();
    }
}
