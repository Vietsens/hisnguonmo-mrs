using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisEkipTempUserSO : StagingObjectBase
    {
        public HisEkipTempUserSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EKIP_TEMP_USER, bool>>> listHisEkipTempUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EKIP_TEMP_USER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EKIP_TEMP_USER, bool>>> listVHisEkipTempUserExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EKIP_TEMP_USER, bool>>>();
    }
}
