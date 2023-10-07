using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBloodVolumeSO : StagingObjectBase
    {
        public HisBloodVolumeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_VOLUME, bool>>> listHisBloodVolumeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BLOOD_VOLUME, bool>>>();
    }
}
