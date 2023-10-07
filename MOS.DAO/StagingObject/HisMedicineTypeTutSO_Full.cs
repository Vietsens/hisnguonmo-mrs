using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicineTypeTutSO : StagingObjectBase
    {
        public HisMedicineTypeTutSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_TYPE_TUT, bool>>> listHisMedicineTypeTutExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_TYPE_TUT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_TYPE_TUT, bool>>> listVHisMedicineTypeTutExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDICINE_TYPE_TUT, bool>>>();
    }
}
