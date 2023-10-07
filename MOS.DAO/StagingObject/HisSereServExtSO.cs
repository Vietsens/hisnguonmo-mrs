using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServExtSO : StagingObjectBase
    {
        public HisSereServExtSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_EXT, bool>>> listHisSereServExtExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_EXT, bool>>>();
    }
}
