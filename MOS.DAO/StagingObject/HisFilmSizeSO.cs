using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisFilmSizeSO : StagingObjectBase
    {
        public HisFilmSizeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_FILM_SIZE, bool>>> listHisFilmSizeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_FILM_SIZE, bool>>>();
    }
}
