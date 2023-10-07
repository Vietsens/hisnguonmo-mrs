using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicinePatySO : StagingObjectBase
    {
        public HisMedicinePatySO()
        {
            //listHisMedicinePatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMedicinePatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_PATY, bool>>> listHisMedicinePatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_PATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_PATY, bool>>> listVHisMedicinePatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_PATY, bool>>>();
    }
}
