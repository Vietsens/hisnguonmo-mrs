using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicineLineSO : StagingObjectBase
    {
        public HisMedicineLineSO()
        {
            listHisMedicineLineExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_LINE, bool>>> listHisMedicineLineExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_LINE, bool>>>();
    }
}
