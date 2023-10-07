using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServDepositSO : StagingObjectBase
    {
        public HisSereServDepositSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_DEPOSIT, bool>>> listHisSereServDepositExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_DEPOSIT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_DEPOSIT, bool>>> listVHisSereServDepositExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_DEPOSIT, bool>>>();
    }
}
