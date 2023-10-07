using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMedicineGroupSO : StagingObjectBase
    {
        public HisMedicineGroupSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_GROUP, bool>>> listHisMedicineGroupExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_GROUP, bool>>>();
    }
}
