using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEkipUserSO : StagingObjectBase
    {
        public HisEkipUserSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EKIP_USER, bool>>> listHisEkipUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EKIP_USER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EKIP_USER, bool>>> listVHisEkipUserExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EKIP_USER, bool>>>();
    }
}
