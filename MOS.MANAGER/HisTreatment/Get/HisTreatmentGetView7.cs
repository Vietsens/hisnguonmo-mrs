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
        internal List<V_HIS_TREATMENT_7> GetView7(HisTreatmentView7FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetView7(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_7 GetView7ById(long id)
        {
            try
            {
                return GetView7ById(id, new HisTreatmentView7FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_7 GetView7ById(long id, HisTreatmentView7FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetView7ById(id, filter.Query());
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
