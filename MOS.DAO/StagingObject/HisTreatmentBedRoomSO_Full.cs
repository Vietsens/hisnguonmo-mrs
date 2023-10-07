using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTreatmentBedRoomSO : StagingObjectBase
    {
        public HisTreatmentBedRoomSO()
        {
            //listHisTreatmentBedRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisTreatmentBedRoomExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_BED_ROOM, bool>>> listHisTreatmentBedRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_BED_ROOM, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_BED_ROOM, bool>>> listVHisTreatmentBedRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_BED_ROOM, bool>>>();
    }
}
