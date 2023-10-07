using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicinePaty
{
    class HisMedicinePatyGet : GetBase
    {
        internal HisMedicinePatyGet()
            : base()
        {

        }

        internal HisMedicinePatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_PATY> Get(HisMedicinePatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicinePatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDICINE_PATY> GetView(HisMedicinePatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicinePatyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_PATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicinePatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_PATY GetById(long id, HisMedicinePatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicinePatyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEDICINE_PATY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMedicinePatyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_PATY GetViewById(long id, HisMedicinePatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicinePatyDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_PATY> GetAppliedMedicinePaty(List<long> medicineIds, long patientTypeId)
        {
            HisMedicinePatyFilterQuery filter = new HisMedicinePatyFilterQuery();
            filter.MEDICINE_IDs = medicineIds;
            filter.PATIENT_TYPE_ID = patientTypeId;
            return new HisMedicinePatyGet().Get(filter);
        }

        internal List<HIS_MEDICINE_PATY> GetByPatientTypeId(long patientTypeId)
        {
            try
            {
                HisMedicinePatyFilterQuery filter = new HisMedicinePatyFilterQuery();
                filter.PATIENT_TYPE_ID = patientTypeId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_PATY> GetByMedicineId(long medicineId)
        {
            HisMedicinePatyFilterQuery filter = new HisMedicinePatyFilterQuery();
            filter.MEDICINE_ID = medicineId;
            return this.Get(filter);
        }

        internal List<HIS_MEDICINE_PATY> GetByMedicineIds(List<long> medicineIds)
        {
            if (IsNotNullOrEmpty(medicineIds))
            {
                HisMedicinePatyFilterQuery filter = new HisMedicinePatyFilterQuery();
                filter.MEDICINE_IDs = medicineIds;
                return this.Get(filter);
            }
            return null;
        }

        /// <summary>
        /// Lay "medicinePaty" duoc ap dung
        /// - Trong danh sach medicinePaty truyen vao
        /// - Dang co hieu luc neu dap ung 1 trong 2 dieu kien sau:
        ///     + Thoi gian chi dinh nam trong khoang (from_time, to_time)
        ///     + Thoi gian dieu tri nam trong khoang (treatment_from_time, treatment_to_time)
        /// - Co priority lon nhat
        /// - Tuong ung voi medicineId, patientTypeId
        /// </summary>
        /// <param name="treatmentTime"></param>
        /// <returns></returns>
        internal HIS_MEDICINE_PATY GetApplied(List<HIS_MEDICINE_PATY> hisMedicinePaties, long medicineId, long patientTypeId)
        {
            HIS_MEDICINE_PATY result = null;
            try
            {
                if (IsNotNullOrEmpty(hisMedicinePaties))
                {
                    result = hisMedicinePaties
                        .Where(o => o.PATIENT_TYPE_ID == patientTypeId && o.MEDICINE_ID == medicineId)
                        .FirstOrDefault();
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
            }
            return result;
        }
    }
}
