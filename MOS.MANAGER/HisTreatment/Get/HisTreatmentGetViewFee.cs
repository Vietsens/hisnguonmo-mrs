using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.Filter;
using MOS.MANAGER.HisDepartmentTran;
using AutoMapper;
using MOS.MANAGER.HisHeinApproval;

namespace MOS.MANAGER.HisTreatment
{
    partial class HisTreatmentGet : GetBase
    {

        internal List<V_HIS_TREATMENT_FEE> GetFeeView(HisTreatmentFeeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetFeeView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TREATMENT_FEE> GetFeeViewByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisTreatmentFeeViewFilterQuery filter = new HisTreatmentFeeViewFilterQuery();
                filter.IDs = ids;
                return this.GetFeeView(filter);
            }
            return null;
        }

        internal V_HIS_TREATMENT_FEE GetFeeViewById(long id)
        {
            try
            {
                return GetFeeViewById(id, new HisTreatmentFeeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_FEE GetFeeViewById(long id, HisTreatmentFeeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetFeeViewById(id, filter.Query());
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
