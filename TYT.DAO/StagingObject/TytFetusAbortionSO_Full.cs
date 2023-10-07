using TYT.DAO.Base;
using TYT.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace TYT.DAO.StagingObject
{
    public class TytFetusAbortionSO : StagingObjectBase
    {
        public TytFetusAbortionSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<TYT_FETUS_ABORTION, bool>>> listTytFetusAbortionExpression = new List<System.Linq.Expressions.Expression<Func<TYT_FETUS_ABORTION, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_TYT_FETUS_ABORTION, bool>>> listVTytFetusAbortionExpression = new List<System.Linq.Expressions.Expression<Func<V_TYT_FETUS_ABORTION, bool>>>();
    }
}
