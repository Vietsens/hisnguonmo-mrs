using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServTempSO : StagingObjectBase
    {
        public HisSereServTempSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEMP, bool>>> listHisSereServTempExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEMP, bool>>>();
    }
}
