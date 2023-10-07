using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisKskContractSO : StagingObjectBase
    {
        public HisKskContractSO()
        {
            listHisKskContractExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_KSK_CONTRACT, bool>>> listHisKskContractExpression = new List<System.Linq.Expressions.Expression<Func<HIS_KSK_CONTRACT, bool>>>();
    }
}
