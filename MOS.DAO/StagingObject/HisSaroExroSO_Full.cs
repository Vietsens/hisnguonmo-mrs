using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisSaroExroSO : StagingObjectBase
    {
        public HisSaroExroSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SARO_EXRO, bool>>> listHisSaroExroExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SARO_EXRO, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SARO_EXRO, bool>>> listVHisSaroExroExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SARO_EXRO, bool>>>();
    }
}
