using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialPaty
{
    class HisMaterialPatyGet : GetBase
    {
        internal HisMaterialPatyGet()
            : base()
        {

        }

        internal HisMaterialPatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MATERIAL_PATY> Get(HisMaterialPatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialPatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_PATY> GetView(HisMaterialPatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialPatyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL_PATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMaterialPatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL_PATY GetById(long id, HisMaterialPatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialPatyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_PATY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMaterialPatyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_PATY GetViewById(long id, HisMaterialPatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialPatyDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay "medicinePaty" duoc ap dung
        /// - Tuong ung voi medicineId, patientTypeId
        /// </summary>
        /// <param name="treatmentTime"></param>
        /// <returns></returns>
        internal HIS_MATERIAL_PATY GetApplied(List<HIS_MATERIAL_PATY> hisMaterialPaties, long materialId, long patientTypeId)
        {
            HIS_MATERIAL_PATY result = null;
            try
            {
                if (IsNotNullOrEmpty(hisMaterialPaties))
                {
                    result = hisMaterialPaties
                        .Where(o => o.PATIENT_TYPE_ID == patientTypeId && o.MATERIAL_ID == materialId)
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

        internal List<HIS_MATERIAL_PATY> GetAppliedMaterialPaty(List<long> materialIds, long patientTypeId)
        {
            HisMaterialPatyFilterQuery filter = new HisMaterialPatyFilterQuery();
            filter.MATERIAL_IDs = materialIds;
            filter.PATIENT_TYPE_ID = patientTypeId;
            return new HisMaterialPatyGet().Get(filter);
        }

        internal List<HIS_MATERIAL_PATY> GetByPatientTypeId(long patientTypeId)
        {
            try
            {
                HisMaterialPatyFilterQuery filter = new HisMaterialPatyFilterQuery();
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

        internal List<HIS_MATERIAL_PATY> GetByMaterialId(long materialId)
        {
            try
            {
                HisMaterialPatyFilterQuery filter = new HisMaterialPatyFilterQuery();
                filter.MATERIAL_ID = materialId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL_PATY> GetByMaterialIds(List<long> materialIds)
        {
            if (IsNotNullOrEmpty(materialIds))
            {
                HisMaterialPatyFilterQuery filter = new HisMaterialPatyFilterQuery();
                filter.MATERIAL_IDs = materialIds;
                return this.Get(filter);
            }
            return null;
        }
    }
}
