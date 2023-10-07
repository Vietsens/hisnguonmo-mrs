using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMediOrgSO : StagingObjectBase
    {
        public HisMediOrgSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEDI_ORG, bool>>> listHisMediOrgExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_ORG, bool>>>();
    }
}
