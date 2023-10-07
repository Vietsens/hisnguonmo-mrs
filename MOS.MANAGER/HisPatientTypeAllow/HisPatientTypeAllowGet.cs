using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisPatientTypeAllow
{
    class HisPatientTypeAllowGet : BusinessBase
    {
        internal HisPatientTypeAllowGet()
            : base()
        {

        }

        internal HisPatientTypeAllowGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PATIENT_TYPE_ALLOW> Get(HisPatientTypeAllowFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeAllowDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_PATIENT_TYPE_ALLOW> GetView(HisPatientTypeAllowViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeAllowDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay danh sach patient_type_id cho phep chuyen doi dua vao patient_type_id truyen vao
        /// </summary>
        /// <param name="patientTypeId"></param>
        /// <returns></returns>
        internal List<long> GetPatientTypeAllowId(long patientTypeId)
        {
            try
            {
                HisPatientTypeAllowFilterQuery filter = new HisPatientTypeAllowFilterQuery();
                filter.PATIENT_TYPE_ID = patientTypeId;
                List<HIS_PATIENT_TYPE_ALLOW> data = this.Get(filter);
                return data != null ? data.Select(o => o.PATIENT_TYPE_ALLOW_ID).ToList() : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_ALLOW GetById(long id)
        {
            try
            {
                return GetById(id, new HisPatientTypeAllowFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_ALLOW GetById(long id, HisPatientTypeAllowFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeAllowDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALLOW> GetByPatientTypeIdOrPatientTypeAllowId(long patientTypeId)
        {
            try
            {
                HisPatientTypeAllowFilterQuery filter = new HisPatientTypeAllowFilterQuery();
                filter.PATIENT_TYPE_ID__OR__PATIENT_TYPE_ALLOW_ID = patientTypeId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALLOW> GetActive()
        {
            try
            {
                HisPatientTypeAllowFilterQuery filter = new HisPatientTypeAllowFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.Get(filter);
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
