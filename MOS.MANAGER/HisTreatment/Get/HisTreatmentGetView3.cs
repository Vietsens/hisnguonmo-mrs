using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.Filter;
using AutoMapper;

namespace MOS.MANAGER.HisTreatment
{
    partial class HisTreatmentGet : GetBase
    {
        internal List<V_HIS_TREATMENT_3> GetView3(HisTreatmentView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetView3(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_3 GetView3ById(long id)
        {
            try
            {
                return GetView3ById(id, new HisTreatmentView3FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_3 GetView3ById(long id, HisTreatmentView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetView3ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
