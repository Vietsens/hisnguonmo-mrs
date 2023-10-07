using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRehaTrainTypeSO : StagingObjectBase
    {
        public HisRehaTrainTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN_TYPE, bool>>> listHisRehaTrainTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_REHA_TRAIN_TYPE, bool>>> listVHisRehaTrainTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REHA_TRAIN_TYPE, bool>>>();
    }
}
