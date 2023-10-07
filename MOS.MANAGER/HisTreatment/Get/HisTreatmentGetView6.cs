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
        internal List<V_HIS_TREATMENT_6> GetView6(HisTreatmentView6FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetView6(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_6 GetView6ById(long id)
        {
            try
            {
                return GetView6ById(id, new HisTreatmentView6FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_6 GetView6ById(long id, HisTreatmentView6FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetView6ById(id, filter.Query());
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
