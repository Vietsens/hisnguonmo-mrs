using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialPaty
{
    public partial class HisMaterialPatyManager : BusinessBase
    {
        public HisMaterialPatyManager()
            : base()
        {

        }

        public HisMaterialPatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MATERIAL_PATY> Get(HisMaterialPatyFilterQuery filter)
        {
            List<HIS_MATERIAL_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MATERIAL_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).Get(filter);
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

        
        public HIS_MATERIAL_PATY GetById(long data)
        {
            HIS_MATERIAL_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MATERIAL_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetById(data);
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

        
        public HIS_MATERIAL_PATY GetById(long data, HisMaterialPatyFilterQuery filter)
        {
            HIS_MATERIAL_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MATERIAL_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetById(data, filter);
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

        
        public HIS_MATERIAL_PATY GetApplied(List<HIS_MATERIAL_PATY> hisMaterialPaties, long materialId, long patientTypeId)
        {
            HIS_MATERIAL_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(hisMaterialPaties);
                valid = valid && IsNotNull(materialId);
                valid = valid && IsNotNull(patientTypeId);
                HIS_MATERIAL_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetApplied(hisMaterialPaties, materialId, patientTypeId);
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

        
        public List<HIS_MATERIAL_PATY> GetAppliedMaterialPaty(List<long> materialIds, long patientTypeId)
        {
            List<HIS_MATERIAL_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(materialIds);
                valid = valid && IsNotNull(patientTypeId);
                List<HIS_MATERIAL_PATY> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MATERIAL_PATY>();
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var Ids = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMaterialPatyGet(param).GetAppliedMaterialPaty(Ids, patientTypeId));
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

        
        public List<HIS_MATERIAL_PATY> GetByPatientTypeId(long patientTypeId)
        {
            List<HIS_MATERIAL_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(patientTypeId);
                List<HIS_MATERIAL_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetByPatientTypeId(patientTypeId);
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

        
        public List<HIS_MATERIAL_PATY> GetByMaterialId(long materialId)
        {
            List<HIS_MATERIAL_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(materialId);
                List<HIS_MATERIAL_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetByMaterialId(materialId);
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

        
        public List<HIS_MATERIAL_PATY> GetByMaterialIds(List<long> materialIds)
        {
            List<HIS_MATERIAL_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(materialIds);
                List<HIS_MATERIAL_PATY> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MATERIAL_PATY>();
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var Ids = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMaterialPatyGet(param).GetByMaterialIds(Ids));
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
    }
}
