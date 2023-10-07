using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMaterialType
{
    public partial class HisEmteMaterialTypeManager : BusinessBase
    {
        public HisEmteMaterialTypeManager()
            : base()
        {

        }

        public HisEmteMaterialTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EMTE_MATERIAL_TYPE> Get(HisEmteMaterialTypeFilterQuery filter)
        {
             List<HIS_EMTE_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMTE_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).Get(filter);
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

        
        public  HIS_EMTE_MATERIAL_TYPE GetById(long data)
        {
             HIS_EMTE_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMTE_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).GetById(data);
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

        
        public  HIS_EMTE_MATERIAL_TYPE GetById(long data, HisEmteMaterialTypeFilterQuery filter)
        {
             HIS_EMTE_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMTE_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).GetById(data, filter);
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

        
        public  List<HIS_EMTE_MATERIAL_TYPE> GetByMaterialTypeId(long id)
        {
             List<HIS_EMTE_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_EMTE_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).GetByMaterialTypeId(id);
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

        
        public  List<HIS_EMTE_MATERIAL_TYPE> GetByExpMestTemplateId(long id)
        {
             List<HIS_EMTE_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_EMTE_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).GetByMaterialTypeId(id);
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
