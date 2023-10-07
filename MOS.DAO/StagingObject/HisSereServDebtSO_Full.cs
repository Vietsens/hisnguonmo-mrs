using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServDebtSO : StagingObjectBase
    {
        public HisSereServDebtSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_DEBT, bool>>> listHisSereServDebtExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_DEBT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_DEBT, bool>>> listVHisSereServDebtExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_DEBT, bool>>>();
    }
}
