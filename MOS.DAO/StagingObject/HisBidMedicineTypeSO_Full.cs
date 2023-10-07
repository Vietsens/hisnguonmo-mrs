using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBidMedicineTypeSO : StagingObjectBase
    {
        public HisBidMedicineTypeSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BID_MEDICINE_TYPE, bool>>> listHisBidMedicineTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BID_MEDICINE_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MEDICINE_TYPE, bool>>> listVHisBidMedicineTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MEDICINE_TYPE, bool>>>();
    }
}
