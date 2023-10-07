using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPatientSO : StagingObjectBase
    {
        public HisPatientSO()
        {
            //listHisPatientExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisPatientExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PATIENT, bool>>> listHisPatientExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT, bool>>> listVHisPatientExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT, bool>>>();
    }
}
