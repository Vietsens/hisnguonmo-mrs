using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRehaTrainSO : StagingObjectBase
    {
        public HisRehaTrainSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN, bool>>> listHisRehaTrainExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_REHA_TRAIN, bool>>> listVHisRehaTrainExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REHA_TRAIN, bool>>>();
    }
}
