using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicinePaty
{
    public partial class HisMedicinePatyManager : BusinessBase
    {
        public HisMedicinePatyManager()
            : base()
        {

        }

        public HisMedicinePatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEDICINE_PATY> Get(HisMedicinePatyFilterQuery filter)
        {
            List<HIS_MEDICINE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_MEDICINE_PATY GetById(long data)
        {
            HIS_MEDICINE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_MEDICINE_PATY GetById(long data, HisMedicinePatyFilterQuery filter)
        {
            HIS_MEDICINE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDICINE_PATY> GetByPatientTypeId(long data)
        {
            List<HIS_MEDICINE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetByPatientTypeId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDICINE_PATY> GetByMedicineId(long data)
        {
            List<HIS_MEDICINE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetByMedicineId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDICINE_PATY> GetByMedicineIds(List<long> data)
        {
            List<HIS_MEDICINE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MEDICINE_PATY>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicinePatyGet(param).GetByMedicineIds(Ids));
                    }
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDICINE_PATY> GetAppliedMedicinePaty(List<long> medicineIds, long patientTypeId)
        {
            List<HIS_MEDICINE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(medicineIds);
                valid = valid && IsNotNull(patientTypeId);
                List<HIS_MEDICINE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MEDICINE_PATY>();
                    var skip = 0;
                    while (medicineIds.Count - skip > 0)
                    {
                        var Ids = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicinePatyGet(param).GetAppliedMedicinePaty(Ids, patientTypeId));
                    }
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_MEDICINE_PATY GetApplied(List<HIS_MEDICINE_PATY> hisMedicinePaties, long medicineId, long patientTypeId)
        {
            HIS_MEDICINE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(hisMedicinePaties);
                valid = valid && IsNotNull(medicineId);
                valid = valid && IsNotNull(patientTypeId);
                HIS_MEDICINE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetApplied(hisMedicinePaties, medicineId, patientTypeId);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
