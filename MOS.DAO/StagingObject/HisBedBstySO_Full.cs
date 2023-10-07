using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBedBstySO : StagingObjectBase
    {
        public HisBedBstySO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BED_BSTY, bool>>> listHisBedBstyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BED_BSTY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BED_BSTY, bool>>> listVHisBedBstyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BED_BSTY, bool>>>();
    }
}
