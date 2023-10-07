using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPatientTypeSO : StagingObjectBase
    {
        public HisMestPatientTypeSO()
        {
            //listHisMestPatientTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMestPatientTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PATIENT_TYPE, bool>>> listHisMestPatientTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PATIENT_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PATIENT_TYPE, bool>>> listVHisMestPatientTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PATIENT_TYPE, bool>>>();
    }
}
