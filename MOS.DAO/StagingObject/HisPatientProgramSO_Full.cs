using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPatientProgramSO : StagingObjectBase
    {
        public HisPatientProgramSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_PROGRAM, bool>>> listHisPatientProgramExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PATIENT_PROGRAM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_PROGRAM, bool>>> listVHisPatientProgramExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_PATIENT_PROGRAM, bool>>>();
    }
}
