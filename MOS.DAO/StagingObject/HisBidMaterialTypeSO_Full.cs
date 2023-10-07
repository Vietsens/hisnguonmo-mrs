using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBidMaterialTypeSO : StagingObjectBase
    {
        public HisBidMaterialTypeSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BID_MATERIAL_TYPE, bool>>> listHisBidMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BID_MATERIAL_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MATERIAL_TYPE, bool>>> listVHisBidMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MATERIAL_TYPE, bool>>>();
    }
}
