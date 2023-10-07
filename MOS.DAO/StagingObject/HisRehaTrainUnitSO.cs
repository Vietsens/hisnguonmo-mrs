using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRehaTrainUnitSO : StagingObjectBase
    {
        public HisRehaTrainUnitSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN_UNIT, bool>>> listHisRehaTrainUnitExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN_UNIT, bool>>>();
    }
}
