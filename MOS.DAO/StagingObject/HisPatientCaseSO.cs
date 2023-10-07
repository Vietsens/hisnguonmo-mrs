using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPatientCaseSO : StagingObjectBase
    {
        public HisPatientCaseSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_CASE, bool>>> listHisPatientCaseExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_CASE, bool>>>();
    }
}
