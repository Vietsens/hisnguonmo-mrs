using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMedicineType
{
    public partial class HisEmteMedicineTypeManager : BusinessBase
    {
        public HisEmteMedicineTypeManager()
            : base()
        {

        }

        public HisEmteMedicineTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EMTE_MEDICINE_TYPE> Get(HisEmteMedicineTypeFilterQuery filter)
        {
             List<HIS_EMTE_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMTE_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).Get(filter);
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

        
        public  HIS_EMTE_MEDICINE_TYPE GetById(long data)
        {
             HIS_EMTE_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMTE_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).GetById(data);
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

        
        public  HIS_EMTE_MEDICINE_TYPE GetById(long data, HisEmteMedicineTypeFilterQuery filter)
        {
             HIS_EMTE_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMTE_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).GetById(data, filter);
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

        
        public  List<HIS_EMTE_MEDICINE_TYPE> GetByMedicineTypeId(long filter)
        {
             List<HIS_EMTE_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMTE_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).GetByMedicineTypeId(filter);
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

        
        public  List<HIS_EMTE_MEDICINE_TYPE> GetByExpMestTemplateId(long filter)
        {
             List<HIS_EMTE_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMTE_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).GetByExpMestTemplateId(filter);
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
