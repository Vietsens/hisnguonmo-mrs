using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisTreatmentBedRoom
{
    partial class HisTreatmentBedRoomGet : EntityBase
    {
        public V_HIS_TREATMENT_BED_ROOM GetViewById(long id, HisTreatmentBedRoomSO search)
        {
            V_HIS_TREATMENT_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_TREATMENT_BED_ROOM.AsQueryable().Where(p => p.ID == id);
                        if (search.listVHisTreatmentBedRoomExpression != null && search.listVHisTreatmentBedRoomExpression.Count > 0)
                        {
                            foreach (var item in search.listVHisTreatmentBedRoomExpression)
                            {
                                query = query.Where(item);
                            }
                        }
                        result = query.SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => id), id) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
