using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPatientTypeAlterSO : StagingObjectBase
    {
        public HisPatientTypeAlterSO()
        {
            listHisPatientTypeAlterExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisPatientTypeAlterExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALTER, bool>>> listHisPatientTypeAlterExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_TYPE_ALTER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALTER, bool>>> listVHisPatientTypeAlterExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_TYPE_ALTER, bool>>>();
    }
}
