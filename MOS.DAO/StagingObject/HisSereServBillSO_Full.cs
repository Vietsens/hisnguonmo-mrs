using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSereServBillSO : StagingObjectBase
    {
        public HisSereServBillSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_BILL, bool>>> listHisSereServBillExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_BILL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_BILL, bool>>> listVHisSereServBillExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_BILL, bool>>>();
    }
}
